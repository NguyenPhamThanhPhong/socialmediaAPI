using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;
using socialmediaAPI.Services.CloudinaryService;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageRepository _messageRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly IMapper _mapper;
        private readonly string _messageFolderName;

        public MessageController(IMessageRepository messageRepository, 
            CloudinaryHandler cloudinaryHandler, IMapper mapper,
            CloudinaryConfigs cloudinaryConfigs)
        {
            _messageRepository = messageRepository;
            _cloudinaryHandler = cloudinaryHandler;
            _mapper = mapper;
            _messageFolderName = cloudinaryConfigs.MessageFolderName;
        }
        [Authorize]
        [HttpPost("/message-send")]
        public async Task<IActionResult> Create([FromForm] MessageCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var message = _mapper.Map<Message>(request);
            if (request.Files != null)
                message.FileUrls = await _cloudinaryHandler.UploadImages(request.Files, _messageFolderName);
             await _messageRepository.Create(message);
            return Ok(message);
        }
        [Authorize]
        [HttpDelete("/message-delete/{id}")]
        public async Task<IActionResult> Edit(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var deletedMessage  = await _messageRepository.Delete(id);
            if (deletedMessage.FileUrls != null)
                foreach (var item in deletedMessage.FileUrls)
                    await _cloudinaryHandler.Delete(item.Value);

            return Ok($"delete state is {deletedMessage!=null}");
        }
        [Authorize]
        [HttpPost("/message-get-many/{skip}")]
        public async Task<IActionResult> GetMany([FromBody] List<string> messageIds, int skip)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            var messages = await _messageRepository.GetbyIds(messageIds, skip);
            return Ok(messages);
        }
    }
}
