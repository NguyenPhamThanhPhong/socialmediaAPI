using socialmediaAPI.Models.Entities;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Interface
{
    public interface IPostRepository
    {
        public Task CreatePost(Post post);

        public Task<Post> GetbyId(string id);
        public Task<List<Post>> GetbyFilterString(string filterString);

        public Task UpdatebyInstance(Post post);
        public Task UpdatebyParameters(string id, List<UpdateParameter> parameters);

        public Task Delete(string id);
    }
}
