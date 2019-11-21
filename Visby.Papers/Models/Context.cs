using System;
using Microsoft.EntityFrameworkCore;

namespace Karenia.Visby.Papers.Models
{
    public class AccountContext : DbContext
    {
        public AccountContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}