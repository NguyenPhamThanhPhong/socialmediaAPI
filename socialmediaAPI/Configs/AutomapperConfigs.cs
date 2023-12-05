using AutoMapper;
using socialmediaAPI.Models.DTO;
using socialmediaAPI.Models.Entities;

namespace socialmediaAPI.Configs
{
    public class AutomapperConfigs : Profile
    {
        public AutomapperConfigs()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
