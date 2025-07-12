using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Interfaces
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Task<IEnumerable<Employee>> GetByNameAsync(string name);
        //IEnumerable<Employee> GetAllAsync();
        //Employee GetAsync(int Id);
        //int AddAsync(Employee entity);
        //int Update(Employee entity);
        //int Delete(Employee entity);
    }
}
