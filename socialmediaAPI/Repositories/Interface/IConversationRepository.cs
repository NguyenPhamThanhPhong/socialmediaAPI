using MongoDB.Driver;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Interface
{
    public interface IConversationRepository
    {
        public Task Create(Conversation conversation);
        public Task<IEnumerable<Conversation>> GetbyFilter(FilterDefinition<Conversation> filter);
        public Task<IEnumerable<Conversation>> GetbyIds(IEnumerable<string> ids,int skip);

        public Task<Conversation> Delete(string id);
    }
}
