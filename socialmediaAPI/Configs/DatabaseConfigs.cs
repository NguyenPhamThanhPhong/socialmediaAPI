using MongoDB.Driver;
using socialmediaAPI.Models.Entities;

namespace socialmediaAPI.Configs
{
    public class DatabaseConfigs
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }
        public string MessageLogCollectionName { get; set; }
        public string ConversationCollectionName { get; set; }
        public string PostCollectionName { get; set; }
        public string CommentLogCollectionName { get; set; }

        public IMongoClient? MongoClient { get; private set; }
        public IMongoDatabase? MongoDtb { get; private set; }

        public IMongoCollection<User> UserCollection { get; set; }
        public IMongoCollection<MessageLog> MessageLogCollection { get; set; }
        public IMongoCollection<Conversation> ConversationCollection { get; set; }
        public IMongoCollection<Post> PostCollection { get; set; }
        public IMongoCollection<CommentLog> CommentLogCollection { get; set; }

        public void SetupDatabase()
        {
            InstantiateCollections();
        }

        private void InstantiateCollections()
        {
            MongoClient = new MongoClient(ConnectionString);
            MongoDtb = MongoClient.GetDatabase(DatabaseName);
            UserCollection = MongoDtb.GetCollection<User>(UserCollectionName);
            MessageLogCollection = MongoDtb.GetCollection<MessageLog>(MessageLogCollectionName);
            ConversationCollection = MongoDtb.GetCollection<Conversation>(ConversationCollectionName);
            PostCollection = MongoDtb.GetCollection<Post>(PostCollectionName);
            CommentLogCollection = MongoDtb.GetCollection<CommentLog>(CommentLogCollectionName);
        }
    }
}
