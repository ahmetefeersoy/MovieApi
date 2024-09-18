using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model
{
    [Table("CommentLikePortfolio")]

    public class CommentLikePortfolio
    { 
        public string AppUserId { get; set; }
        public int CommentId { get; set; }
        public AppUser AppUser { get; set; }
        public Comment Comment { get; set; }

    }
}