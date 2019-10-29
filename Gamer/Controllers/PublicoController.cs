using Gamer.Models;
using Gamer.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Gamer.Controllers
{
    public class PublicoController : Controller
    {
        // GET: Publico
        public ActionResult Logar(string email, string senha)
        {
            if (Funcoes.AutenticarUsuario(email, senha) == false)
            {
                ViewBag.Error = "Insira seu e-mail e senha";
                return View();
            }
            return RedirectToAction("Index", "Home");
        }

        public ActionResult AcessoNegado()
        {
            using (Context c = new Context())
            {
                return View();
            }
        }

        public ActionResult Logoff()
        {
            Gamer.Repositories.Funcoes.Deslogar();
            return RedirectToAction("Logar", "Publico");
        }
    }
}