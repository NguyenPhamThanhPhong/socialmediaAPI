namespace socialmediaAPI.RequestsResponses.Requests
{
    public class UpdateFilesRequest
    {
        public List<string>? prevUrls{ get; set; }

        public List<IFormFile>? files { get; set; }
    }
}
