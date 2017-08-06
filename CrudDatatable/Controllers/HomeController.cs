using CrudDatatable.Models;
using CrudDatatable.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Net;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Threading.Tasks;

namespace CrudDatatable.Controllers
{

    public class HomeController : Controller
    {
        #region Constructor

        private EmployeeDBEntities ent;
        public HomeController()
        {
            ent = new EmployeeDBEntities();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                ent.Dispose();
            base.Dispose(disposing);
        }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult getEmployee()
        {
            var Draw = Request.Form.GetValues("draw").FirstOrDefault();
            var Start = Request.Form.GetValues("start").FirstOrDefault();
            var Length = Request.Form.GetValues("length").FirstOrDefault();

            var SortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][data]").FirstOrDefault();
            var SortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();

            var firstName = Request.Form.GetValues("columns[0][search][value]").FirstOrDefault();
            var lastName = Request.Form.GetValues("columns[1][search][value]").FirstOrDefault();
            var gender = Request.Form.GetValues("columns[2][search][value]").FirstOrDefault();
            var sal = Request.Form.GetValues("columns[3][search][value]").FirstOrDefault();
            var joinDate = Request.Form.GetValues("columns[4][search][value]").FirstOrDefault();
            var deptName = Request.Form.GetValues("columns[5][search][value]").FirstOrDefault();

            DateTime DJ = DateTime.MinValue;
            if (joinDate != "")
                DJ = joinDate.ToDateHifenddMMyyyy();

            int PageSize = Length != null ? Convert.ToInt32(Length) : 0;
            int Skip = Start != null ? Convert.ToInt32(Start) : 0;
            int TotalRecords = 0;

            IEnumerable emp = (from e in ent.Employees
                               join d in ent.Departments on e.DeptID equals d.DeptID
                               where
                               e.FirstName.Contains(firstName) &&
                               e.LastName.Contains(lastName) &&
                               e.Gender.StartsWith(gender) &&
                               e.Salary.ToString().Contains(sal) &&
                               EntityFunctions.TruncateTime(e.DOJ) >= DJ &&
                               d.DeptName.Contains(deptName)
                               select new EmployeeVM
                               {
                                   ID = e.ID,
                                   FirstName = e.FirstName,
                                   LastName = e.LastName,
                                   Gender = e.Gender,
                                   Salary = e.Salary,
                                   DOJ = e.DOJ,
                                   DeptName = d.DeptName
                               }).ToList();

            if (!(string.IsNullOrEmpty(SortColumn) && string.IsNullOrEmpty(SortColumnDir)))
            {
                emp = emp.OrderBy(SortColumn + " " + SortColumnDir).ToList();
            }

            TotalRecords = emp.ToList().Count();
            var NewItems = emp.Skip(Skip).Take(PageSize == -1 ? TotalRecords : PageSize).ToList();

            return Json(new { draw = Draw, recordsFiltered = TotalRecords, recordsTotal = TotalRecords, data = NewItems }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Create()
        {
            getGender();
            var dept = ent.Departments.ToList();
            var viewModel = new EmployeeViewModel
            {
                Departments = dept
            };
            return PartialView("_CreateEmp", viewModel);
        }

        [HttpPost]
        public ActionResult Create(EmployeeViewModel evm)
        {
            try
            {
                getGender();
                var dept = ent.Departments.ToList();
                evm.Departments = dept;
                if (ModelState.IsValid)
                {
                    Employee emp = Mapper.Map(evm.Employee);
                    ent.Employees.Add(emp);
                    ent.SaveChanges();
                    return Json(new { success = true, message = "Saved Successfully." });
                }
                return PartialView("_CreateEmp", evm);
            }
            catch (Exception ex)
            {
                return new JsonResult
                {
                    Data = new { ErrorMessage = ex.Message, Success = false },
                    ContentEncoding = System.Text.Encoding.UTF8,
                    JsonRequestBehavior = JsonRequestBehavior.DenyGet
                };

            }
        }

        public ActionResult Edit(int? Id)
        {
            try
            {
                if (Id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Employee emp = ent.Employees.Find(Id);
                if (emp == null)
                    return HttpNotFound();
                EmployeeViewModel viewModel = new EmployeeViewModel()
                {
                    Departments = ent.Departments.ToList(),
                    Employee = Mapper.Map(emp)
                };
                getGender();
                return PartialView("_EditEmp", viewModel);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult getEmpDetails(int? EmpID)
        {
            try
            {
                var rslt = (from e in ent.Employees
                            join d in ent.Departments on e.DeptID equals d.DeptID
                            where e.ID == EmpID
                            select new
                            {
                                Name = e.FirstName + " " + e.LastName,
                                Gender = e.Gender,
                                Sal = e.Salary,
                                DOJ = e.DOJ,
                                DeptName = d.DeptName
                            });

                return Json(rslt, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public ActionResult deleteConfirmed(int? EmpID)
        {
            try
            {
                var rslt = ent.Employees.FirstOrDefault(x => x.ID == EmpID);
                if (rslt != null)
                {
                    ent.Employees.Remove(rslt);
                    ent.SaveChanges();
                    return Json("Success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        public ActionResult Edit(EmployeeViewModel evm)
        {
            try
            {
                evm.Departments = ent.Departments.ToList();
                if (ModelState.IsValid)
                {
                    getGender();
                    Employee emp = Mapper.Map(evm.Employee);
                    ent.Entry(emp).State = EntityState.Modified;
                    ent.SaveChanges();

                    return Json(new { success = true, message = "Updated Successfully." });
                }
                return PartialView("_EditEmp", evm);
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        #region Dept & Gender

        private void getGender()
        {
            List<selectlistitem> gender = new List<selectlistitem>();
            gender.Add(new SelectListItem() { Value = "Male", Text = "Male" });
            gender.Add(new SelectListItem() { Value = "Female", Text = "Female" });
            ViewBag.gender = gender;
        }

        private void getDept()
        {
            ViewBag.dept = ent.Departments.ToList();
        }

        #endregion
    }
}