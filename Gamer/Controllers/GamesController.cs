using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gamer.Models;

namespace Gamer.Controllers
{
    public class GamesController : Controller
    {
        private Context db = new Context();

        // GET: Games
        public ActionResult Index()
        {
            var games = db.Games.Include(g => g.Usuario);
            return View(games.ToList());
        }

        // GET: Games/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            return View(game);
        }

        // GET: Games/Create
        public ActionResult Create()
        {
            ViewBag.ID = new SelectList(db.Usuarios, "Id", "Nome");
            return View();
        }

        // POST: Games/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GameID,Nome,Plataforma,Preco,TipoNegocio,Descricao,Categoria,Img,ID")] Game game, HttpPostedFileBase img)
        {
            ViewBag.FotoMensagem = "";
            try
            {
                if (ModelState.IsValid)
                {
                    string fileName = "";
                    string contentType = "";
                    string path = "";
                    
                    if (img != null && img.ContentLength > 0)
                    {
                        fileName = System.IO.Path.GetFileName(img.FileName);
                        contentType = img.ContentType;
                        path = System.Configuration.ConfigurationManager.AppSettings["PathFiles"] + "\\Games\\" + fileName;
                        
                        img.SaveAs(path);
                        game.Img = fileName;
                    }
                    db.Games.Add(game);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                ViewBag.ImgMensagem = "Não foi possível salvar a foto";
            }
            

            ViewBag.ID = new SelectList(db.Usuarios, "Id", "Nome", game.ID);
            return View(game);
        }
    

        // GET: Games/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            if (game == null)
            {
                return HttpNotFound();
            }
            ViewBag.ID = new SelectList(db.Usuarios, "Id", "Nome", game.ID);
            return View(game);
        }

        // POST: Games/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GameID,Nome,Plataforma,Preco,TipoNegocio,Descricao,Img,ID")] Game game, HttpPostedFileBase img)
        {
            ViewBag.ImgMensagem = "";
            try
            {
                string fileName = "";
                string contentType = "";
                string path = "";
                Game gameBD = db.Games.Find(game.GameID);
                if (img != null && img.ContentLength > 0)
                {
                    fileName = System.IO.Path.GetFileName(img.FileName);
                    contentType = img.ContentType;
                    path = System.Configuration.ConfigurationManager.AppSettings["PathFiles"] + "\\Games\\" + fileName;
                    img.SaveAs(path);
                    game.Img = fileName;
                }
                else
                {
                    if (img == null)
                    {
                        if (gameBD.Img != null)
                        {
                            if (gameBD.Img.Length > 0)
                            {
                                //usa valores que ja estao no BD
                                game.Img = gameBD.Img;
                            }
                        }
                    }
                }
            ((IObjectContextAdapter)db).ObjectContext.Detach(gameBD);
                db.Entry(game).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.FotoMensagem = "Não foi possível salvar a foto";
            }
            ViewBag.ID = new SelectList(db.Usuarios, "ID", "Nome", game.ID);
            return View(game);

        }

        // GET: Games/Delete/5
        public ActionResult Delete(int? id, string arquivo)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Game game = db.Games.Find(id);
            try
            {
                if (ModelState.IsValid)
                {
                    string file = "";
                    switch (arquivo)
                    {
                        case "Img":
                            file =
                           System.Configuration.ConfigurationManager.AppSettings["PathFiles"] + "\\Games\\" + game.Img;
                            break;
                    }
                    if (System.IO.File.Exists(file))
                    {
                        System.IO.File.Delete(file);
                        Game gameBD = db.Games.Find(game.GameID);
                        switch (arquivo)
                        {
                            case "Img":
                                game.Img = string.Empty;
                                break;
                        }
                    ((IObjectContextAdapter)db).ObjectContext.Detach(gameBD);
                        db.Entry(game).State = EntityState.Modified;
                        db.SaveChanges();
                        ViewBag.CidadeID = new SelectList(db.Usuarios, "ID", "Nome", game.ID);
                        return View("Edit", game);
                    }
                    else
                    {
                        ViewBag.FotoMensagem = "Não foi possível excluir a foto";
                    }
                }
                else
                {
                    ViewBag.FotoMensagem = "Não foi possível excluir a foto";
                }
            }
            catch (Exception ex)
            {
                ViewBag.FotoMensagem = "Não foi possível excluir a foto";
            }
            ViewBag.CidadeID = new SelectList(db.Usuarios, "ID", "Nome", game.ID);
            return View(game);
        }

        [HttpPost]
        public ActionResult Search(FormCollection fc, string searchString)
        {
            if (!String.IsNullOrEmpty(searchString))
            {
                var games = db.Games.Include(c => c.Usuario).Where(c => c.Nome.Contains(searchString)).OrderBy(o => o.Nome);
                return View("Index", games.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }
        public ActionResult Filter(FormCollection fc, string filterString)
        {
            if (!String.IsNullOrEmpty(filterString))
            {
                var games = db.Games.Where(c => c.Categoria.Contains(filterString)).OrderBy(o => o.Nome);
                return View("Index", games.ToList());
            }
            else
            {
                return RedirectToAction("Index");
            }
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Game game = db.Games.Find(id);
            db.Games.Remove(game);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }


}
