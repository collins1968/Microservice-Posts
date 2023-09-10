using Microsoft.EntityFrameworkCore;
using Post_Service.Model;

namespace Post_Service.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }

        public DbSet<Post> Posts { get; set; }
    }
}
