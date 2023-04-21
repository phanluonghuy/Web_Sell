using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web_Sell.Models;

namespace Web_Sell.Controllers
{
    public class HomeController : Controller
    {
        PhoneManagerEntities _db = new PhoneManagerEntities();
        public ActionResult Index()
        {
            return View(_db.Products.ToList());
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

        public ActionResult MyOrders()
        {
            return View(_db.Orders.ToList());
        }
    }
}