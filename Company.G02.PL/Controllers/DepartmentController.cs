using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.BLL.UnitOfWork;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Company.G02.PL.Controllers
{
    [Authorize]
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
        public async Task<IActionResult> Index()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.DepartmentRepository.AddAsync(department);
                var count = await _unitOfWork.CompleteAsync();
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? Id)
        {
            if(Id is null) return BadRequest(); // 400
            var department = await _unitOfWork.DepartmentRepository.GetAsync(Id.Value);
            if (department is null) return NotFound(); // 404
            return View(department);
        }

        [HttpGet]
        public async Task<IActionResult> Update(int? Id)
        {
            try
            {
                if (Id is null) return BadRequest(); // 400
                var department = await _unitOfWork.DepartmentRepository.GetAsync(Id.Value);
                if (department is null) return NotFound(); // 404
                return View(department);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        // Server Side Validation
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int? Id, Department department)
        {
            try
            {
                if (Id != department.Id) return BadRequest(); // 400
                if (ModelState.IsValid)
                {
                    _unitOfWork.DepartmentRepository.Update(department);
                    var count = await _unitOfWork.CompleteAsync();
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
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                if (Id is null) return BadRequest();
                var department = await _unitOfWork.DepartmentRepository.GetAsync(Id.Value);
                if (department is null) return NotFound();
                return View(department);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? Id, Department department)
        {
            try
            {
                if (Id != department.Id) return BadRequest(); // 400
                if (ModelState.IsValid)
                {
                    _unitOfWork.DepartmentRepository.Delete(department);
                    var count = await _unitOfWork.CompleteAsync();
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
