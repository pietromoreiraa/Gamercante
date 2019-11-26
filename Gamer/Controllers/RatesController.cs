using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Gamer.Models;

namespace Gamer.Controllers
{
    public class RatesController : Controller
    {
        private Context db = new Context();

        // GET: Rates
        public ActionResult Index(string GameId)
        {
            int gameid = Convert.ToInt32(GameId);
                var rates = db.Rates.Where(c => c.GameId == gameid).Select(gp => new { Rates = gp.Rating });
            int resultcount = rates.Count();
            
            int resultTotal = rates.Sum(c => c.Rates);

            decimal medium = resultTotal / resultcount;
            
           
            return View(rates.ToList());
        }

        // GET: Rates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // GET: Rates/Create
       
       
        

        // POST: Rates/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RateId,Rating,UserId,GameId")] Rate rate, string UserId, string GameId,  string Rating)
        {
            
            Context bd = new Context();
            int UId = Convert.ToInt32(UserId);
            int GId = Convert.ToInt32(GameId);
            var rates = db.Rates.Where(c => c.UserId == UId).Where(c => c.GameId == GId);
            try
            {
                var resultrates = rates.Single();
            }
            catch(Exception e)
            { 
                if (ModelState.IsValid)
                {
                    db.Rates.Add(rate);
                    db.SaveChanges();

                }

                ViewBag.GameId = new SelectList(db.Games, "GameID", "Nome", rate.GameId);
            }

                return Redirect(Request.UrlReferrer.ToString());
           
        }

        // GET: Rates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            ViewBag.GameId = new SelectList(db.Games, "GameID", "Nome", rate.GameId);
            return View(rate);
        }

        // POST: Rates/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RateId,Rating,UserId,GameId")] Rate rate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rate).State = EntityState.Modified;
                db.SaveChanges();
                
            }
            ViewBag.GameId = new SelectList(db.Games, "GameID", "Nome", rate.GameId);
            return View(rate);
        }

        // GET: Rates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Rate rate = db.Rates.Find(id);
            if (rate == null)
            {
                return HttpNotFound();
            }
            return View(rate);
        }

        // POST: Rates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Rate rate = db.Rates.Find(id);
            db.Rates.Remove(rate);
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
