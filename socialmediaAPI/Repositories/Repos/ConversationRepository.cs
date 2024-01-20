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
            var filter = Builders<User>.Filter.In(u => u.ID, conversation.ParticipantIds);
            var update = Builders<User>.Update.Push(u => u.ConversationIds, conversation.ID);
            await _userCollection.UpdateManyAsync(filter, update);
        }

        public async Task<Conversation> Delete(string id)
        {
            var conversation = await _conversationCollection.FindOneAndDeleteAsync(u => u.ID == id);
            var messageFilter = Builders<Message>.Filter.In(s=>s.Id,conversation.ParticipantIds);
            await _messageCollection.DeleteManyAsync(messageFilter);
            return conversation;
        }

        public async Task<IEnumerable<Conversation>> GetbyFilter(FilterDefinition<Conversation> filter)
        {
            return await _conversationCollection.Find(filter).ToListAsync();
        }

        public async Task<IEnumerable<Conversation>> GetbyIds(IEnumerable<string> ids,int skip)
        {
            var filter = Builders<Conversation>.Filter.In(c => c.ID, ids);
            var sort = Builders<Conversation>.Sort.Descending(c => c.RecentMessage.RecentTime);
            return await _conversationCollection.Find(filter).Sort(sort).Skip(skip).Limit(50).ToListAsync();
        }
    }
}
