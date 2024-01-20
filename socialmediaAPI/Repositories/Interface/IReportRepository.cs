using socialmediaAPI.Models.Entities;

namespace socialmediaAPI.Repositories.Interface
{
    public interface IReportRepository
    {
        public Task Create(Report report);
        public Task<IEnumerable<Report>> GetAll();
        public Task<IEnumerable<Report>> GetFromIds(IEnumerable<string> ids);
        public Task<Report?> GetbyId(string id);
        public Task UpdatebyInstance(Report report);
        public Task Delete(string id);
    }
}
