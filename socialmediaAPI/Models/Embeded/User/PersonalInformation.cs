﻿using MongoDB.Bson.Serialization.Attributes;

namespace socialmediaAPI.Models.Embeded.User
{
    [BsonIgnoreExtraElements]
    public class PersonalInformation
    {
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? Favorites { get; set; }
        public string? Biography { get; set; }

    }
}
