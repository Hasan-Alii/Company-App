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
        public IActionResult Create(Department model)
        {
            if (ModelState.IsValid)
            {
                var count = _departmentRepository.Add(model);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public IActionResult Details(int? Id)
        {
            if(Id is null) return BadRequest(); //400
            var department = _departmentRepository.Get(Id.Value);
            if (department is null) return NotFound(); //404
            return View(department);
        }
    }
}
