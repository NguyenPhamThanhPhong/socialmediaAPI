using Microsoft.Extensions.Hosting;
using MongoDB.Driver;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;

namespace socialmediaAPI.Repositories.Repos
{
    public class ReportRepository : IReportRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Report> _reportCollection;
        public ReportRepository(DatabaseConfigs configuration)
        {
            _userCollection = configuration.UserCollection;
            _reportCollection = configuration.ReportCollection;
        }

        public async Task Create(Report report)
        {
            await _reportCollection.InsertOneAsync(report);
            var filterUser = Builders<User>.Filter.Eq(u => u.ID, report.ReporterId);
            var updateUser = Builders<User>.Update.Push(u => u.PostIds, report.ID);
            await _userCollection.UpdateOneAsync(filterUser, updateUser);
        }

        public async Task Delete(string id)
        {
            var deletedReport = await _reportCollection.FindOneAndDeleteAsync(s => s.ID == id);
            var filterUser = Builders<User>.Filter.Eq(u => u.ID, deletedReport.ReporterId);
            var updateUser = Builders<User>.Update.Pull(u => u.PostIds, deletedReport.ID);
            await _userCollection.UpdateOneAsync(filterUser, updateUser);
        }

        public async Task<IEnumerable<Report>> GetAll()
        {
            return await _reportCollection.Find(_ => true).ToListAsync();
        }

        public async Task<Report?> GetbyId(string id)
        {
            return await _reportCollection.Find(s => s.ID == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Report>> GetFromIds(IEnumerable<string> ids)
        {
            var filter = Builders<Report>.Filter.In(s=>s.ID,ids);
            return await _reportCollection.Find(filter).ToListAsync();
        }

        public Task UpdatebyInstance(Report report)
        {
            return _reportCollection.ReplaceOneAsync(s=>s.ID==report.ID,report);
        }
    }
}
