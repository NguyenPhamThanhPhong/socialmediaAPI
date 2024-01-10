namespace socialmediaAPI.RequestsResponses.Requests
{
    public class UpdateCommentRequest
    {
        public string Id { get; set; }
        public List<IFormFile>? Files { get; set; }
        public string Content { get; set; }

        public UpdateCommentRequest() {
            Id = string.Empty; Content= string.Empty;
        }

    }
}
