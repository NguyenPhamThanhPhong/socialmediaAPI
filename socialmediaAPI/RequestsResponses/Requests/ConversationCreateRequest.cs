namespace socialmediaAPI.RequestsResponses.Requests
{
    public class ConversationCreateRequest
    {
        public string? Name { get; set; }
        public List<string>? ParticipantIds { get; set; }
        public IFormFile? File { get; set; }
    }
}
