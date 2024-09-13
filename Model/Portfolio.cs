using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Model
{
    [Table("Portfolios")]

    public class Portfolio
    {
        public string AppUserId { get; set; }
        public int FilmId { get; set; }
        public AppUser AppUser { get; set; }
        public Films Film { get; set; }
    }
}