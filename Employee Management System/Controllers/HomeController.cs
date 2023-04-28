using Employee_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Login = Employee_Management_System.Models.Login;

namespace Employee_Management_System.Controllers
{
    public class HomeController : Controller
    {
        //private readonly databaseContext context;
        //public HomeController(databaseContext _context)
        //{
        //    context = _context;
        //}
        EMSEntities4 ems = new EMSEntities4();

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public JsonResult Register(Login login)
        {
            ems.Logins.Add(login);
            ems.SaveChanges();
            return Json(login,JsonRequestBehavior.AllowGet);
        }
        public JsonResult Loginpage(Login login)
        {
            var data = ems.Logins.Where(x => x.Password == login.Password).FirstOrDefault();
            
            if(data.Id > 0)
            {
                Session["logger"] = data.Name;
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return Json(login, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Reset()
        {
            return View();
        }
        [HttpPost]
        public JsonResult Reset(Login login)
         {
            using (EMSEntities4 entities = new EMSEntities4())
            {
                Login updatedCustomer = (from c in entities.Logins
                                            where c.Email == login.Email
                                            select c).FirstOrDefault();

                if (updatedCustomer != null)
                {
                    updatedCustomer.Name = login.Email;
                    updatedCustomer.Password = login.Password;
                    entities.SaveChanges();
                    return Json(true);
                }
            }
            return Json(false);
        }
    }
}