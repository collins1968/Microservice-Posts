using AutoMapper;
using CommentService.Models;
using CommentService.Models.Dto;
using CommentService.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CommentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommentInterface _commentInterface;
        private readonly ResponseDto responseDto;

        public CommentController(IMapper mapper, ICommentInterface commentInterface)
        {
            _mapper = mapper;
            _commentInterface = commentInterface;
            responseDto = new ResponseDto();
        }

        [HttpPost]
        public async Task<IActionResult> AddCommentAsync(AddComment comment)
        {
            try
            {
                var mappedComment = _mapper.Map<Comment>(comment);

                //get the user id from the token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                mappedComment.UserId = userIdClaim.Value;

                var result = await _commentInterface.AddCommentAsync(mappedComment);
                responseDto.IsSuccess = true;
                responseDto.Message = result;
                return Ok(responseDto);

            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }      
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsAsync()
        {
            try
            {
                var result = await _commentInterface.GetCommentsAsync();
                //check if there is no comment
                if (result == null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "No comments found";
                    return NotFound(responseDto);
                }
                //if there is comments
                responseDto.Result = result;
                responseDto.IsSuccess = true;
                responseDto.Message = "comment fetched successfully";
                return Ok(responseDto);
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentByIdAsync(Guid id)
        {
            try
            {
                var result = await _commentInterface.GetCommentByIdAsync(id);
                responseDto.Result = result;
                responseDto.IsSuccess = true;
                responseDto.Message = "comment fetched successfully";
                return Ok(responseDto);
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentByPostIdAsync(Guid postId)
        {
            try
            {
                var result = await _commentInterface.GetCommentByPostIdAsync(postId);
                //check if there is no comment with that post id
                if (result == null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "No comments found";
                    return NotFound(responseDto);
                }
                //if there is comments with that post id
                responseDto.Result = result;
                responseDto.IsSuccess = true;
                responseDto.Message = "comment fetched successfully";
                return Ok(responseDto);
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCommentAsync(UpdateComment comment, Guid id)
        {
            try
            {
                //get comment by id
                var commentFromDb = await _commentInterface.GetCommentByIdAsync(id);
                //check if comment exists
                if (commentFromDb == null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "comment not found";
                    return NotFound(responseDto);
                }
                //check if user is the owner of the comment
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (commentFromDb.UserId != userIdClaim.Value)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "You are not the owner of this comment";
                    return Unauthorized(responseDto);
                }
                //if all is well, update the comment
                var mappedComment = _mapper.Map(comment, commentFromDb);
                var result = await _commentInterface.UpdateCommentAsync(mappedComment);
                responseDto.IsSuccess = true;
                responseDto.Message = result;
                return Ok(responseDto);

            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCommentAsync(Guid id)
        {
            try
            {
                //get comment by id
                var commentFromDb = await _commentInterface.GetCommentByIdAsync(id);
                //check if comment exists
                if (commentFromDb == null)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "comment not found";
                    return NotFound(responseDto);
                }
                //check if user is the owner of the comment
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (commentFromDb.UserId != userIdClaim.Value)
                {
                    responseDto.IsSuccess = false;
                    responseDto.Message = "You are not the owner of this comment";
                    return Unauthorized(responseDto);
                }
                //if all is well, delete the comment
                var result = await _commentInterface.DeleteCommentAsync(commentFromDb);
                responseDto.IsSuccess = true;
                responseDto.Message = result;
                return Ok(responseDto);

            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.Message = e.InnerException.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, responseDto);
            }
        }
    }
}
