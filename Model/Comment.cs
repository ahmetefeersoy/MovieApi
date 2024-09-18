using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace api.Model
{
    [Table("Comments")]

    public class Comment
    {
        public int Id { get; set; }

        public int StarRating { get; set; }
        
        public int NumberOfLikes { get; set; }
        public string Content { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; } = DateTime.Now;
        public int? FilmId { get; set; }

        public Films? Film { get; set; } 
        public string AppUserId { get; set; }   
        public AppUser AppUser { get; set; }

        
        }
}