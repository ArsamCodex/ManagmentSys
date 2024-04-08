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
            try
            {
                var departments = await _Db.Employees.ToListAsync();
                return Ok(departments);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            try
            {
                var employees = await _Db.Employees
                   .Include(e => e.Department)
                   .FirstOrDefaultAsync(e => e.EmployeeId == id);
                if (employees == null) {
                    return NotFound();
                }
                return employees;
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost]
        public async Task<ActionResult<Employee>> CreateEmployee( Employee employee)
        {
            try
            {
                await _Db.Employees.AddAsync(employee);
                await _Db.SaveChangesAsync();

                // Return the newly created employee with a status code of 201 (Created)
                return CreatedAtAction(nameof(GetEmployee), new { id = employee.EmployeeId }, employee);
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
                // You can customize the error message as needed
            }
        }
        [HttpPut]
        public async Task<ActionResult<Employee>> UpdateEmployee([FromBody] Employee employee)
        {
            try
            {
                var result = await _Db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == employee.EmployeeId);

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

                    return Ok(result); // Return updated employee with status code 200 (OK)
                }

                return NotFound(); // Employee not found
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
                // You can customize the error message as needed
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Employee>> DeleteEmployee(int id)
        {
            try
            {
                var employee = await _Db.Employees.FirstOrDefaultAsync(e => e.EmployeeId == id);

                if (employee == null)
                {
                    return NotFound(); // Employee not found
                }

                _Db.Employees.Remove(employee);
                await _Db.SaveChangesAsync();

                return Ok(employee); // Return the deleted employee
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
                // You can customize the error message as needed
            }
        }

        [HttpGet("{search}")]
        public async Task<ActionResult<IEnumerable<Employee>>> Search(string name, Gender? gender)
        {
            try
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

                var result = await query.ToListAsync();

                if (result == null || result.Count == 0)
                {
                    return NotFound(); // No employees found
                }

                return Ok(result); // Return the list of employees
            }
            catch (Exception ex)
            {
                return BadRequest($"An error occurred: {ex.Message}");
                // You can customize the error message as needed
            }
        }
    }
}
