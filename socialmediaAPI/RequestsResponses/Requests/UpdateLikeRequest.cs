using socialmediaAPI.Models.Embeded.Post;
using socialmediaAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace socialmediaAPI.RequestsResponses.Requests
{
    public class UpdateLikeRequest
    {
        [Required]
        public string UserId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public Emoji Emo { get; set; }
        public UpdateAction UpdateAction { get; set; }

        public LikeRepresentation ConvertToLike()
        {
            return new LikeRepresentation
            {
                Emo = Emo,
                UserId = UserId
            };
        }

    }
}
