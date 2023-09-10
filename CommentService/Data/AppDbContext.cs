using CommentService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommentService.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<Comment> Comments { get; set; }
    }
    
}
