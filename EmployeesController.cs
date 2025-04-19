
using Microsoft.AspNetCore.Mvc;
using MyDbApp.Data;
using MyDbApp.Models;
using MySqlConnector;

namespace MyDbApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetEmployees")]  //read
        public async Task<ActionResult<IEnumerable<dynamic>>> GetEmployees()
        {
            var sql = "SELECT * FROM Employees";
            var Employees = await _context.ExecuteSqlQueryAsync(sql);
            return Ok(Employees);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetEmployeesbyid(int id)
        {
            var sql = "SELECT * FROM Employees WHERE employees_id = @id";
            var parameters = new[] { new MySqlParameter("@id", id) };
            var employees = await _context.ExecuteSqlQueryAsync(sql, parameters);

            if (employees == null || !employees.Any())
            {
                return NotFound();
            }
            return Ok(employees.First());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> DeleteEmployees(int id)
        {
            var sql = "DELETE FROM Employees WHERE employees_id = @id";
            var parameters = new[] { new MySqlParameter("@id", id) };
            await _context.ExecuteSqlQueryAsync(sql, parameters);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> CreateEmployees([FromBody] EmployeesData employeesData)
        {
            var sql = "INSERT INTO Employees (manager_id, first_name, last_name, branch_id, salary) VALUES (@managerid, @firstname, @lastname, @branchid, @Sal); SELECT LAST_INSERT_ID()";
            var parameters = new[]
            {
                new MySqlParameter("@managerid", employeesData.manager_id),
                new MySqlParameter("@firstname", employeesData.first_name),
                new MySqlParameter("@lastname", employeesData.last_name),
                new MySqlParameter("@branchid", employeesData.branch_id),
                new MySqlParameter("@Sal", employeesData.salary)
            };
            var id = await _context.ExecuteSqlQueryAsync(sql, parameters);
            return CreatedAtAction(nameof(GetEmployees), new { id = id }, employeesData);
        }
        [HttpPatch]
        public async Task<ActionResult<dynamic>> UpdateEmployees([FromBody] EmployeesData employeesData)
        {
            var sql = "UPDATE Employees SET manager_id = @managerid, first_name = @firstname, last_name = @lastname, branch_id = @branchid, salary = @Sal WHERE employees_id = @id";
            var parameters = new[] {
                 new MySqlParameter("@managerid", employeesData.manager_id),
                new MySqlParameter("@firstname", employeesData.first_name),
                new MySqlParameter("@lastname", employeesData.last_name),
                new MySqlParameter("@branchid", employeesData.branch_id),
                new MySqlParameter("@Sal", employeesData.salary),
                new MySqlParameter("@id", employeesData.employees_id)
                };
            var id = await _context.ExecuteSqlQueryAsync(sql, parameters);
            return CreatedAtAction(nameof(GetEmployees), new { id = employeesData.employees_id }, employeesData);

        }
        //Need to define some sql joints
    }
 }