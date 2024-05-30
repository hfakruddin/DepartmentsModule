using System.Collections.Generic;
using TaskModules.Entities.Models;

namespace TaskModules.Models
{
    public class DepartViewModel
    {
        //public Department department {  get; set; }
        //public List<Department> SubDepartments { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set;}
        public int ParentDeptID { get; set; }
        public string SubDepartmentName { get; set; }
    }
}
