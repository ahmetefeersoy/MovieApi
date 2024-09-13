using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentDto
    {       
          [Required]
          [MinLength(1,ErrorMessage = "You must give at least 1 star")]

          public int StarRating { get; set; }
          
          [Required]
          [MinLength(5,ErrorMessage = "Content must be 5 characters")]
          [MaxLength(250,ErrorMessage = "Content cannot be over 250 characters")]
          public string Content { get; set; } = string.Empty;
    }
}