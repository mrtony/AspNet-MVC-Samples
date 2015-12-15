using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC4_DavaValidation.Models;

namespace MVC4_DavaValidation.Controllers
{
    public class UserController : Controller
    {
        //
        // GET: /User/

        public ActionResult Index()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Index(User model)
        {
            if (model.AddErrorEmail.Equals("t@gmail.com"))
            {
                ModelState.AddModelError("ModelStateError", "Email不可以是t@gmail.com");
            }

            if (ModelState.IsValid)
            {
                ViewBag.Name = model.UserName;
                ViewBag.Email = model.Email;
                ViewBag.Password = model.Password;
                ViewBag.Mobile = model.Phone;
            }
            return View(model);
        }

        //
        // GET: /User/Details/5

        [HttpGet]
        public ActionResult Customer()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Customer(Customer customerInput)
        {
            if (ModelState.IsValid)
            {
                // store the data
                // ...
                //return RedirectToAction("Index", "Home");
                return View(customerInput);
            }
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(AddUrlViewModel userInput)
        {
            if (ModelState.IsValid)
            {
                // store the data
                // ...
                //return RedirectToAction("Index", "Home");
                return View(userInput);
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /User/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /User/Create

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
        // GET: /User/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /User/Edit/5

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
        // GET: /User/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /User/Delete/5

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
