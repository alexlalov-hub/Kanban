using Microsoft.AspNetCore.Identity;

namespace Kanban.Models
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public IEnumerable<Task> Tasks { get; set; } = new List<Task>();
    }
}
