using MongoDB.Driver;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Embeded.CommentLog;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Repos
{
    public class CommentLogRepository : ICommentLogRepository
    {
        private readonly IMongoCollection<Post> _postCollection;
        private readonly IMongoCollection<CommentLog> _commentLogCollection;
        public CommentLogRepository(DatabaseConfigs configuration)
        {
            _postCollection = configuration.PostCollection;
            _commentLogCollection = configuration.CommentLogCollection;
        }
        public async Task Create(CommentLog commentLog)
        {
            await _commentLogCollection.InsertOneAsync(commentLog);
            var filterPost = Builders<Post>.Filter.Eq(c => c.Id, commentLog.PostId);
            var updatePost =
                Builders<Post>.Update
                .Push(u => u.CommentLogIds, commentLog.Id)
                .Inc( p => p.CommentCounter, 1);

            await _postCollection.UpdateOneAsync(filterPost, updatePost);
            return;
        }


        public Task<CommentLog> GetById(string id)
        {
            var filter = Builders<CommentLog>.Filter.Eq(p => p.Id, id);
            return _commentLogCollection.Find(filter).FirstOrDefaultAsync();
        }


        public async Task UpdatebyParameters(string id, List<UpdateParameter> parameters)
        {
            var filter = Builders<CommentLog>.Filter.Eq(p => p.Id, id);
            var updateBuilder = Builders<CommentLog>.Update;
            List<UpdateDefinition<CommentLog>> subUpdates = new List<UpdateDefinition<CommentLog>>();
            foreach (var parameter in parameters)
            {
                subUpdates.Add(GetUpdatefromParameter(parameter));
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);
            await _commentLogCollection.UpdateOneAsync(filter, combinedUpdate);
        }

        private UpdateDefinition<CommentLog> GetUpdatefromParameter(UpdateParameter parameter)
        {
            switch (parameter.updateAction)
            {
                case UpdateAction.set:
                    return Builders<CommentLog>.Update.Set(parameter.FieldName, parameter.Value);
                case UpdateAction.push:
                    return Builders<CommentLog>.Update.Push(parameter.FieldName, parameter.Value);
                case UpdateAction.pull:
                    return Builders<CommentLog>.Update.Pull(parameter.FieldName, parameter.Value);
            }
            return Builders<CommentLog>.Update.Set(parameter.FieldName, parameter.Value);

        }
    }
}
