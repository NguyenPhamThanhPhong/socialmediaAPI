using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;
using Org.BouncyCastle.Crypto;
using socialmediaAPI.Configs;

namespace socialmediaAPI.Repositories.Repos
{
    public class ConversationRepository : IConversationRepository
    {
        private IMongoCollection<Conversation> _conversationCollection;
        private IMongoCollection<User> _userCollection;
        private IMongoCollection<Message> _messageCollection;

        public ConversationRepository(DatabaseConfigs databaseConfigs)
        {
            _conversationCollection = databaseConfigs.ConversationCollection;
            _userCollection = databaseConfigs.UserCollection;
            _messageCollection = databaseConfigs.MessageCollection;
        }

        public async Task Create(Conversation conversation)
        {
            await _conversationCollection.InsertOneAsync(conversation);
            var filter = Builders<User>.Filter.Eq(u => u.ConverationIds, conversation.ParticipantIds);
            var update = Builders<User>.Update.Push(u => u.ConverationIds, conversation.ID);
            await _userCollection.UpdateManyAsync(filter, update);
        }

        public Task<Conversation> Delete(string id)
        {
            return _conversationCollection.FindOneAndDeleteAsync(u=>u.ID == id);
        }

        public async Task<IEnumerable<Conversation>> GetbyFilterString(string filterString)
        {
            var filterDocument = BsonSerializer.Deserialize<BsonDocument>(filterString);
            var filter = new BsonDocumentFilterDefinition<Post>(filterDocument);
            return await _conversationCollection.Find(filterString).ToListAsync();
        }

        public async Task<IEnumerable<Conversation>> GetbyIds(IEnumerable<string> ids)
        {
            var filter = Builders<Conversation>.Filter.In(c => c.ID, ids);
            return await _conversationCollection.Find(filter).ToListAsync();
        }

        public Task UpdatebyInstance(Conversation conversation)
        {
            return _conversationCollection.ReplaceOneAsync(c=>c.ID==conversation.ID, conversation);  
        }

        public Task UpdatebyParameters(string id, IEnumerable<UpdateParameter> parameters)
        {
            var filter = Builders<Conversation>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<Conversation>.Update;
            List<UpdateDefinition<Conversation>> subUpdates = new List<UpdateDefinition<Conversation>>();

            foreach (var parameter in parameters)
            {
                switch (parameter.updateAction)
                {
                    case UpdateAction.set:
                        subUpdates.Add(Builders<Conversation>.Update.Set(parameter.FieldName, parameter.Value));
                        continue;
                    case UpdateAction.push:
                        subUpdates.Add(Builders<Conversation>.Update.Push(parameter.FieldName, parameter.Value));
                        continue;
                    case UpdateAction.pull:
                        subUpdates.Add(Builders<Conversation>.Update.Pull(parameter.FieldName, parameter.Value));
                        continue;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);
            return _conversationCollection.UpdateOneAsync(filter, combinedUpdate);
        }

        public Task UpdateStringFields(string id, IEnumerable<UpdateParameter> parameters)
        {
            var filter = Builders<Conversation>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<Conversation>.Update;
            List<UpdateDefinition<Conversation>> subUpdates = new List<UpdateDefinition<Conversation>>();

            foreach (var parameter in parameters)
            {
                switch (parameter.updateAction)
                {
                    case UpdateAction.set:
                        subUpdates.Add(Builders<Conversation>.Update.Set(parameter.FieldName, parameter.Value?.ToString()));
                        continue;
                    case UpdateAction.push:
                        subUpdates.Add(Builders<Conversation>.Update.Push(parameter.FieldName, parameter.Value?.ToString()));
                        continue;
                    case UpdateAction.pull:
                        subUpdates.Add(Builders<Conversation>.Update.Pull(parameter.FieldName, parameter.Value?.ToString()));
                        continue;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);
            return _conversationCollection.UpdateOneAsync(filter, combinedUpdate);
        }
    }
}
