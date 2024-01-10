using socialmediaAPI.Models.Entities;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Interface
{
    public interface ICommentRepository
    {
        public Task Create(Comment comment);
        public Task<IEnumerable<Comment>> GetfromIds(IEnumerable<string>ids, int skip);

        public Task UpdatebyParameters(string id, List<UpdateParameter> parameters);
        public Task UpdateContent(string id, string content);

        public Task<Comment> Delete(string id);

    }
}
