using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Embeded.Post;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;
using socialmediaAPI.Services.CloudinaryService;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _postFolderName;

        public PostController(IPostRepository postRepository, 
            CloudinaryHandler cloudinaryHandler,CloudinaryConfigs cloudinaryConfigs)
        {
            _postRepository = postRepository;
            _cloudinaryHandler = cloudinaryHandler;
            _postFolderName = cloudinaryConfigs.PostFolderName;
        }
        [HttpPost("/create")]
        public async Task<IActionResult> Create([FromForm] CreatePostRequest request )
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            Post post = request.ConvertToPost();

            if (request.Files != null)
            {
                var fileUrls = await _cloudinaryHandler.UploadImages(request.Files, _postFolderName);
                post.FileUrls = fileUrls;
            }
            await _postRepository.CreatePost(post);
            return Ok(post);
        }
        [HttpPut("/update-parameters/{id}")]
        public async Task<IActionResult> UpdateParameters(string id,[FromBody] List<UpdateParameter> updateParameters)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            await _postRepository.UpdatebyParameters(id, updateParameters);
            return Ok("updated");
        }

        [HttpPost("/like-unlike")]
        public async Task<IActionResult> UpdateLikes(string id, [FromBody] (LikeRepresentation like, UpdateAction updateAction) request)
        {
            if (!ModelState.IsValid || request.updateAction == UpdateAction.set)
                return BadRequest("invalid modelstate");
            var parameter = new UpdateParameter(Post.GetFieldName(p=>p.Likes),request.like,request.updateAction);
            return Ok("updated");
        }

        [HttpGet("/view/{id}")]
        public async Task<IActionResult> Get(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var post = await _postRepository.GetbyId(id);
            return Ok(post);
        }
        [HttpPut("/update-files/{id}")]
        public async Task<IActionResult> UpdateImages(string id, [FromForm] (List<string> prevUrls, List<IFormFile> files) request)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            if (request.files == null)
                return Ok("deleted files");
            foreach(var prevUrl in request.prevUrls)
            {
                await _cloudinaryHandler.Delete(prevUrl);
            }
            var fileUrls = await _cloudinaryHandler.UploadImages(request.files,_postFolderName);
            var parameter = new UpdateParameter()
            {
                FieldName = Post.GetFieldName(p => p.FileUrls),
                Value = fileUrls,
                updateAction = UpdateAction.set
            };
            await _postRepository.UpdatebyParameters(id, new List<UpdateParameter> { parameter });
            return Ok("updated");
        }
        [HttpDelete("/delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var deleted = await _postRepository.Delete(id);
            foreach(var item in deleted.FileUrls)
            {
                await _cloudinaryHandler.Delete(item.Value);
            }
            return Ok(("deleted", deleted));
        }

    }
}
