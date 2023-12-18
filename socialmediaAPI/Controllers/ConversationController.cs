using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.Repositories.Repos;
using socialmediaAPI.RequestsResponses.Requests;
using socialmediaAPI.Services.CloudinaryService;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly IMapper _mapper;
        private readonly string _conversationFolderName = "Conversation";

        public ConversationController(IConversationRepository conversationRepository, CloudinaryHandler cloudinaryHandler, IMapper mapper)
        {
            _conversationRepository = conversationRepository;
            _cloudinaryHandler = cloudinaryHandler;
            _mapper = mapper;
        }
        [HttpPost("/create-converation")]
        public async Task<IActionResult> Create([FromForm] ConversationCreateRequest request)
        {
            if(!ModelState.IsValid) 
                return BadRequest(ModelState);
            var conversation = _mapper.Map<Conversation>(request);
            if (request.File != null)
                conversation.AvatarUrl = await _cloudinaryHandler.UploadSingleImage(request.File, _conversationFolderName);
            await _conversationRepository.Create(conversation);
            return Ok(conversation);
        }

        [HttpPost("/[controller]/update-fields/{id}")]
        public async Task<IActionResult> UpdateParameters(string id, [FromBody] List<UpdateParameter> parameters)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            await _conversationRepository.UpdateStringFields(id, parameters);
            return Ok("updated");
        }
        [HttpPost("/[controller]/view")]
        public async Task<IActionResult> Get([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var posts = await _conversationRepository.GetbyIds(ids);
            return Ok(posts);
        }


    }
}
