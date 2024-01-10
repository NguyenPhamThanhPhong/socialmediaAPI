using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using socialmediaAPI.Configs;
using socialmediaAPI.Models.Entities;
using socialmediaAPI.Repositories.Interface;
using socialmediaAPI.Services.CloudinaryService;

namespace socialmediaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IMongoCollection<Comment> _commentRepository;
        private readonly CloudinaryHandler _cloudinaryHandler;
        private readonly string _commentFolderName;

        public CommentController(CloudinaryHandler cloudinaryHandler, CloudinaryConfigs cloudinaryConfigs, 
            IMongoCollection<Comment> commentRepository)
        {
            _cloudinaryHandler = cloudinaryHandler;
            _commentFolderName = cloudinaryConfigs.CommentFolderName;
            _commentRepository = commentRepository;
        }
        

    }
}
