using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using socialmediaAPI.Models.DTO;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;
using socialmediaAPI.Services.CloudinaryService;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _userFolderName;


        public UserController(IMapper mapper, IUserRepository userRepository, CloudinaryHandler cloudinaryHandler, string userFolderName)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _cloudinaryHandler = cloudinaryHandler;
            _userFolderName = userFolderName;
        }

        [HttpGet("/viewDTO/{id}")]
        public async Task<IActionResult> GetUserDTO(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid id");
            var user = await _userRepository.GetbyId(id);
            var userDTO = _mapper.Map<UserDTO>(user);
            return Ok(userDTO);
        }
        [HttpPut("/update-by-parameters/{id}")]
        public async Task<IActionResult> UpdatebyParameter(string id, [FromBody] List<UpdateParameter> parameters)
        {
            if (!ModelState.IsValid)
                return BadRequest($"invalid model state {parameters}");
            await _userRepository.UpdatebyParameters(id, parameters);
            return Ok("updated");
        }
        [HttpPut("/update-instance")]
        public async Task<IActionResult> UpdatebyParameter([FromBody] User user)
        {
            if (!ModelState.IsValid)
                return BadRequest($"invalid model state ");
            await _userRepository.UpdatebyInstance(user);
            return Ok("updated");
        }
        [HttpPut("/update-avatar/{id}")]
        public async Task<IActionResult> UpdateAvatar(string id, [FromForm] (string prevUrl, IFormFile file) formData)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var avatarParameter = new UpdateParameter()
            {
                FieldName = Models.Entities.User.GetFieldName(u => u.PersonalInfo.AvatarUrl),
                updateAction = UpdateAction.set
            };
            if (formData.file == null)
            {
                if(!string.IsNullOrEmpty(formData.prevUrl))
                    await _cloudinaryHandler.Delete(formData.prevUrl);
                avatarParameter.Value = null;
            }
            else
                avatarParameter.Value = await _cloudinaryHandler.UploadSingleImage(formData.file, _userFolderName);

            await _userRepository.UpdatebyParameters(id,new List<UpdateParameter> { avatarParameter });
            return Ok("updated");
        }

        [HttpDelete("/delete-user/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest($"invalid model state ");
            var deletedUser = await _userRepository.Delete(id);
            await _cloudinaryHandler.Delete(deletedUser.PersonalInfo.AvatarUrl);
            return Ok("deleted");
        }

        #region private Util function update user Database & Cloudinary
        private async Task UpdateUserAvatar(string id, List<IFormFile> files)
        {
            var avatarSet = await _cloudinaryHandler.UploadImages(files, _userFolderName);
            UpdateParameter parameter = new UpdateParameter()
            {
                FieldName = Models.Entities.User.GetFieldName(u => u.PersonalInfo.AvatarUrl),
                Value = avatarSet.Values.FirstOrDefault(),
                updateAction = UpdateAction.set
            };
            await _userRepository.UpdatebyParameters(id, new List<UpdateParameter> { parameter });
        }
        #endregion
    }
}
