using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskModules.Entities.Models;

namespace TaskModules.Models
{
    public class DepartmentViewModel
    {
        [Required(ErrorMessage ="Please enter depart name")]
        [StringLength(30, ErrorMessage ="Department name should not exceed 30 characters.")]
        public string DepartmentName { get; set; }

        
        public int ParentDeptID { get; set; }
        public string DepartmentLogo { get; set; }
        public bool IsRootDept { get; set; }
        //[Required(ErrorMessage = "Please select a Parent Department.")]
        public List<Departments> ParentDepartment { get; set; }
    }
}
