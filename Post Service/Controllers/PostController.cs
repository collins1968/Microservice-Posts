using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Post_Service.Model;
using Post_Service.Model.Dto;
using Post_Service.Services.IServices;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Post_Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class PostController : ControllerBase
    {

        private readonly IPostInterface _postInterface;
        private readonly ResponseDto _response;
        private readonly IMapper _mapper;

        public PostController(IPostInterface postInterface, IMapper mapper)
        {
            _postInterface = postInterface;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ResponseDto>> AddPostAsync(AddPost addPost)
        {
            try
            {
                //map the addPost to Post
                var newpost  = _mapper.Map<Post>(addPost);

                //get the user id from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                newpost.UserId = userIdClaim.Value;
                _response.Result = await _postInterface.CreatePost(newpost);
                _response.IsSuccess = true;
                _response.Message = "Post Created Successfully";
                return Ok(_response);

            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return BadRequest(_response);
            }
        }
        //get all posts

        [HttpGet]
        public async Task <ActionResult<ResponseDto>> GetPostsAsync()
        {
            try
            {
                _response.Result = await _postInterface.GetPosts();
                _response.IsSuccess = true;
                _response.Message = "Posts Fetched Successfully";
                return Ok(_response);

             
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return BadRequest(_response);
            }
        }

        //get post by id
        [HttpGet("{id}")]
        public async Task <ActionResult<ResponseDto>> GetPostById(Guid id)
        {
            try
            {
                
                _response.Result = await _postInterface.GetPostById(id);
                _response.IsSuccess = true;
                _response.Message = "Post Fetched Successfully";
                return Ok(_response);

             
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return BadRequest(_response);
            }
        }

        //update post

        [HttpPut]
        [Authorize]
        public async Task <ActionResult<ResponseDto>> UpdatePostAsync(AddPost updatePost, Guid id)
        {
            
            try
            {
                //check if the post exists
                var postId = await _postInterface.GetPostById(id);
                if (postId == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Post Not Found";
                    return NotFound(_response);
                }

                //get the user id from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;

                //only the user who created the post can update it
                if (postId.UserId != userId)
                {
                    _response.IsSuccess = false;
                    _response.Message = "You are not authorized to update this post";
                    return Unauthorized(_response);
                }

                //update the post

                //var post = _mapper.Map<Post>(updatePost);
                var post = new Post()
                {
                    Title = updatePost.Title,
                    Description = updatePost.Description,
                    UserId = userId
                };
                _response.Result = await _postInterface.UpdatePost(post);
                _response.IsSuccess = true;
                _response.Message = "Post Updated Successfully";
                return Ok(_response);

             
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.InnerException.Message;
                return BadRequest(_response);
            }
        }

        //delete post

        [HttpDelete("{id}")]
        [Authorize]
        public async Task <ActionResult<ResponseDto>> DeletePost(Guid id)
        {
            try
            {
                //check if the post exists
                var postId = await _postInterface.GetPostById(id);
                if (postId == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Post Not Found";
                    return NotFound(_response);
                }

                //get the user id from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                string userId = userIdClaim.Value;

                //only the user who created the post can delete it
                if (postId.UserId != userId)
                {
                    _response.IsSuccess = false;
                    _response.Message = "You are not authorized to delete this post";
                    return Unauthorized(_response);
                }

                //delete the post
                await _postInterface.DeletePost(postId);
                _response.IsSuccess = true;
                _response.Message = "Post Deleted Successfully";
                return Ok(_response);

            }
            catch
            (Exception e)
            {
                _response.IsSuccess = false;
                _response.Message = e.Message;
                return BadRequest(_response);
            }
        }

    } 
}