using AutoMapper;
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
        [HttpPost("/send-message")]
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


    }
}
