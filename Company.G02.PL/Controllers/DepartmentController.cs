using Company.G02.BLL.Interfaces;
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

        public IActionResult Details(int? Id)
        {
            if(Id is null) return BadRequest(); //400
            var department = _departmentRepository.Get(Id.Value);
            if (department is null) return NotFound(); //404
            return View(department);
        }

        [HttpGet]
        public IActionResult Update(int? Id)
        {
            if (Id is null) return BadRequest();
            var department = _departmentRepository.Get(Id.Value);
            if (department is null) return NotFound();
            return View(department);
        }

        public IActionResult Update(Department dept)
        {
            if (ModelState.IsValid)
            {
                var count = _departmentRepository.Update(dept);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(dept);
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            if (Id is null) return BadRequest();
            var department = _departmentRepository.Get(Id.Value);
            if (department is null) return NotFound();
            return View(department);
        }

        public IActionResult Delete(Department department)
        {
            if (ModelState.IsValid)
            {
                var count = _departmentRepository.Delete(department);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }
    }
}
