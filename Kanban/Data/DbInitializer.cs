
using Kanban.Models;
using Microsoft.AspNetCore.Identity;

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

            IdentityRole[] roles = new IdentityRole[]
            {
                new IdentityRole
                {
                    Id = "4af8f32f-1c3c-43b0-9f5a-216f3df61618",
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                },

                new IdentityRole
                {
                    Id = "6bd33099-ebce-44d7-90dc-87312c20ee64",
                    Name = "Client",
                    NormalizedName = "CLIENT"
                },
                new IdentityRole
                {
                    Id = "c0027093-a4d1-4630-8458-f1195bc5e637",
                    Name = "Developer",
                    NormalizedName = "DEVELOPER"
                }
            };

            context.Roles.AddRange(roles);
            context.SaveChanges();

            //Users
            User[] users = new User[]
            {
                new User
                {
                    Id = "7f0aca23-1fc5-447a-88f8-a078f2ab580c",
                    UserName = "admin",
                    FirstName = "Admin",
                    LastName = "Admin",
                    NormalizedUserName = "ADMIN",
                    Email = "admin@admin.com",
                    NormalizedEmail = "ADMIN@ADMIN.COM"
                },
                new User
                {
                    Id = "d504e155-5a72-4da9-8872-71876610436a",
                    UserName = "client",
                    FirstName = "Client",
                    LastName = "Client",
                    NormalizedUserName = "CLIENT",
                    Email = "client@client.com",
                    NormalizedEmail = "CLIENT@CLIENT.COM"
                },

                new User
                {
                    Id = "99f7484c-d997-4b6e-87d4-27d59f5c7c6e",
                    UserName = "developer",
                    FirstName = "Developer",
                    LastName = "Developer",
                    NormalizedUserName = "DEVELOPER",
                    Email = "developer@developer.com",
                    NormalizedEmail = "DEVELOPER@DEVELOPER.COM"
                },

            };

            context.Users.AddRange(users);
            context.SaveChanges();

            PasswordHasher<User> hasher = new();
            users[0].PasswordHash = hasher.HashPassword(users[0], "Admin1!");
            users[1].PasswordHash = hasher.HashPassword(users[1], "Client1!");
            users[2].PasswordHash = hasher.HashPassword(users[2], "Developer1!");

            //UserRoles
            IdentityUserRole<string>[] userRoles = new IdentityUserRole<string>[]
            {
                new IdentityUserRole<string>
                {
                    UserId = users[0].Id,
                    RoleId = roles[0].Id
                },

                new IdentityUserRole<string>
                {
                    UserId = users[1].Id,
                    RoleId = roles[1].Id
                },
                new IdentityUserRole<string>
                {
                    UserId = users[2].Id,
                    RoleId = roles[2].Id
                }
            };

            context.UserRoles.AddRange(userRoles);
            context.SaveChanges();

            Models.Task[] tasks = new Models.Task[]
            {
                new Models.Task
                {
                    Title = "Fix UI",
                    CreatorId = users[0].Id,
                    CreatedDate = DateTime.Now,
                    ExpectedEndDate = DateTime.Now.AddDays(7),
                    Description = "Fix a graphic mistake when clicking the login button",
                    Status = Models.Task.Statuses.Open,
                },
                new Models.Task
                {
                    Title = "Fix Backend",
                    CreatorId = users[0].Id,
                    ExecutorId = users[2].Id,
                    CreatedDate = DateTime.Now,
                    ExpectedEndDate = DateTime.Now.AddDays(7),
                    Description = "Fix message coming from error when entering invalid data",
                    Status = Models.Task.Statuses.InProgress,
                },
                new Models.Task
                {
                    Title = "Fix UX",
                    CreatorId = users[0].Id,
                    ExecutorId = users[2].Id,
                    CreatedDate = DateTime.Now,
                    ExpectedEndDate = DateTime.Now.AddDays(7),
                    Description = "The login button is too far to be easily visible and accessable to users",
                    Status = Models.Task.Statuses.Finished,
                },
            };

            context.Tasks.AddRange(tasks);
            context.SaveChanges();

        }
    }
}
