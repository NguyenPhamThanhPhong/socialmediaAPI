using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Text.Json;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;

namespace socialmediaAPI.Repositories.Repos
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userCollection;
        public UserRepository(DatabaseConfigs configuration)
        {
            _userCollection = configuration.UserCollection;
        }
        public Task Create(User user)
        {
            return _userCollection.InsertOneAsync(user);
        }
        public Task<User> Delete(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.ID, id);
            return _userCollection.FindOneAndDeleteAsync(filter);
        }

        public Task<List<User>> GetbyFilterString(string filterString)
        {
            var filterDocument = BsonSerializer.Deserialize<BsonDocument>(filterString);
            var filter = new BsonDocumentFilterDefinition<User>(filterDocument);
            return _userCollection.Find(filter).ToListAsync();
        }

        public Task<User> GetbyId(string id)
        {
            var filter = Builders<User>.Filter.Eq(u => u.ID, id);
            return _userCollection.Find(filter).FirstOrDefaultAsync();
        }

        public Task<User> GetbyUsername(string username)
        {
            var filter = Builders<User>.Filter.Eq(u => u.AuthenticationInfo.Username, username);
            return _userCollection.Find(filter).FirstOrDefaultAsync();
        }

        public Task UpdatebyInstance(User user)
        {
            var filter = Builders<User>.Filter.Eq(u => u.ID, user.ID);
            return _userCollection.ReplaceOneAsync(filter, user);
        }

        public Task UpdatebyParameters(string id, List<UpdateParameter> parameters)
        {
            var filter = Builders<User>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<User>.Update;
            List<UpdateDefinition<User>> subUpdates = new List<UpdateDefinition<User>>();
            foreach (var parameter in parameters)
            {
                object? myValue;
                if (parameter.Value is string)
                    myValue = JsonSerializer.Serialize(parameter.Value);
                myValue = parameter.Value;
                Console.WriteLine(JsonSerializer.Serialize(myValue));
                switch (parameter.updateAction)
                {
                    case UpdateAction.set:
                        subUpdates.Add(Builders<User>.Update.Set(parameter.FieldName, myValue));
                        continue;
                    case UpdateAction.push:
                        subUpdates.Add(Builders<User>.Update.Push(parameter.FieldName, myValue));
                        continue;
                    case UpdateAction.pull:
                        subUpdates.Add(Builders<User>.Update.Pull(parameter.FieldName, myValue));
                        continue;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);
            return _userCollection.UpdateOneAsync(filter, combinedUpdate);
        }
        public Task UpdateStringFields(string id, List<UpdateParameter> parameters)
        {
            var filter = Builders<User>.Filter.Eq(p => p.ID, id);
            var updateBuilder = Builders<User>.Update;
            List<UpdateDefinition<User>> subUpdates = new List<UpdateDefinition<User>>();
            foreach (var parameter in parameters)
            {
                string? myValue = parameter.Value.ToString();
                switch (parameter.updateAction)
                {
                    case UpdateAction.set:
                        subUpdates.Add(Builders<User>.Update.Set(parameter.FieldName, myValue ?? null));
                        continue;
                    case UpdateAction.push:
                        subUpdates.Add(Builders<User>.Update.Push(parameter.FieldName, myValue ?? null));
                        continue;
                    case UpdateAction.pull:
                        subUpdates.Add(Builders<User>.Update.Pull(parameter.FieldName, myValue ?? null));
                        continue;
                }
            }
            var combinedUpdate = updateBuilder.Combine(subUpdates);
            return _userCollection.UpdateOneAsync(filter, combinedUpdate);
        }
    }
}
