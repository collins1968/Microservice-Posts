using EmailService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Data
{
    public class AppDbContext: DbContext 
    {
        public AppDbContext(DbContextOptions<AppDbContext> options): base(options)
        {
            
        }
        public DbSet<EmailLoggers> EmailLoggers { get; set; }
    }
}
