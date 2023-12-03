using socialmediaAPI.Models.Embeded.CommentLog;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Interface
{
    public interface ICommentLogRepository
    {
        public Task Create(CommentLog commentLog);
        public Task<CommentLog> GetById(string id);
        public Task UpdatebyParameters(string id, List<UpdateParameter> parameters);



    }
}
