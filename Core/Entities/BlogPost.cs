using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class BlogPost
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public string CreatorId { get; set; }
        public ApplicationUser Creator { get; set; } 

        [Required]
        [MaxLength(250)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTime PublicationDate { get; set; } = DateTime.UtcNow;

        public string Tags { get; set; }

        // If images are file paths or URLs.
        public string ImagePath { get; set; }

        // Navigation property for related comments.
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<BlogLike> Likes { get; set; }
    }
}
