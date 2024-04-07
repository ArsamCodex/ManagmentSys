using ManagmentSys.DataBase;
using ManagmentSys.Interface;
using ManagmentSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagmentSys.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentsController : ControllerBase
    {
        private readonly IDepartmentRepository departmentRepository;
        public DatabaseContext _Db { get; set; }
        public DepartmentsController(IDepartmentRepository departmentRepository, DatabaseContext db)
        {
            this.departmentRepository = departmentRepository;
           this. _Db = db;
        }

        [HttpGet]
        public async Task<ActionResult> GetDepartments()
        {

            var departments = await _Db.Departments.ToListAsync();
            return Ok(departments);

        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Department>> GetDepartment(int id)
        {
            return await _Db.Departments.FirstOrDefaultAsync(d => d.DepartmentId == id);
        }
    }
}
