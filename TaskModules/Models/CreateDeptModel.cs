using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TaskModules.Models
{
    public class CreateDeptModel
    {
        [Required(ErrorMessage = "Please enter depart name")]
        [StringLength(30, ErrorMessage = "Department name should not exceed 30 characters.")]
        public string DepartmentName { get; set; }


        public int ParentDeptID { get; set; }
        public IFormFile DepartmentLogo { get; set; }
        public bool IsRootDept { get; set; }
        //public IFormFile LogoImage { get; set; }

        //[Required(ErrorMessage = "Please select a Parent Department.")]
        //public List<Departments> ParentDepartment { get; set; }
        public List<SelectListItem> AvailableDepartments { get; set; }
    }
}
