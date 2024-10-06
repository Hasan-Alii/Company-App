using AutoMapper;
using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Company.G02.PL.ViewModels.Employee;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.IdentityModel.Tokens;

namespace Company.G02.PL.Controllers
{
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

        public IActionResult Index(string InputSearch)
        {
            var employees = Enumerable.Empty<Employee>();
            //IEnumerable<Employee> employees;
            if (string.IsNullOrEmpty(InputSearch))
            {
                employees = _unitOfWork.EmployeeRepository.GetAll();
            }
            else
            {
                employees = _unitOfWork.EmployeeRepository.GetByName(InputSearch);
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

        [HttpGet]
        public IActionResult Create()
        {
            var departments = _unitOfWork.DepartmentRepository.GetAll();
            ViewData["departments"] = departments;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            try
            {
                // casting viewmodel -> model and vice versa
                if (ModelState.IsValid)
                {
                    var result = _mapper.Map<Employee>(employee);
                    _unitOfWork.EmployeeRepository.Add(result);
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
            return View(employee);
        }

        [HttpGet]
        public IActionResult Details(int? Id, string ViewName = "Details")
        {
            try
            {
                if (Id is null) return BadRequest(); // 400 
                var employee = _unitOfWork.EmployeeRepository.Get(Id.Value);
                if (employee is null) return NotFound(); // 404
                var result = _mapper.Map<EmployeeViewModel>(employee);
                return View(result);
            }
            catch (Exception e )
            {
                ModelState.AddModelError(string.Empty, e.Message);
                return RedirectToAction("Error", "Home");
            }
        }

        public IActionResult Update(int? Id)
        {
            try
            {
                var departments = _unitOfWork.DepartmentRepository.GetAll();
                ViewData["departments"] = departments;
                if (Id is null) return BadRequest();
                var employee = _unitOfWork.EmployeeRepository.Get(Id.Value);
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
        public IActionResult Update([FromRoute]int? Id, Employee employee)
        {
            try
            {
                if(Id != employee.Id) return BadRequest(); // 400
                if (ModelState.IsValid) // Server Side Validation
                {
                    var result = _mapper.Map<Employee>(employee);
                    _unitOfWork.EmployeeRepository.Update(result);
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
            return View(employee);
        }

        [HttpGet]
        public IActionResult Delete(int? Id)
        {
            try
            {
                var departments = _unitOfWork.DepartmentRepository.GetAll();
                ViewData[index: "departments"] = departments;
                if (Id is null) return BadRequest();
                var employee = _unitOfWork.EmployeeRepository.Get(Id.Value);
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
        public IActionResult Delete([FromRoute]int? Id, Employee employee)
        {
            try
            {
                if (Id != employee.Id) return BadRequest(); // 400
                if (ModelState.IsValid)
                {
                    var result = _mapper.Map<Employee>(employee);
                    _unitOfWork.EmployeeRepository.Delete(result);
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
            return View(employee);
        }
    }
}
