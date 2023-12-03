using socialmediaAPI.Models.Entities;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Interface
{
    public interface IUserRepository
    {
        public Task Create(User user);

        public Task<User> GetbyId(string id);
        public Task<User> GetbyUsername(string username);
        public Task<List<User>> GetbyFilterString(string filterString);

        public Task Delete(string id);

        public Task UpdatebyInstance(User user);
        public Task UpdatebyParameters(string id, List<UpdateParameter> parameters);
    }
}
