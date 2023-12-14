using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.DTO;
using socialmediaAPI.Models.Embeded.User;
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


        public UserController(IMapper mapper, IUserRepository userRepository, CloudinaryHandler cloudinaryHandler, 
            CloudinaryConfigs cloudinaryConfigs)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _cloudinaryHandler = cloudinaryHandler;
            _userFolderName = cloudinaryConfigs.UserFolderName;
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
        #region email-password

        [HttpPut("/update-email/{id}")]
        public async Task<IActionResult> UpdateEmail(string id, [FromBody] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest($"invalid model state");
            var parameter = new UpdateParameter(Models.Entities.User.GetFieldName(u => u.AuthenticationInfo.Email), email, UpdateAction.set);
            await _userRepository.UpdatebyParameters(id, new List<UpdateParameter> { parameter});
            return Ok("updated");
        }
        [HttpPut("/update-password/{id}")]
        public async Task<IActionResult> UpdatePassword(string id, [FromBody] string password)
        {
            if (!ModelState.IsValid)
                return BadRequest($"invalid model state");
            var parameter = new UpdateParameter(Models.Entities.User.GetFieldName(u => u.AuthenticationInfo.Password), password, UpdateAction.set);
            await _userRepository.UpdatebyParameters(id, new List<UpdateParameter> { parameter });
            return Ok("updated");
        }
        #endregion

        [HttpPut("/update-avatar/{id}/{prevUrl}")]
        public async Task<IActionResult> UpdateAvatar(string id, string prevUrl , [FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var avatarParameter = new UpdateParameter()
            {
                FieldName = Models.Entities.User.GetFieldName(u => u.PersonalInfo.AvatarUrl),
                updateAction = UpdateAction.set
            };
            if (file == null)
            {
                if(!string.IsNullOrEmpty(prevUrl))
                    await _cloudinaryHandler.Delete(prevUrl);
                avatarParameter.Value = null;
            }
            else
                avatarParameter.Value = await _cloudinaryHandler.UploadSingleImage(file, _userFolderName);

            await _userRepository.UpdatebyParameters(id,new List<UpdateParameter> { avatarParameter });
            return Ok("updated");
        }
        [HttpPut("/update-personal-info/{id}")]
        public async Task<IActionResult> UpdatePersonalInfo(string id, [FromForm] UpdateUserPersonalInformationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var personalInfo = request.ConvertToPersonalInformation();
            if (!string.IsNullOrEmpty(request.prevAvatar))
                await _cloudinaryHandler.Delete(request.prevAvatar);
            if (request.AvatarFile != null)
            {
                var dict = await _cloudinaryHandler.UploadImages(new List<IFormFile> { request.AvatarFile }, _userFolderName); ;
                personalInfo.AvatarUrl = dict.Values.FirstOrDefault();
            }
            var parameter = new UpdateParameter(Models.Entities.User.GetFieldName(u => u.PersonalInfo),personalInfo,UpdateAction.set);
            await _userRepository.UpdatebyParameters(id,new List<UpdateParameter> { parameter});
            return Ok(parameter);

        }
        [HttpPut("/update-parameters-string-fields/{id}")]
        public async Task<IActionResult> UpdateParmeters(string id, [FromBody] List<UpdateParameter> parameters)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            await _userRepository.UpdateStringFields(id,parameters);
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
