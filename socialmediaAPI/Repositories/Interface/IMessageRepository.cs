using socialmediaAPI.Models.Entities;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Interface
{
    public interface IMessageRepository
    {
        public Task Create(Message message);
        public Task<IEnumerable<Message>> GetbyFilterString(string filterString);
        public Task<IEnumerable<Message>> GetbyIds(IEnumerable<string> ids);

        public Task UpdatebyParameters(string id, IEnumerable<UpdateParameter> parameters);
        public Task UpdateContent(string id, string content);

        public Task<Message> Delete(string id);
    }
}
