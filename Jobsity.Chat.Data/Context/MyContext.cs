using Jobsity.Chat.Data.Mapping;
using Jobsity.Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.Data.Context
{
    public class MyContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<MessageEntity> Messages { get; set; }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {   
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
            modelBuilder.Entity<MessageEntity>(new MessageMap().Configure);
        }
    }
}
