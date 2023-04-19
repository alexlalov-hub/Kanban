using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Kanban.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public int TaskId { get; set; }

        [ForeignKey("TaskId")]
        public Task? Task { get; set; }

        [Required(ErrorMessage = "{0} е задължителен")]
        [Display(Name = "Текстът")]
        public string CommentText { get; set; }

        public User? User { get; set; }

        public DateTime DatePosted { get; set; }
    }
}
