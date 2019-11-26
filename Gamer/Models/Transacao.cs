using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Gamer.Models
{
    public class Transacao
    {
        [Key]
        public int TId { get; set; }
        public int TCount { get; set; }
        public DateTime Tday { get; set; }
    }
}