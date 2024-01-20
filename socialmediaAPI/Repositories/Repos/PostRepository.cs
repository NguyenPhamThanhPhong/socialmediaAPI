using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Repos
{
    public class PostRepository : IPostRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        private readonly IMongoCollection<Post> _postCollection;
        private readonly IMongoCollection<Comment> _commentCollection;
        public PostRepository(DatabaseConfigs configuration)
        {
            _userCollection = configuration.UserCollection;
            _postCollection = configuration.PostCollection;
            _commentCollection = configuration.CommentCollection;
        }
        public async Task CreatePost(Post post)
        {
            await _postCollection.InsertOneAsync(post);
            var filterUser = Builders<User>.Filter.Eq(u => u.ID, post.Owner.UserId);
            var updateUser = Builders<User>.Update.Push(u => u.PostIds, post.Id);
            await _userCollection.UpdateOneAsync(filterUser, updateUser);
        }

        public async Task<Post> Delete(string id)
        {
            var deletedPost = await _postCollection.FindOneAndDeleteAsync(p => p.Id == id);

            var filterUser = Builders<User>.Filter.Eq(u => u.ID, deletedPost.Owner.UserId);
            var updateUser = Builders<User>.Update.Pull(u => u.PostIds, deletedPost.Id);
            await _userCollection.UpdateOneAsync(filterUser, updateUser);

            var filterCommentLog = Builders<Comment>.Filter.In(c => c.Id, deletedPost.CommentIds);
            await _commentCollection.DeleteManyAsync(filterCommentLog);
            return deletedPost;
        }

        public async Task<IEnumerable<Post>> GetbyIds(IEnumerable<string> ids)
        {
            var filter = Builders<Post>.Filter.In(p => p.Id, ids);
            return await _postCollection.Find(filter).ToListAsync();
        }

        public Task UpdatebyInstance(Post post)
        {
            var filter = Builders<Post>.Filter.Eq(p => p.Id, post.Id);
            return _postCollection.FindOneAndReplaceAsync(filter, post);
        }

    }
}
