using CommentService.Models;
using CommentService.Models.Dto;

namespace CommentService.Services.IServices
{
    public interface ICommentInterface
    {
        //add comment to a post
       Task <string> AddCommentAsync(Comment comment);

        //get all comments
        Task<IEnumerable<Comment>> GetCommentsAsync();

        //get comment by id
        Task<Comment> GetCommentByIdAsync(Guid id);

        //get comment by post id
        Task<IEnumerable<Comment>> GetCommentByPostIdAsync(Guid postId);

        //update comment
        Task<string> UpdateCommentAsync(Comment comment);

        //delete comment
        Task<string> DeleteCommentAsync(Comment comment);




    }
}
