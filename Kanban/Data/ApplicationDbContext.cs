using Kanban.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Task = Kanban.Models.Task;

namespace Kanban.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .HasMany(t => t.Tasks)
                .WithOne(u => u.Creator)
                .OnDelete(DeleteBehavior.NoAction);


            base.OnModelCreating(builder);
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}