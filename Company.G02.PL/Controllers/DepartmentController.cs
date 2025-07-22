using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        //private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(
            IUnitOfWork unitOfWork
            /*IDepartmentRepository repository*/)
        {
            _unitOfWork = unitOfWork;
            //_departmentRepository = repository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
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
                _unitOfWork.DepartmentRepository.Add(department);
                var count = _unitOfWork.Complete();
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
            var department = _unitOfWork.DepartmentRepository.Get(Id.Value);
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
                    _unitOfWork.DepartmentRepository.Add(department);
                    var count = _unitOfWork.Complete();
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
                    _unitOfWork.DepartmentRepository.Add(department);
                    var count = _unitOfWork.Complete();
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
