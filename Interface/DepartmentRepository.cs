using ManagmentSys.DataBase;
using ManagmentSys.Models;
using System;

namespace ManagmentSys.Interface
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly DatabaseContext appDbContext;

        public DepartmentRepository(DatabaseContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public Department GetDepartment(int departmentId)
        {
            return appDbContext.Departments
                .FirstOrDefault(d => d.DepartmentId == departmentId);
        }

        public IEnumerable<Department> GetDepartments()
        {
            return appDbContext.Departments;
        }
    }

}
