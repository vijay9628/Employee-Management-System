using Employee_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Employee_Management_System.Controllers
{
    public class EMSController : Controller
    {
        // GET: EMS
        EMSEntities4 entities = new EMSEntities4();
        public ActionResult Index()
        {
            ViewBag.department = entities.Departments.ToList();
            return View();
        }
        public JsonResult getEmpdata()
        {
            var data = entities.Employees.ToList();
         
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddEmp(Employee emp)
        {
            if(emp.DOB != null  && emp.JoiningDate != null)
            {
                var employee = new Employee();
                employee.EmpName = emp.EmpName;
                employee.EmpGender= emp.EmpGender;
                employee.EmpDepartment= emp.EmpDepartment;
                employee.DOB= emp.DOB;
                employee.JoiningDate= emp.JoiningDate;
                employee.EmpDailySalary = emp.EmpDailySalary;
                entities.Employees.Add(employee);
               // entities.Set<Employee>().Add(emp);
                entities.SaveChanges();
                return Json(employee, JsonRequestBehavior.AllowGet);
            }
            return Json(false);
        }

        [HttpPost]
        public JsonResult editEmployee(Employee editemp)
        {

            if(editemp.Id> 0)
            {
                if(editemp.DOB != null && editemp.JoiningDate != null )
                {
                   var data= entities.Entry(editemp).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                    return Json(data,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false);
                }
            }
            else
            {
                return Json(false);
            }
        }


        public ActionResult subviewpartial()
        {
            return PartialView();
        }
        public ActionResult Department()
        {
            var data = entities.Departments.ToList();
            return View(data);
        }
        public JsonResult depData()
        {
            var data = entities.Departments.ToList();
            return Json(data,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult Department(Department department)
        {
           var data= entities.Departments.Add(department);
            entities.SaveChanges();
            return Json(entities.Departments.ToList(),JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult UpdateDep(Department dep)
        {
            if(dep.Id >0)
            {
                    Department department = (from dd in entities.Departments where dd.Id == dep.Id select dd).FirstOrDefault();
                    if(department != null)
                    {
                        department.Department_Name = dep.Department_Name;
                        entities.SaveChanges();
                        return Json(true);
                    }
            }
            return Json(false);
        }
        [HttpPost]
        public JsonResult DeleteDep(Department dep)
        {
          if(dep.Id > 0)
            { 
                var data = entities.Departments.Find(dep.Id);
                entities.Departments.Remove(data);
                entities.SaveChanges();
                return Json(true);
            }
            return Json(false);
        }
        public ActionResult CreateDep(Department d)
        {
            return PartialView("_CreateDep",d);
        }

        //[HttpGet]
        //public ActionResult salary(int id)
        //{
        //    var data= entities.Employees.Where(x => x.Id == id).FirstOrDefault();
        //    ViewBag.sal = data.EmpDailySalary;
        //    return View();
        //}
        public ActionResult Salary(SalaryDetail salary)
        {
            ViewBag.salary = entities.Employees.ToList();
            var data = entities.SalaryDetails.ToList();
            return View(data);
        }
        //[HttpGet]
        //public JsonResult salary()
        //{
        //    var data = entities.SalaryDetails.ToList();
        //    return Json(data,JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult salary(SalaryDetail detail)
        {
            try
            {
                if (detail.EmpNo > 0)
                {
                    var user = entities.SalaryDetails.Create();
                    user.SalaryAmount = detail.SalaryAmount;
                    user.Period = detail.Period;
                    user.DayAttend = detail.DayAttend;
                    user.EmpNo = detail.EmpNo;
                    entities.SalaryDetails.Add(user);
                    entities.SaveChanges();
                    var data = entities.SalaryDetails.ToList();

                    return Json(data, JsonRequestBehavior.AllowGet);
                }
                {
                    var ndata = entities.SalaryDetails.ToList();
                    return Json(ndata, JsonRequestBehavior.AllowGet);

                }
            }
            catch(System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
          
        }
        public JsonResult Editsalary()
        {
            return Json(true);
        }
        [HttpPost]
        public JsonResult Editsalary(SalaryDetail detail)
        {
            if(detail.SCode> 0)
            {
                if(detail.Period != null)
                {
                  var data=  entities.Entry(detail).State = System.Data.Entity.EntityState.Modified;
                    entities.SaveChanges();
                    return Json(data,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(false);
                }
             
            }
            else
            {
                return Json(false);

            }
        }

        [HttpGet]
        public JsonResult Empdata(int id)
        {
            if(id > 0)
            {
                var data = entities.Employees.Where(x => x.Id == id).FirstOrDefault();
                ViewData["eds"] = data.EmpDailySalary;
                return Json(data,JsonRequestBehavior.AllowGet);
            }
            return Json(false);
        }


    }
}