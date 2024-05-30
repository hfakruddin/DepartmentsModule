using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace TaskModules.Models
{
    public class DepartmentEditViewModel
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentLogo { get; set; }
        public IFormFile DepartmentLogoFile { get; set; }
        public int? ParentDeptId { get; set; } // Nullable for parent department
        public List<SelectListItem> AvailableDepartments { get; set; }
    }
}
