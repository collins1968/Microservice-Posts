using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Post_Service.Data;
using Post_Service.Model;
using Post_Service.Model.Dto;
using Post_Service.Services.IServices;

namespace Post_Service.Services
{
    public class PostService : IPostInterface
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public PostService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        

        public async Task<string> CreatePost(Post Post)
        {
            
           _context.Posts.Add(Post);
            await _context.SaveChangesAsync();
            return "Post Created Successfully";

        }

        public async Task<string> DeletePost(Post post)
        {
            _context.Posts.Remove(post);    
            await _context.SaveChangesAsync();
            return "Post Deleted Successfully";

        }

        public Task<Post> GetPostById(Guid id)
        {
            return _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Post>> GetPosts()
        {
            return _context.Posts.ToListAsync();
        }

        public async Task<string> UpdatePost(Post post)
        {
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();
            return "Post Updated Successfully";

        }
    }
}
