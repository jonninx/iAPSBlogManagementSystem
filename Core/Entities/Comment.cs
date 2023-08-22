using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Comment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [Required]
        public string AuthorName { get; set; }

        public string AuthorEmail { get; set; }

        [Required]
        [MaxLength(1000)] 
        public string Content { get; set; }

        public DateTime CommentDate { get; set; } = DateTime.UtcNow;

        public Guid BlogPostId { get; set; }

        [ForeignKey("BlogPostId")]
        public virtual BlogPost BlogPost { get; set; }
    }
}
