using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Kanban.Models
{
    public class Task
    {
        public enum Statuses
        {
            Open,
            InProgress,
            Finished
        }

        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} е задължително")]
        [Display(Name = "Заглавието")]
        [StringLength(100)]
        public string? Title { get; set; }

        public string? CreatorId { get; set; }

        public User? Creator { get; set; }

        public string? ExecutorId { get; set; }

        public User? Executor { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime CreatedDate { get; set; } = DateTime.Today;

        [Column(TypeName = "date")]
        public DateTime BeginDate { get; set; }

        [Required(ErrorMessage = "{0} е задължителна")]
        [Display(Name = "Крайната дата")]
        [Column(TypeName = "date")]
        public DateTime ExpectedEndDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime ActualEndDate { get; set; }

        [Required(ErrorMessage = "{0} е задължително")]
        [Display(Name = "Описанието")]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        public Statuses Status { get; set; }

        public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    }
}
