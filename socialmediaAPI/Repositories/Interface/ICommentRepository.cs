using socialmediaAPI.Models.Entities;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Interface
{
    public interface ICommentRepository
    {
        public Task Create(Comment comment);
        public Task<IEnumerable<Comment>> GetbyFilterString(string filterString);
        public Task<IEnumerable<Comment>> GetbyIds(IEnumerable<string> ids);

        public Task UpdatebyParameters(string id, IEnumerable<UpdateParameter> parameters);
        public Task UpdateContent(string id, string content);

        public Task<bool> Delete(string id);
    }
}
