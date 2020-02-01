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
using PetGrooming.Models.ViewModels;
using PetGrooming.Models;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class PetController : Controller
    {
       
        private PetGroomingContext db = new PetGroomingContext();

        //this to show the list of the pets.
        public ActionResult List()
        {
            
            var pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            return View(pets);
        }

      //this function show the details of one individual pet.
        public ActionResult Show(int? id)
        {
            if (id == null)

            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }
        //this will add up a new pet to the list.
        [HttpPost]
        public ActionResult Add(string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes)
        {
            string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes)";
            SqlParameter[] sqlparams = new SqlParameter[5]; 
            sqlparams[0] = new SqlParameter("@PetName",PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes",PetNotes);

            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }


        public ActionResult Add()
        {
            

            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            return View(species);
        }

      // this will update the details of the pet whenever needed.
        public ActionResult Update(int id)
        {
            
            Pet selectedpet = db.Pets.SqlQuery("select * from pets where petid = @id", new SqlParameter("@id", id)).FirstOrDefault();
            List<Species> species = db.Species.SqlQuery("select * from species").ToList();

            UpdatePet viewmodel = new UpdatePet();
            viewmodel.pet = selectedpet;
            viewmodel.species = species;
            return View(viewmodel);
        }

        [HttpPost]
        public ActionResult Update(string PetName, string PetColor, double PetWeight, int SpeciesID,string PetNotes, int id)
        {

            
            string query = "update pets set PetName=@PetName, Weight=@PetWeight, color=@PetColor, Notes=@PetNotes, SpeciesID=@SpeciesID where petid=@id"; 
            SqlParameter[] sqlparams = new SqlParameter[6]; 
            sqlparams[0] = new SqlParameter("@PetName", PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes", PetNotes);
            sqlparams[5] = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        //this will help delete the enteries which are not needed anymore.
        public ActionResult Delete(int id)
        {

            string query = "delete from pets where petid=@id";
            SqlParameter sqlparam = new SqlParameter("@id", id);

            db.Database.ExecuteSqlCommand(query, sqlparam);

            return RedirectToAction("list");
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
