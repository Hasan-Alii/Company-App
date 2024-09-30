﻿using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository repository)
        {
            _departmentRepository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department department)
        {
            if (ModelState.IsValid)
            {
                var count = _departmentRepository.Add(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Details(int? Id, string ViewName = nameof(Update))
        {
            if(Id is null) return BadRequest(); // 400
            var department = _departmentRepository.Get(Id.Value);
            if (department is null) return NotFound(); // 404
            return View(ViewName, department);
        }

        [HttpGet]
        public IActionResult Update(int? Id)
        {
            //if (Id is null) return BadRequest();
            //var department = _employeeRepository.Get(Id.Value);
            //if (department is null) return NotFound();
            return Details(Id, nameof(Update));
        }

        // Server Side Validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute] int? Id, Department department)
        {
            try
            {
                if (Id != department.Id) return BadRequest(); // 400
                if (ModelState.IsValid)
                {
                    var count = _departmentRepository.Update(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(department);
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            //if (Id is null) return BadRequest();
            //var department = _employeeRepository.Get(Id.Value);
            //if (department is null) return NotFound();
            return Details(Id, "Delete");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? Id, Department department)
        {
            try
            {
                if (Id != department.Id) return BadRequest(); // 400
                if (ModelState.IsValid)
                {
                    var count = _departmentRepository.Delete(department);
                    if (count > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(department);
        }
    }
}
