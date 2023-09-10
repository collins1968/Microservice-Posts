using CommentService.Data;
using CommentService.Models;
using CommentService.Models.Dto;
using CommentService.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Services
{
    public class CommentServices : ICommentInterface
    {
        private readonly AppDbContext _context;

        public CommentServices(AppDbContext context)
        {
            _context = context;
        }
        public async Task<string> AddCommentAsync(Comment comment)
        {
           _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return "Comment Added Successfully";
        }

        public async Task<string> DeleteCommentAsync(Comment comment)
        {
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return "Comment Deleted Successfully";
        }

        public Task<Comment> GetCommentByIdAsync(Guid id)
        {
            return _context.Comments.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Comment>> GetCommentByPostIdAsync(Guid postId)
        {
            return await _context.Comments.Where(x => x.PostId == postId).ToListAsync();
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }

        public async Task<string> UpdateCommentAsync(Comment comment)
        {
           _context.Comments.Update(comment);
           await _context.SaveChangesAsync();
           return "Comment Updated Successfully";
        }
    }
}
