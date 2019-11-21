using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Gamer.Models
{
    public class Game
    {
        [Key]
        public int GameID { get; set; }
        [Required(ErrorMessage = "Preencha o nome do jogo")]
        [DisplayName("Nome do jogo")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "O nome da jogo deve ter entre 2 e 100 caracteres.")]
        public string Nome { get; set; }
        public string Plataforma { get; set; }
        public decimal Preco { get; set; }
        public int TipoNegocio { get; set; }
        public string Descricao { get; set; }
        public string Categoria { get; set; }
        public string Img { get; set; }

        public int ID { get; set; }
        public virtual Usuario Usuario { get; set; }

        public virtual ICollection<Rate> Rates { get; set; }
    }
}