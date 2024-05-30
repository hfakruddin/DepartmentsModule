using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using TaskModules.Entities;
using TaskModules.Entities.Models;
using TaskModules.Models;

namespace TaskModules.Services
{
    public class DeptService:IDepartmentRepository
    {
        private readonly RepositoryContext _repositoryContext;

        public DeptService(RepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext;
        }

        public IEnumerable<Department> GetDepartmentsList()
        {
            var departments = _repositoryContext.Department                 
                .ToList();
            return departments;

        }

        public Department GetDepartmentById(int departmentId)
        {
            var department = _repositoryContext.Department
                .Include(d=>d.ParentDept)
                .Include(d=>d.SubDepartments)
                .Where(d=>d.DepartmentId == departmentId).FirstOrDefault();  
            if (department == null)
            {
                return null;
            }
            return department;
        }

        public List<Department> GetAllDeparts(int departID)
        {
            var DeptIDParam = new SqlParameter("@DepartmentID", departID);
            var department = _repositoryContext.Department
                .FromSqlRaw("Exec GetSubDepartments @DepartmentID", DeptIDParam)
                .ToList();


            if (department == null)
            {
                return null;
            }

            return department;
        }

        public List<Department> GetParentDepartments(int departmentId)
        {
            var DeptIDParam = new SqlParameter("@DepartmentID", departmentId);
            var parentDepartments = _repositoryContext.Department
                .FromSqlRaw("Exec GetParentDepartments @DepartmentID", DeptIDParam)
                .ToList();


            if (parentDepartments == null)
            {
                return null;
            }
            return parentDepartments;
        }
        
        public async Task<Department> AddDepartment(Department department)
        {
            var result = await _repositoryContext.Department.AddAsync(department);
            await _repositoryContext.SaveChangesAsync();            
            return result.Entity;
        }

        public async Task<Department> UpdateDepartment(Department department)
        {
            var depart = _repositoryContext.Department.Find(department.DepartmentId);
            if (depart != null)
            {
               depart.DepartmentName = department.DepartmentName;
               depart.ParentDeptID = department.ParentDeptID;
                depart.DepartmentLogo = department.DepartmentLogo;
                
                await _repositoryContext.SaveChangesAsync();
                _repositoryContext.Entry(depart).Reload();
                return depart;
            }
            return null;
        }

        public async Task DeleteDepartment(Department department)
        {
            _repositoryContext.Department.Remove(department);
           await _repositoryContext.SaveChangesAsync();

        }

        public IEnumerable<Department> GetParentDepartmentsList()
        {
            var parentDepartments = _repositoryContext.Department
                .Where(d=>d.DepartmentId == d.ParentDeptID)
                .ToList();
            return parentDepartments;
        }

        public List<SelectListItem> GetDeptSelectListItems()
        {
            return _repositoryContext.Department
                 .Select(d => new SelectListItem
                 {
                     Value = d.DepartmentId.ToString(),
                     Text= d.DepartmentName
                 })
                 .ToList();
        }
    }
}
