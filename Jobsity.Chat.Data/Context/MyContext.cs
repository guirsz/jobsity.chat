using Jobsity.Chat.Data.Mapping;
using Jobsity.Chat.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jobsity.Chat.Data.Context
{
    public class MyContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }

        public MyContext(DbContextOptions<MyContext> options) : base(options)
        {
            Users.Add(new UserEntity()
            {
                Name = "Guilherme",
                Email = "guirsz@gmail.com",
                Password = "AB0pKbH5oxV86+35xEZIk66RmdmwEuk8NtO+F6sCumXZDynEpwMs3cRVtMFiQj5SdQ==", //jobsity
            });
            SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(new UserMap().Configure);
        }
    }
}
