using System.Collections.Generic;
using TaskModules.Entities.Models;

namespace TaskModules.Models
{
    public class DeptModel
    {
        public int SelectedDeptId { get; set; }
        public List<Departments> Departments { get; set; }
        //public IEnumerable<Department> Departments { get; set; }
    }

    public class Departments
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set;}
    }
}
