using AutoMapper;
using MailKit.Search;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.Repositories.Repos;
using socialmediaAPI.RequestsResponses.Requests;
using socialmediaAPI.Services.CloudinaryService;
using System.Text.RegularExpressions;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly IMapper _mapper;
        private readonly string _conversationFolderName = "Conversation";

        public ConversationController(IConversationRepository conversationRepository, CloudinaryHandler cloudinaryHandler, IMapper mapper, IMessageRepository messageRepository)
        {
            _conversationRepository = conversationRepository;
            _cloudinaryHandler = cloudinaryHandler;
            _mapper = mapper;
            _messageRepository = messageRepository;
        }
        [HttpPost("/conversation-create")]
        public async Task<IActionResult> Create([FromForm] ConversationCreateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var conversation = _mapper.Map<Conversation>(request);
            if (request.File != null)
                conversation.AvatarUrl = await _cloudinaryHandler.UploadSingleImage(request.File, _conversationFolderName);
            await _conversationRepository.Create(conversation);
            return Ok(conversation);
        }

        [HttpPost("/conversation-update-string-fields/{id}")]
        public async Task<IActionResult> UpdateParameters([FromBody] List<UpdateParameter> parameters, string id)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            await _conversationRepository.UpdateStringFields(id, parameters);
            return Ok("updated");
        }
        [HttpPost("/conversation-get-many/{skip}")]
        public async Task<IActionResult> GetMany([FromBody] List<string> ids, int skip)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var conversations = await _conversationRepository.GetbyIds(ids, skip);
            var messages = await _messageRepository.GetbyIds(ids, skip);
            return Ok(new { conversations, messages });
        }

        [HttpPost("/conversation-get-by-name")]
        public async Task<IActionResult> GetbyName(string name)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var sanitizedPattern = new string(name
                    .Where(c => Char.IsLetterOrDigit(c)) // Keep only alphanumeric characters
                    .ToArray());

            var filter = Builders<Conversation>.Filter.Regex(c => c.Name, new BsonRegularExpression($"/{Regex.Escape(sanitizedPattern)}/i"));
            var conversations = await _conversationRepository.GetbyFilter(filter);
            return Ok(conversations);

        }


    }
}
