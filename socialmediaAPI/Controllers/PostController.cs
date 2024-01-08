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
        [HttpPost("/post-create")]
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
        [HttpPost("/post-update-string-field/{id}")]
        public async Task<IActionResult> UpdateParameters(string id, [FromBody] List<UpdateParameter> parameters)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            await _postRepository.UpdateStringFields(id, parameters);
            return Ok("updated");
        }

        [HttpPost("/post-like-unlike/{id}/{updateAction}")]
        public async Task<IActionResult> UpdateLikes(string id, UpdateAction updateAction, LikeRepresentation likeRepresentation )
        {
            if (!ModelState.IsValid || updateAction == UpdateAction.set)
                return BadRequest("invalid modelstate");
            var parameter = new UpdateParameter(Post.GetFieldName(p=>p.Likes),likeRepresentation,updateAction);
            await _postRepository.UpdatebyParameters(id, new List<UpdateParameter> { parameter });
            return Ok("updated");
        }

        [HttpPost("/post-get-many")]
        public async Task<IActionResult> Get([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var posts = await _postRepository.GetbyIds(ids);
            return Ok(posts);
        }
        [HttpPut("/post-update-files/{id}")]
        public async Task<IActionResult> UpdateImages(string id, [FromForm] UpdateFilesRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            if(request.prevUrls!=null)
                foreach(var prevUrl in request.prevUrls)
                {
                    Console.WriteLine(prevUrl);
                    await _cloudinaryHandler.Delete(prevUrl);
                }
            if (request.files == null)
                return Ok("deleted files");
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
            if(deleted.FileUrls!=null)
                foreach (var item in deleted.FileUrls)
                {
                    await _cloudinaryHandler.Delete(item.Value);
                }
            return Ok(("deleted", deleted));
        }

    }
}
