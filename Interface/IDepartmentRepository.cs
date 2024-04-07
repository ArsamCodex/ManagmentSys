using ManagmentSys.Models;

namespace ManagmentSys.Interface
{
    public interface IDepartmentRepository
    {
        IEnumerable<Department> GetDepartments();
        Department GetDepartment(int departmentId);
    }
}
