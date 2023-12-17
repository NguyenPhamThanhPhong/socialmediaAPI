using socialmediaAPI.Models.Entities;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Interface
{
    public interface IConversationRepository
    {
        public Task Create(Conversation conversation);
        public Task<IEnumerable<Conversation>> GetbyFilterString(string filterString);
        public Task<IEnumerable<Conversation>> GetbyIds(IEnumerable<string> ids);

        public Task UpdatebyInstance(Conversation conversation);
        public Task UpdatebyParameters(string id, IEnumerable<UpdateParameter> parameters);
        public Task UpdateStringFields(string id, IEnumerable<UpdateParameter> parameters);

        public Task<Conversation> Delete(string id);
    }
}
