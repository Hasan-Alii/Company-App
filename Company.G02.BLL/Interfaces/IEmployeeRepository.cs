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
        //IEnumerable<Employee> GetAll();
        //Employee Get(int Id);
        //int Add(Employee entity);
        //int Update(Employee entity);
        //int Delete(Employee entity);
    }
}
