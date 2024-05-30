using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TaskModules.Entities;
using TaskModules.Entities.Models;
using TaskModules.Models;
using TaskModules.Services;

namespace TaskModules.Controllers
{
    public class DepartmentsController : Controller
    {
        public IDepartmentRepository _DepartmentRepository { get; }
        public IWebHostEnvironment _environment;

        public DepartmentsController(IDepartmentRepository departmentRepository, IWebHostEnvironment webHostEnvironment)
        {
            _DepartmentRepository = departmentRepository;
            _environment = webHostEnvironment;
        }

        // GET: DepartmentsController
        public ActionResult Index()
        {
           var departments =  _DepartmentRepository.GetDepartmentsList();             
            return View(departments);
        }

        // GET: DepartmentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: DepartmentsController/Create
        public ActionResult Create()
        {
            var departments = _DepartmentRepository.GetDepartmentsList();
                        
            var deptModel = new CreateDeptModel();
            var selItem = new SelectListItem(value: "0", text: "Root Department");
            
            deptModel.AvailableDepartments = _DepartmentRepository.GetDeptSelectListItems();
            deptModel.AvailableDepartments.Add(selItem);
            
            return View(deptModel);
        }

        // POST: DepartmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CreateDeptModel departViewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {                  
                    if (departViewModel.DepartmentLogo == null || departViewModel.DepartmentLogo.Length ==0) 
                    {
                        ModelState.AddModelError("DepartmentLogo", "Please select department logo..");
                        //return View(departViewModel);
                    }
                    else if (departViewModel.DepartmentLogo.Length > 5 * 1024*1024) //5MB limit
                    {
                        ModelState.AddModelError("DepartmentLogo", "Max allowed file size is 5MB");
                        //return View(departViewModel);
                    }
                    else
                    {
                        var filepath = await UploadLogo(departViewModel.DepartmentLogo);

                        var department = new Department();
                        department.DepartmentName = departViewModel.DepartmentName;
                        if (departViewModel.ParentDeptID == 0)
                        {
                            department.ParentDeptID = null;
                        }
                        else
                        {
                            department.ParentDeptID = departViewModel.ParentDeptID;
                        }
                        department.DepartmentLogo = filepath.Substring(8, filepath.Length - 8);

                        var result = await _DepartmentRepository.AddDepartment(department);
                        if (result != null)
                        {
                            return RedirectToAction(nameof(Index));
                        }
                    }

                }
                departViewModel.AvailableDepartments = _DepartmentRepository.GetDeptSelectListItems();
                var selItem = new SelectListItem(value: "0", text: "Root Department");                
                departViewModel.AvailableDepartments.Add(selItem);

                return View(departViewModel);
            }
            catch
            {
                departViewModel.AvailableDepartments = _DepartmentRepository.GetDeptSelectListItems();
                var selItem = new SelectListItem(value: "0", text: "Root Department");
                departViewModel.AvailableDepartments.Add(selItem);
                return View(departViewModel);
            }
        }

        // GET: DepartmentsController/Edit/5
        public ActionResult Edit(int id)
        {
            var department = _DepartmentRepository.GetDepartmentById(id);
            if (department == null)
                return NotFound();

            var viewModel = new DepartmentEditViewModel
            {
                DepartmentId = department.DepartmentId,
                DepartmentName = department.DepartmentName,
                DepartmentLogo = department.DepartmentLogo,
                ParentDeptId = department.ParentDeptID,
                AvailableDepartments = _DepartmentRepository.GetDeptSelectListItems()
            };
            var selItem = new SelectListItem(value: "0", text: "Root Department");
            viewModel.AvailableDepartments.Add(selItem);

            if (department.ParentDept == null && department.SubDepartments.Count >= 0)
            { ViewBag.IsRootDept = true; }

            return View(viewModel);
        }

        // POST: DepartmentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, DepartmentEditViewModel viewModel)
        {
            var filepath = "";
            if (ModelState.IsValid)
            {
                if (viewModel.DepartmentLogoFile != null)
                {
                    if (viewModel.DepartmentLogoFile.Length > 5 * 1024 * 1024) //5MB limit
                    {
                        ModelState.AddModelError("DepartmentLogoFile", "Max allowed file size is 5MB");
                        viewModel.AvailableDepartments = _DepartmentRepository.GetDeptSelectListItems();
                        var sItem = new SelectListItem(value: "0", text: "Root Department");
                        viewModel.AvailableDepartments.Add(sItem);
                        return View(viewModel);
                    }
                    filepath = await UploadLogo(viewModel.DepartmentLogoFile);
                    filepath = filepath.Substring(8, filepath.Length - 8);
                }
                else
                {
                    filepath = viewModel.DepartmentLogo;
                }
                     

                    Department department = new Department
                    {
                        DepartmentId = viewModel.DepartmentId,
                        DepartmentName = viewModel.DepartmentName,
                        ParentDeptID = viewModel.ParentDeptId == 0 ? null : viewModel.ParentDeptId,
                        DepartmentLogo = filepath 
                    };

                    var result = await _DepartmentRepository.UpdateDepartment(department);
                    return RedirectToAction(nameof(Index));
                
                
            }
                
            viewModel.AvailableDepartments = _DepartmentRepository.GetDeptSelectListItems();
            var selItem = new SelectListItem(value: "0", text: "Root Department");
            viewModel.AvailableDepartments.Add(selItem);
            return View(viewModel);
            
        }

        // GET: DepartmentsController/Delete/5
        public ActionResult Delete(int id)
        {
            if (id == 0)
            {
                return NotFound();
            }

            var department = _DepartmentRepository.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            if (department.ParentDept == null && department.SubDepartments.Count > 0) 
            { ViewBag.IsRootDept = true; }

            if (department.ParentDept != null && department.SubDepartments.Count > 0) 
            { ViewBag.IsParentDept = true; }

            return View(department);
            
        }

        // POST: DepartmentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {

            var department = _DepartmentRepository.GetDepartmentById(id);
            if (department != null)
            {
                await _DepartmentRepository.DeleteDepartment(department);
                var filedelete = DeleteLogoFile(department.DepartmentLogo);
                return RedirectToAction("Index"); // Redirect to the department list
            }

            return NotFound();            
        }

        public IActionResult ShowHierarchy()
        {
            var departments = _DepartmentRepository.GetDepartmentsList();
            var deptModel = new DeptModel();
            deptModel.Departments = new List<Departments>();
            if (departments != null)
            {
                foreach (var department in departments)
                {
                    deptModel.Departments.Add(new Departments
                    {
                        DepartmentId = department.DepartmentId,
                        DepartmentName = department.DepartmentName
                    });
                }
            }
            return View(deptModel);            
        }

        [HttpPost()]
        public IActionResult GetDeptHierarchy(int SelectedDeptId, int? disoption)
        {
            var model = new DepartViewModel();
            var department = _DepartmentRepository.GetDepartmentById(SelectedDeptId);
            if (department == null)
            {
                return null;
            }
            ViewBag.SelectedDeptName = department.DepartmentName;

            if (disoption != null && disoption != 0)
            {
                if (disoption == 1)
                {
                    List<Department> subDepts = new List<Department>();
                    subDepts = _DepartmentRepository.GetAllDeparts(SelectedDeptId);
                    ViewBag.disOption = 1;
                    return View(subDepts);
                }
                else if (disoption == 2)
                {
                    var parentDepts = _DepartmentRepository.GetParentDepartments(SelectedDeptId);
                    ViewBag.disOption = 2;
                    return View(parentDepts);
                }
            }

            return null;

        }


        private async Task<string> UploadLogo(IFormFile DepartmentLogo)
        {
            var filename = Guid.NewGuid().ToString() + Path.GetExtension(DepartmentLogo.FileName);
            var filepath = Path.Combine("wwwroot", "uploads", filename);

            using (var stream = new FileStream(filepath, FileMode.Create))
            {
                await DepartmentLogo.CopyToAsync(stream);
            }
            return filepath;
        }

        private bool DeleteLogoFile(string imagePath)
        {          

            // Combine the image path with the root path of your application
            var fullPath = Path.Combine("wwwroot", imagePath);

            // Check if the file exists and delete it
            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);   
                return true;
            }
            return false;
        }
    }
}
