using Company.G02.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Company.G02.BLL.Interfaces
{
    public interface IDepartmentRepository : IGenericRepository<Department>
    {
        //IEnumerable<Department> GetAllAsync();
        //Department GetAsync(int Id);
        //int AddAsync(Department entity);
        //int Update(Department entity);
        //int Delete(Department entity);
    }
}
