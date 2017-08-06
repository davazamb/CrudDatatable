using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CrudDatatable.ViewModels
{

    public class EmployeeVM
    {
        public int ID { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string Gender { get; set; }

        public Nullable<int> Salary { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public System.DateTime DOJ { get; set; }

        [Required]
        public Nullable<int> DeptID { get; set; }

        public string DeptName { get; set; }
    }

    public class DepartmentVM
    {
        public int DeptID { get; set; }

        [Required]
        public string DeptName { get; set; }
    }

    public class EmployeeViewModel
    {
        public EmployeeVM Employee { get; set; }
        public IEnumerable Departments { get; set; }
    }
}