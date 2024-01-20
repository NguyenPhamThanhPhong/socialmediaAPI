﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Embeded.Post;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.RequestsResponses.Requests;
using socialmediaAPI.Services.CloudinaryService;
using System.Security.Claims;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _postFolderName;
        private readonly IMongoCollection<Post> _postCollection;
        private readonly IMongoCollection<User> _userCollection;

        public PostController(IPostRepository postRepository, DatabaseConfigs databaseConfigs,
            CloudinaryHandler cloudinaryHandler, CloudinaryConfigs cloudinaryConfigs)
        {
            _postRepository = postRepository;
            _cloudinaryHandler = cloudinaryHandler;
            _postFolderName = cloudinaryConfigs.PostFolderName;
            _postCollection = databaseConfigs.PostCollection;
            _userCollection = databaseConfigs.UserCollection;
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
        [Authorize]
        [HttpPost("/post-like-unlike/{id}/{updateAction}")]
        public async Task<IActionResult> UpdateLikes(string id, bool updateAction)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var selfId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (selfId == null)
                return Unauthorized("no id found");
            var filter = Builders<Post>.Filter.Eq(s => s.Id, id);
            if (updateAction)
            {
                var update = Builders<Post>.Update.AddToSet(s => s.Likes, selfId);
                var SelfUpdate = Builders<User>.Update.AddToSet(s => s.LikedPostIds, id);
                await Task.WhenAll(_postCollection.UpdateOneAsync(filter, update),
                    _userCollection.UpdateOneAsync(s => s.ID == selfId, SelfUpdate));
            }
            else
            {
                var update = Builders<Post>.Update.Pull(s => s.Likes, selfId);
                var SelfUpdate = Builders<User>.Update.Pull(s => s.LikedPostIds, id);
                await Task.WhenAll(_postCollection.UpdateOneAsync(filter, update),
                    _userCollection.UpdateOneAsync(s => s.ID == selfId, SelfUpdate));
            }
            return Ok("updated");
        }
        [Authorize]
        [HttpPost("/post-save-unsave/{id}/{updateAction}")]
        public async Task<IActionResult> UpdateSaved(string id, bool updateAction)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var selfId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (selfId == null)
                return Unauthorized("no id found");
            if (updateAction)
            {
                var SelfUpdate = Builders<User>.Update.AddToSet(s => s.SavedPostIds, id);
                await _userCollection.UpdateOneAsync(s => s.ID == selfId, SelfUpdate);
            }
            else
            {
                var SelfUpdate = Builders<User>.Update.Pull(s => s.SavedPostIds, id);
                await _userCollection.UpdateOneAsync(s => s.ID == selfId, SelfUpdate);
            }
            return Ok("updated");
        }


        [HttpPost("/post-get-from-ids")]
        public async Task<IActionResult> Get([FromBody] List<string> ids)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var posts = await _postRepository.GetbyIds(ids);
            Console.WriteLine(JsonSerializer.Serialize(posts));
            return Ok(posts);
        }
        [HttpPost("/post-update/{id}")]
        public async Task<IActionResult> UpdateImages(string id, [FromForm] UpdatePostRequest request)  
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            if (request.deleteUrls != null)
                await _cloudinaryHandler.DeleteMany(request.deleteUrls);

            var fileUrls = request.keepUrls;
            if (request.Files != null)
            {
                var uploadUrl = await _cloudinaryHandler.UploadImages(request.Files, _postFolderName);
                if (fileUrls == null)
                    fileUrls = uploadUrl;
                else
                    foreach (var kvp in uploadUrl)
                        fileUrls.Add(kvp.Key, kvp.Value);
            }

            var filter = Builders<Post>.Filter.Eq(s => s.Id, id);
            var update = Builders<Post>.Update
                .Set(s => s.Content, request.Content)
                .Set(s => s.FileUrls, fileUrls)
                .Set(s => s.Title, request.Title)
                .Set(s => s.IsDraft, request.IsDraft);
            await _postCollection.UpdateOneAsync(filter, update);
            return Ok("updated");
        }

        [HttpDelete("/post-delete/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var deleted = await _postRepository.Delete(id);
            if (deleted.FileUrls != null)
                await _cloudinaryHandler.DeleteMany(deleted.FileUrls.Values.ToList());
            return Ok(("deleted", deleted));
        }
        [HttpPost("/post-search")]
        public async Task<IActionResult> PostSearch([FromBody] string search)
        {
            if (!ModelState.IsValid)
                return BadRequest("invalid modelstate");
            var pattern = new BsonRegularExpression(new Regex(Regex.Escape(search), RegexOptions.IgnoreCase));
            var filter = Builders<Post>.Filter.Regex(p => p.Content, pattern);
            var posts = await _postCollection.Find(filter).ToListAsync();
            return Ok(posts);
        }

    }
}
