using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.Serialization.Attributes;

namespace socialmediaAPI.Models.Embeded.User
{
    [BsonIgnoreExtraElements]
    public class AuthenticationInformation
    {
#pragma warning disable CS8618
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
