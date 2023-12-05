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
                if (deletionResult.Result == "ok")
                {
                    Console.WriteLine($"File deleted successfully. Public ID: {publicId}");
                }
                else
                {
                    Console.WriteLine($"Failed to delete file. Error: {deletionResult.Error.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid Cloudinary URL");
            }
        }

        private string? GetPublicIdFromUrl(string imageUrl)
        {
            // Example Cloudinary URL format: https://res.cloudinary.com/{cloudName}/image/upload/{publicId}.{format}
            var uri = new System.Uri(imageUrl);
            var segments = uri.Segments;

            if (segments.Length >= 3)
            {
                // Extract the public ID from the URL
                var publicIdWithFormat = segments[segments.Length - 1];
                var publicId = Path.GetFileNameWithoutExtension(publicIdWithFormat);
                return publicId;
            }

            return null;
        }
    }
}
