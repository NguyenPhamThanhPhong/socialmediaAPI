using MongoDB.Driver;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Repos
{
    public class CommentRepository : ICommentRepository
    {
        private readonly IMongoCollection<Comment> _commentCollection;
        private readonly IMongoCollection<Post> _postCollection;

        public CommentRepository(DatabaseConfigs databaseConfigs)
        {
            _commentCollection = databaseConfigs.CommentCollection;
            _postCollection = databaseConfigs.PostCollection;
        }

        public async Task Create(Comment comment)
        {
            await _commentCollection.InsertOneAsync(comment);
            var postUpdate = Builders<Post>.Update.Push(s => s.CommentIds,comment.Id);
            await _postCollection.UpdateOneAsync(s=>s.Id==comment.PostId,postUpdate);
        }

        public async Task<bool> Delete(string id)
        {
            var deleteResult = await _commentCollection.DeleteOneAsync(s => s.Id == id);
            return deleteResult.DeletedCount > 0;
        }

        public Task<IEnumerable<Comment>> GetbyFilterString(string filterString)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Comment>> GetbyIds(IEnumerable<string> ids)
        {
            throw new NotImplementedException();
        }

        public Task UpdatebyParameters(string id, IEnumerable<UpdateParameter> parameters)
        {
            throw new NotImplementedException();
        }

        public Task UpdateContent(string id, string content)
        {
            throw new NotImplementedException();
        }
    }
}
