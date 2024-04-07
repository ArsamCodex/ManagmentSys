using ManagmentSys.DataBase;
using ManagmentSys.Interface;
using ManagmentSys.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;

namespace ManagmentSys.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeRepository;
        public DatabaseContext _Db { get; set; }

        public EmployeesController(IEmployeeService employeeRepository, DatabaseContext db)
        {
            this.employeeRepository = employeeRepository;
            this._Db = db;
        }

        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            var departments = await _Db.Employees.ToListAsync();
            return Ok(departments);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            return await _Db.Employees
               .Include(e => e.Department)
               .FirstOrDefaultAsync(e => e.EmployeeId == id);
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee( Employee employee)
        {
            await _Db.Employees.AddAsync(employee);
            await _Db.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK);
        }
        [HttpPut]
        public async Task<ActionResult<Employee>> UpdateEmployee([FromBody] Employee employee)
        {
            var result = await _Db.Employees
                 .FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

            if (result != null)
            {
                result.FirstName = employee.FirstName;
                result.LastName = employee.LastName;
                result.Email = employee.Email;
                result.DateOfBrith = employee.DateOfBrith;
                result.Gender = employee.Gender;
                result.DepartmentId = employee.DepartmentId;
                result.PhotoPath = employee.PhotoPath;

                await _Db.SaveChangesAsync();

                return result;
            }
            return null;

        }
        /*
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                var employeeToDelete = await employeeRepository.GetEmployee(id);

                if (employeeToDelete == null)
                {
                    return NotFound($"Employee with Id = {id} not found");
                }

                return await employeeRepository.DeleteEmployee(id);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }*/
        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            IQueryable<Employee> query = _Db.Employees;

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(e => e.FirstName.Contains(name)
                            || e.LastName.Contains(name));
            }

            if (gender != null)
            {
                query = query.Where(e => e.Gender == gender);
            }

            return await query.ToListAsync();
        }
    }
}
