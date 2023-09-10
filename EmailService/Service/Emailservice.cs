﻿using EmailService.Data;
using EmailService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmailService.Service
{
    public class Emailservice
    {
        private DbContextOptions<AppDbContext> options;

        public Emailservice(DbContextOptions<AppDbContext> options)
        {
            this.options = options;
        }


        public async Task SaveData(EmailLoggers emailLoggers)
        {
            var _context = new AppDbContext(this.options);
            _context.EmailLoggers.Add(emailLoggers);
            await _context.SaveChangesAsync();
        }
    }
}
