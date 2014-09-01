using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Interext.Models
{
    public class Comment
    {
        [Key]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Text { get; set; }

        public DateTime DateTimeCreated { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Event Event { get; set; }

        public bool ShowDeleteButton { get; set; } // will be changed evry time loaded in the page.
    }
}