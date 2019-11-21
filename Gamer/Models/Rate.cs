using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gamer.Models
{
    public class Rate
    {
        [Key]
        public int RateId { get; set; }
        public int Rating { get; set; }
        public int UserId { get; set; }
        public virtual Usuario Usuario { get; set; }
        public int GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}