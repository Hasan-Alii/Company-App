using Company.G02.BLL.Interfaces;
using Company.G02.BLL.Repositories;
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
        public IActionResult Index()
        {
            var departments = _departmentRepository.GetAll();
            return View(departments);
        }
        
        //public IActionResult Create()
        //{
        //    var departments = _departmentRepository.Add();
        //    return View(departments);
        //}
    }
}
