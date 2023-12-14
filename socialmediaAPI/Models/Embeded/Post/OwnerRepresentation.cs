using System.ComponentModel.DataAnnotations;

namespace socialmediaAPI.Models.Embeded.Post
{
#pragma warning disable CS8618

    public class OwnerRepresentation
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? AvatarUrl { get; set; }
    }
}
