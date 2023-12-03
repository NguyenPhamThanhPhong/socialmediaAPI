using socialmediaAPI.Models.Enums;

namespace socialmediaAPI.Models.Embeded.Post
{
#pragma warning disable CS8618

    public class LikeRepresentation
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public Emoji Likes { get; set; }
    }
}
