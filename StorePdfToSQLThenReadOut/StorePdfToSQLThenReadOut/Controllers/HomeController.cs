using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using DataModels;
using DomainClass;
using ViewModels;

namespace StorePdfToSQLThenReadOut.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new PostFiles();
            return View(model);
        }

        public ActionResult Pdfs()
        {
            using (var db = new SignedContractContext())
            {
                var pdfs = db.SignedContracts.ToList();
                return View(pdfs);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Index(PostFiles model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var storeFile = new SignedContract();

            storeFile.SignedFile = new byte[model.File.InputStream.Length];
            storeFile.FileName = model.File.FileName;
            model.File.InputStream.Read(storeFile.SignedFile, 0, storeFile.SignedFile.Length);

            using (var db = new SignedContractContext())
            {
                db.SignedContracts.Add(storeFile);
                db.SaveChanges();
            }
            // now you could pass the byte array to your model and store wherever 
            // you intended to store it

            return Content("Thanks for uploading the file");
        }

        /// <summary>
        /// 由由SQL中讀出PDF內容, 並顯示在瀏覽器上
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult Show(int id)
        {
            using (var db = new SignedContractContext())
            {
                var entry = db.SignedContracts.Find(id);
                return File(entry.SignedFile, "application/pdf");
            }
        }

    }
}