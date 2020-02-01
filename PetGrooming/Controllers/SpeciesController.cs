using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;


namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
       
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
           List<Species> myspecies= db.Species.SqlQuery("select* from species").ToList();
            return View(myspecies);
        }


        [HttpPost]
        public ActionResult Add(string speciesName)
        {
            //creating a query
            string query = "insert into species (Name) values (@speciesName)";
            //calling the first parameter
            SqlParameter[] sqlparams = new SqlParameter[1]; 
            sqlparams[0] = new SqlParameter("@speciesName", speciesName);
           //running the query
            db.Database.ExecuteSqlCommand(query, sqlparams);
            //returning to the list page
            return RedirectToAction("List");
        }


        public ActionResult Add()
        {
            return View();
        }



        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Species species = db.Species.SqlQuery("select * from species where speciesid=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }

        public ActionResult Delete(int id)
        {

            string query = "delete from species where speciesid=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparam);

            return RedirectToAction("list");
        }


        public ActionResult Update(int id)
        {
            string query = "select * from species where speciesid=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);

            Species selectedspecies = db.Species.SqlQuery(query, sqlparam).FirstOrDefault();
            return View(selectedspecies);
        }
        [HttpPost]
        public ActionResult Update(int id, string SpeciesName)
        {
            string query = "update species set Name=@SpeciesName where speciesid=@id";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);
            sqlparams[1] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
    }
}