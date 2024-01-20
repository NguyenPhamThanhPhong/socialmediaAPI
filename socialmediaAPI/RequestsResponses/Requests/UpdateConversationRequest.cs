namespace socialmediaAPI.RequestsResponses.Requests
{
    public class UpdateConversationRequest
    {
        public string Name { get; set; }
        public IFormFile? File { get; set; }
        public List<string> ParticipantIds { get; set; }
    }
}
