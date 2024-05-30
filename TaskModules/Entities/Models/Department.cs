using System.Collections.Generic;

namespace TaskModules.Entities.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public string DepartmentLogo { get; set; }

        //Nullable so that root parent department can store NULL value
        public int? ParentDeptID { get; set;} 

        public Department ParentDept { get; set; }
        public ICollection<Department> SubDepartments { get; set; }


    }
}
