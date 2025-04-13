using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyDbApp.Data;
using MyDbApp.Models;
using MySqlConnector;

namespace MyDbApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JointsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public JointsController(ApplicationDbContext context)
        {
            _context = context;
        }
        // 1. Employees and their associated Clients
        [HttpGet("GetEmployeesWithClients")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetEmployeesWithClients()
        {
            var sql = @"
                  SELECT Employees.first_name, Employees.last_name, Clients.company_name, Clients.client_email_id 
                FROM Employees
               LEFT JOIN Clients ON Employees.employees_id = Clients.employee_id";
            var result = await _context.ExecuteSqlQueryAsync(sql);
            return Ok(result);
        }
        //2. Subqueries - Get vendors who are associated with a branch located in 'Canada'
        [HttpGet("GetVendorsInCanada")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetVendorsInCanada()
        {
            var sql = @"
                  SELECT vendor_name 
                 FROM Vendor 
                WHERE vendor_id IN (
                 SELECT VendorRelation.vendor_id 
                 FROM VendorRelation
                JOIN OfficeBranches ON VendorRelation.branch_id = OfficeBranches.branch_id
               WHERE OfficeBranches.country_name = 'Canada'
              ); ";
            var result = await _context.ExecuteSqlQueryAsync(sql);
            return Ok(result);
        }
        //3. Multiple Table Joints- Show all employees along with their client info and the name of the branch they work at.
        [HttpGet("GetEmployeesClientsBranches")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetEmployeesClientsBranches()
        {
            var sql = @"
            SELECT
            e.first_name, e.last_name,  c.company_name,  c.client_email_id,  b.branch_name 
            FROM Employees e
           JOIN Clients c ON e.employees_id = c.employee_id
           JOIN OfficeBranches b ON e.branch_id = b.branch_id;";

            var result = await _context.ExecuteSqlQueryAsync(sql);
            return Ok(result);

        }
    }
}
