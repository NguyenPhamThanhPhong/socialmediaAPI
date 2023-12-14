using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using socialmediaAPI.Configs;

namespace socialmediaAPI.Services.CloudinaryService
{
    public class CloudinaryHandler
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryHandler(CloudinaryConfigs cloudinaryConfig)
        {
            Account account = new Account(
                cloudinaryConfig.CloudName,
                cloudinaryConfig.APIKey,
                cloudinaryConfig.APISecretKey);

            _cloudinary = new Cloudinary(account);
        }

        public async Task<Dictionary<string, string?>> UploadImages(List<IFormFile> files, string folderName)
        {
            var result = new Dictionary<string, string?>();

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    using (var stream = file.OpenReadStream())
                    {
                        string fileName = Guid.NewGuid().ToString();
                        var uploadParams = new ImageUploadParams
                        {
                            File = new FileDescription(fileName, stream),
                            Folder = folderName
                        };
                        var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                        result.Add(file.FileName, uploadResult.SecureUri.AbsoluteUri);
                    }
                }
            }
            return result;
        }
        public async Task<string?> UploadSingleImage(IFormFile file, string folderName)
        {
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(fileName, stream),
                        Folder = folderName
                    };
                    var uploadResult = await _cloudinary.UploadAsync(uploadParams);
                    return (uploadResult.SecureUri).AbsoluteUri;
                }
            }
            return null;
        }
        public async Task Delete(string? Url)
        {
            if (string.IsNullOrEmpty(Url))
                return;
            var publicId = GetPublicIdFromUrl(Url);

            if (!string.IsNullOrEmpty(publicId))
            {
                var deletionParams = new DeletionParams(publicId);

                var deletionResult = await _cloudinary.DestroyAsync(deletionParams);

                // Check if the deletion was successful
                if (deletionResult.Result.ToLower() == "ok")
                {
                    Console.WriteLine($"File deleted successfully. Public ID: {publicId}");
                }
                else
                {
                    Console.WriteLine($"failed Public ID: {publicId}");
                    Console.WriteLine($"Failed to delete file. Error: {deletionResult.Result}");
                    Console.WriteLine($"Failed to delete file. Error: {deletionResult.Error?.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Cloudinary URL");
            }
        }


        private string? GetPublicIdFromUrl(string imageUrl)
        {
            // Example Cloudinary URL format: https://res.cloudinary.com/{cloudName}/image/upload/v{version}/{publicId}.{format}
            var uri = new System.Uri(imageUrl);
            var segments = uri.Segments;

            // Check if there are at least five segments (assuming the URL structure)
            if (segments.Length >= 5)
            {
                // Extract the public ID from the URL (considering the version and file extension)
                var publicIdWithVersionAndFormat = segments[segments.Length - 1];
                var publicId = Path.GetFileNameWithoutExtension(publicIdWithVersionAndFormat);

                // If there is a version, remove it from the public ID
                var version = segments[segments.Length - 3];
                if (version.StartsWith("v"))
                {
                    publicId = publicId.Replace(version, "");
                }

                return publicId;
            }

            return null;
        }
    }
}
