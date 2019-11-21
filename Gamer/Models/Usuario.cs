using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Gamer.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cpf { get; set; }
        public string Celular { get; set; }
        public char Sexo { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Cep { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }

        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}