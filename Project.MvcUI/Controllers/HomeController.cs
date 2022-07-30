using Project.Entity;
using Project.ORM;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Project.MvcUI.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index() {
            List<Categories> categories = CategoriesORM.Current.Select().Data;
            return View(categories);
        }
    }
}