using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.Helper;
using Company.G02.PL.ViewModels.Employee;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace Company.G02.PL.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        //private readonly IEmployeeRepository _employeeRepository;
        //private readonly IDepartmentRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EmployeeController(
            //IEmployeeRepository employeeRepository,
            //IDepartmentRepository departmentRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper
            )
        {
            //_employeeRepository = employeeRepository;
            //_departmentRepository = departmentRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region Index of Employees
        public async Task<IActionResult> Index(string SearchInput)
        {
            var employees = Enumerable.Empty<Employee>();
            //IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(SearchInput))
            {
                employees = await _unitOfWork.EmployeeRepository.GetAllAsync();
            }
            else
            {
                employees = await _unitOfWork.EmployeeRepository.GetByNameAsync(SearchInput);
            }

            var result = _mapper.Map<IEnumerable<EmployeeViewModel>>(employees);

            /*
             * View's Dictionary: Transfer Data From Action To View (One Way)
             * 1. ViewData: Property Inherited From Controller Class, Dictionary
             * ViewData["Data01"] = "Hello From ViewData";
             * 
             * 2. ViewBag: Property Inherited From Controller Class, dynamic
             * ViewBag["Data02"] = "Hello From ViewBag"; 
             * 
             * 3. TempData: Property Inherited From Controller Class, Dictionary
             * -  It transfers Data From A Request To Another Request
             * TempData["Data03"] = "Hello From TempData"; 
             */

            return View(result);
        } 
        #endregion

        #region Create Employee
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employee)
        {
            try
            {
                if (employee.Image != null)
                {
                    employee.ImageName = DocumentSettings.Upload(employee.Image, "images");
                }
                // casting viewmodel -> model and vice versa
                if (ModelState.IsValid)
                {
                    var result = _mapper.Map<Employee>(employee);
                    await _unitOfWork.EmployeeRepository.AddAsync(result);
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
            return View(employee);
        }
        #endregion

        #region Details of Employee
        [HttpGet]
        public async Task<IActionResult> Details(int? Id, string ViewName = "Details")
        {
            try
            {
                if (Id is null) return BadRequest(); // 400 
                var employee = await _unitOfWork.EmployeeRepository.GetAsync(Id.Value);
                if (employee is null) return NotFound(); // 404
                var result = _mapper.Map<EmployeeViewModel>(employee);
                return View(result);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }
        #endregion

        #region Update Employee
        public async Task<IActionResult> Update(int? Id)
        {
            try
            {
                var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
                ViewData["departments"] = departments;
                if (Id is null) return BadRequest();
                var employee = await _unitOfWork.EmployeeRepository.GetAsync(Id.Value);
                if (employee is null) return NotFound();
                var result = _mapper.Map<EmployeeViewModel>(employee);
                return View(result);
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromRoute] int? Id, EmployeeViewModel employee)
        {
            try
            {
                if (Id != employee.Id) return BadRequest(); // 400
                if (ModelState.IsValid) // Server Side Validation
                {
                    if (employee.ImageName is not null)
                        DocumentSettings.Delete(employee.ImageName, "images");
                    if (employee.Image != null)
                        employee.ImageName = DocumentSettings.Upload(employee.Image, "images");
                    var result = _mapper.Map<Employee>(employee);
                    _unitOfWork.EmployeeRepository.Update(result);
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
            return View(employee);
        }
        #endregion

        #region Delete Employee
        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            try
            {
                var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();
                ViewData["departments"] = departments;
                if (Id is null) return BadRequest();
                var employee = await _unitOfWork.EmployeeRepository.GetAsync(Id.Value);
                if (employee is null) return NotFound();
                var result = _mapper.Map<EmployeeViewModel>(employee);
                return View(result);
            }
            catch (Exception e)
            {

                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int? Id, EmployeeViewModel employee)
        {
            try
            {
                if (Id != employee.Id) return BadRequest(); // 400
                if (ModelState.IsValid)
                {
                    var result = _mapper.Map<Employee>(employee);
                    _unitOfWork.EmployeeRepository.Delete(result);
                    var count = await _unitOfWork.CompleteAsync();
                    if (count > 0)
                    {
                        if (employee.ImageName != null)
                            DocumentSettings.Delete(employee.ImageName, "images");
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError(string.Empty, e.Message);
            }
            return View(employee);
        } 
        #endregion
    }
}
