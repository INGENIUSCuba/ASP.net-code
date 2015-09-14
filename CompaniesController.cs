using System;
using System.Configuration;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataAccess;
using DataAccess.Models;
using SharedHelpers;
using System.Collections.Generic;
using System.Web;
using System.IO;
using AutoMapper;
using AdminOffice365Adoption.ViewModels;

namespace AdminOffice365Adoption.Controllers
{
   // [Authorize(Roles = "ChampionAdmin")]
    public class CompaniesController : Controller
    {
        private readonly MyDbContext _db = new MyDbContext();

        private List<string> Categories = new List<string>() { "skype", "word", "google"};

        public async Task<ActionResult> Index()
        {
            return View(await _db.Companies.ToListAsync());
        }

        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var company = await _db.Companies.FindAsync(id);
            if (company == null)
                return HttpNotFound();

            return View(company);
        }

        public ActionResult Create()
        {
            ViewBag.Categories = Categories;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Name,TenantAdminEmailAddress,HelpMeSupportEmailAddresses")] Company company, HttpPostedFileBase uploadFile)
        {
            if (!ModelState.IsValid)
                return View(company);

            string categories = ";";
            foreach(var categorie in Categories)
            {
                if (Request.Form[categorie] == "on")
                {
                    categories += categorie + ";";
                }
            }
            company.Categories = categories;

            if(uploadFile != null && uploadFile.ContentLength > 0)
            {
                using (var reader = new BinaryReader(uploadFile.InputStream))
                {
                    var imageFile = reader.ReadBytes(uploadFile.ContentLength);
                    //TODO
                    //Upload image file to Azure Blobs and get url to index it
                    
                }
                //TODO
                //Save de url in the Logo field
                company.Logo = Path.GetFileName(uploadFile.FileName);
            }
            

            _db.Companies.Add(company);
            await _db.SaveChangesAsync();

           // await
            // EmailHelper.SendMailAsync(company.TenantAdminEmailAddress, "Office 365 Adoption Sign up", CreateSignUpLink());

            return RedirectToAction("Index");
        }

        private string CreateSignUpLink()
        {
            return
                $"https://login.windows.net/common/oauth2/authorize?response_type=code&client_id={Uri.EscapeDataString(ConfigurationManager.AppSettings["ida:CustomerPortalClientID"])}&resource={Uri.EscapeDataString("https://graph.windows.net")}&redirect_uri={Uri.EscapeDataString(ConfigurationManager.AppSettings["CustomerPortalProcessCodeUrl"])}&prompt={Uri.EscapeDataString("admin_consent")}";
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var company = await _db.Companies.FindAsync(id);
            if (company == null)
                return HttpNotFound();
            ViewBag.Categories = Categories;
            return View(Mapper.Map<CompanyViewModel>(company));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int? id, CompanyViewModel companyViewModel, HttpPostedFileBase uploadFile)
        {
            var company = _db.Companies.Find(id);
            Mapper.Map(companyViewModel, company);
            if (!ModelState.IsValid)
                return View(company);

            string categories = ";";
            foreach (var categorie in Categories)
            {
                if (Request.Form[categorie] == "on")
                {
                    categories += categorie + ";";
                }
            }
            company.Categories = categories;

            if (uploadFile != null && uploadFile.ContentLength > 0)
            {
                using (var reader = new BinaryReader(uploadFile.InputStream))
                {
                    var imageFile = reader.ReadBytes(uploadFile.ContentLength);
                    //TODO
                    //Upload image file to Azure Blobs and get url to index it

                }
                //TODO
                //Save de url in the Logo field
                company.Logo = Path.GetFileName(uploadFile.FileName);
            }
            
            //_db.Entry(company).State = EntityState.Modified;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var company = await _db.Companies.FindAsync(id);
            if (company == null)
                return HttpNotFound();

            return View(company);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            var company = await _db.Companies.FindAsync(id);
            if (company == null)
                return HttpNotFound();

            _db.Companies.Remove(company);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
