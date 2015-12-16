using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4_DavaValidation.Models;

namespace MVC4_DavaValidation.Controllers
{
    public class MultiValidateController : Controller
    {
        //
        // GET: /MultiValidate/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(ValidateRegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                return View("Index");
            }
            return View(registerModel);
        }

        //
        // GET: /MultiValidate/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /MultiValidate/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MultiValidate/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MultiValidate/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /MultiValidate/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /MultiValidate/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /MultiValidate/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
