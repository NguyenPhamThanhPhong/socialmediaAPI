using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.DTO;
using socialmediaAPI.Models.Embeded.User;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;
using socialmediaAPI.Services.CloudinaryService;
using System.Security.Claims;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _userFolderName;
        private readonly IMongoCollection<User> _userCollection;


        public UserController(IMapper mapper, IUserRepository userRepository, CloudinaryHandler cloudinaryHandler,
            CloudinaryConfigs cloudinaryConfigs, DatabaseConfigs databaseConfigs, IConversationRepository conversationRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _cloudinaryHandler = cloudinaryHandler;
            _userFolderName = cloudinaryConfigs.UserFolderName;
            _userCollection = databaseConfigs.UserCollection;
            _conversationRepository = conversationRepository;
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

        [HttpPost("/get-from-ids")]
        public async Task<IActionResult> GetFromIds([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var filter = Builders<User>.Filter.In(s => s.ID, ids);
            var users = await _userCollection.Find(filter).ToListAsync();
            return Ok(users);
        }
        #region email-password

        [HttpPut("/update-email/{id}")]
        public async Task<IActionResult> UpdateEmail(string id, [FromBody] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest($"invalid model state");
            var parameter = new UpdateParameter(Models.Entities.User.GetFieldName(u => u.AuthenticationInfo.Email), email, UpdateAction.set);
            await _userRepository.UpdatebyParameters(id, new List<UpdateParameter> { parameter });
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

        [HttpPost("/update-personal-info/{id}")]
        public async Task<IActionResult> UpdatePersonalInfo(string id, [FromForm] UpdateUserPersonalInformationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var personalInfo = request.ConvertToPersonalInformation();
            if (!string.IsNullOrEmpty(request.prevAvatar))
                await _cloudinaryHandler.Delete(request.prevAvatar);

            if (request.AvatarFile != null)
                personalInfo.AvatarUrl = await _cloudinaryHandler.UploadSingleImage(request.AvatarFile, _userFolderName);

            var filter = Builders<User>.Filter.Eq(s => s.ID, id);
            var update = Builders<User>.Update.Set(s => s.PersonalInfo, personalInfo);
            var result = await _userCollection.UpdateOneAsync(filter, update);
            if (result.ModifiedCount > 0)
                return Ok("updated");
            return BadRequest("failed to update");

        }

        [Authorize]
        [HttpPost("/update-follow/{targetId}/{updateAction}")]
        public async Task<IActionResult> UpdateFollow(string targetId, bool updateAction)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var selfId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (selfId == null)
                return Unauthorized();

            var selfFilter = Builders<User>.Filter.Eq(s => s.ID, selfId);
            var targetFilter = Builders<User>.Filter.Eq(s => s.ID, targetId);
            if (updateAction == true)
            {
                var updateSelf = Builders<User>.Update.AddToSet(s => s.FollowingIds, targetId);
                var updateTarget = Builders<User>.Update.AddToSet(s => s.FollowerIds, selfId);
                await Task.WhenAll(_userCollection.UpdateOneAsync(selfFilter, updateSelf),
                    _userCollection.UpdateOneAsync(targetFilter, updateTarget));
            }
            else
            {
                var updateSelf = Builders<User>.Update.Pull(s => s.FollowingIds, targetId);
                var updateTarget = Builders<User>.Update.Pull(s => s.FollowerIds, selfId);
                await Task.WhenAll(_userCollection.UpdateOneAsync(selfFilter, updateSelf), 
                    _userCollection.UpdateOneAsync(targetFilter, updateTarget));
            }
            return Ok();
        }
        [HttpDelete("/user-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest($"invalid model state ");
            var deletedUser = await _userRepository.Delete(id);
            await _cloudinaryHandler.Delete(deletedUser.PersonalInfo.AvatarUrl);
            return Ok("deleted");
        }
    }
}
