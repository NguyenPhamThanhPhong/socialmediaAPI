using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using socialmediaAPI.Configs;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;
using socialmediaAPI.Services.CloudinaryService;
using socialmediaAPI.Services.Validators;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly UserValidator _userValidator;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _userFolderName;

        public LoginController(IUserRepository userRepository, UserValidator userValidator, 
            CloudinaryHandler cloudinaryHandler,CloudinaryConfigs cloudinaryConfigs)
        {
            _userRepository = userRepository;
            _userValidator = userValidator;
            _cloudinaryHandler = cloudinaryHandler;
            _userFolderName = cloudinaryConfigs.UserFolderName;
        }

        [HttpPost("/register")]
        public async Task<IActionResult> Register([FromForm] UserRegisterRequest request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("invalid modelstate");
            }
            if( await _userValidator.IsValidEmail(request.Email))
            {
                return BadRequest("email doesn't exists");
            }

            var user = request.ConvertToUser();
            try
            {
                await _userRepository.Create(user);
                if(request.File!= null)
                {
                    await this.UpdateUserAvatar(user.ID, new List<IFormFile> { request.File });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(user);
        }

        #region private Util function update user Database & Cloudinary
        private async Task UpdateUserAvatar(string id, List<IFormFile> files)
        {
            var avatarSet = await _cloudinaryHandler.UploadImages(files, _userFolderName);
            UpdateParameter parameter = new UpdateParameter()
            {
                FieldName = Models.Entities.User.GetFieldName(u => u.PersonalInfo.AvatarUrl),
                Value = avatarSet,
                updateAction = UpdateAction.set
            };
            await _userRepository.UpdatebyParameters(id, new List<UpdateParameter> { parameter });
        }
        #endregion
    }
}
