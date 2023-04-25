using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Xml.Linq;
using Web_Sell.Models;

namespace Web_Sell.Controllers
{
    public class LoginController : Controller

    {
        PhoneManagerEntities _db = new PhoneManagerEntities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Agents agent)
        {
            var check = _db.Agents.FirstOrDefault(s => s.Email == agent.Email);
            if (check == null)
            {
                _db.Configuration.ValidateOnSaveEnabled = true;
                _db.Agents.Add(agent);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.error = "Email already exits";
                return View();
            }
            //return View();
        }

        public ActionResult Authen(ManagerLogin managerLogin)
        {
            var check=_db.ManagerLogin.Where(s => s.Username == managerLogin.Username 
            && s.Password == managerLogin.Password && s.isAdmin == false ).FirstOrDefault();
            if (check == null)
            {
                TempData["isLogin"] = "false";
                //managerLogin.LoginErrorMessage = "Can not sign !! Please input correct";
                return View("Login",managerLogin);      
            }
            else
            {
                Session["AgentID"] = check.AgentID;
                //Session["AgentName"] = _db.Agents.Where(s => s.AgentID == managerLogin.AgentID).ToString();
                //var test = _db.Agents.Where(s => s.AgentID == managerLogin.AgentID); 
                //var name = _db.ManagerLogin.Where(s=>s.AgentID == managerLogin.AgentID).FirstOrDefault();
                //Session["name"] = name.AgentName;
                var test = _db.Agents.Where(s=>s.AgentID== check.AgentID).FirstOrDefault();
                if (test != null)
                {
                     Session["name"] = test.AgentName;
                }
                return RedirectToAction("Index", "Home");
            }
        }
        public ActionResult Login(ManagerLogin managerLogin)
        {
            return View();
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
    }
}