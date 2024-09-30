using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
using Company.G02.DAL.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Company.G02.PL.Controllers
{
    public class EmployeeController : Controller
    {
        IEmployeeRepository _employeeRepository { get; set; }

        public EmployeeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees =  _employeeRepository.GetAll().ToList();
            return View(employees);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                var count = _employeeRepository.Add(employee);
                if (count > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(employee);

        }

        [HttpGet]
        public IActionResult Details(int? Id, string ViewName = "Details")
        {
            if (Id is null) return BadRequest(); // 400 
            var employee = _employeeRepository.Get(Id.Value);
            if(employee is null) return NotFound(); // 404
            return View(ViewName, employee);
        }

        public IActionResult Update(int? Id)
        {
            return Details(Id, nameof(Update));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromRoute]int? Id, Employee employee)
        {
            try
            {
                if(Id != employee.Id) return BadRequest(); // 400
                if (ModelState.IsValid)
                {
                    var count = _employeeRepository.Update(employee);
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
            return Details(Id, "Delete");
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
                    var count = _employeeRepository.Delete(employee);
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
