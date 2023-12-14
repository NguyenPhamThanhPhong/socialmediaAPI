using socialmediaAPI.Models.Embeded.User;
using socialmediaAPI.Models.Entities;

namespace socialmediaAPI.RequestsResponses.Requests
{
    public class UpdateUserPersonalInformationRequest
    {
        public string? Name { get; set; }
        public string? prevAvatar { get; set; }
        public IFormFile? AvatarFile { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string? Favorites { get; set; }
        public string? Biography { get; set; }

        public PersonalInformation ConvertToPersonalInformation()
        {
            return new PersonalInformation()
            {
                Name = Name,
                AvatarUrl = "",
                DateofBirth = DateofBirth,
                Favorites= Favorites,
                Biography= Biography
            };
        }
    }
}
