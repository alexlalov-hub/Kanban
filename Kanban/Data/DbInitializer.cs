
using Kanban.Models;

namespace Kanban.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            //context.Database.EnsureCreated();

            if (context.Users.Any() || context.Tasks.Any() || context.Comments.Any())
            {
                return;
            }

            context.Users.AddRange(
                new User
                {
                    Id = "07387941-53df-4db7-9223-78afcbb307c70",
                    FirstName = "Alex",
                    LastName = "Lalov",
                    Email = "admin@admin.com",
                    UserName= "admin",
                },
                new User
                {
                    Id = "c146d32e-7362-4402-8271-d942f2593bd4",
                    FirstName = "Alex",
                    LastName = "Lalov",
                    Email = "developer@developer.com",
                    UserName= "developer"
                },
                new User
                {
                    Id = "c2fb1f90-0cc6-4484-8e6e-5b371a0090fe",
                    FirstName = "Alex",
                    LastName = "Lalov",
                    Email = "client@client.com",
                    UserName = "client"
                }
            );

            context.SaveChanges();

            context.Tasks.AddRange(
                new Models.Task
                {
                    Id = 1,
                    Title = "Fix UI",
                    CreatorId = "07387941-53df-4db7-9223-78afcbb307c70",
                    ExecutorId = "c146d32e-7362-4402-8271-d942f2593bd4",
                    CreatedDate = DateTime.Now,
                    ExpectedEndDate = DateTime.Now.AddDays(7),
                    Description = "Fix a graphic mistake when clicking the login button",
                    Status = Models.Task.Statuses.Open,
                }

            );

        }
    }
}
