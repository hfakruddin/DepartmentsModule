using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TaskModules.Entities.Models;
using TaskModules.Models;

namespace TaskModules.Entities
{
    public interface IDepartmentRepository
    {
        
        IEnumerable<Department> GetDepartmentsList();
        List<SelectListItem> GetDeptSelectListItems();
        List<Department> GetAllDeparts(int departID);
        List<Department> GetParentDepartments(int departmentId);
        IEnumerable<Department> GetParentDepartmentsList();
        Department GetDepartmentById(int departmentId);        
        Task<Department> AddDepartment(Department department);
        Task<Department> UpdateDepartment(Department department);
        Task DeleteDepartment(Department department);
    }
}
