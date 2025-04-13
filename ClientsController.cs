
using Microsoft.AspNetCore.Mvc;
using MyDbApp.Data;
using MyDbApp.Models;
using MySqlConnector;
namespace MyDbApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public ClientsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetClients")] //read
        public async Task<ActionResult<IEnumerable<dynamic>>> GetClients()
        {
            var sql = "SELECT * FROM Clients";
            var Clients = await _context.ExecuteSqlQueryAsync(sql);
            return Ok(Clients);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<dynamic>> GetClientsbyid(int id)
        {
            var sql = "SELECT * FROM Clients WHERE client_id = @id";
            var parameters = new[] { new MySqlParameter("@id", id) };
            var clients = await _context.ExecuteSqlQueryAsync(sql, parameters);
            if (clients == null || !clients.Any())
            {
                return NotFound();
            }
            return Ok(clients);

        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> DeleteClients(int id)
        {
            var sql = "DELETE FROM Clients WHERE client_id = @id";
            var parameters = new[] { new MySqlParameter("@id", id) };
            await _context.ExecuteSqlQueryAsync(sql, parameters);
            return Ok();

        }
        [HttpPost]
        public async Task<ActionResult<dynamic>> CreateClients([FromBody] ClientsData clientsData)
        {
            var sql = "INSERT INTO Clients(company_name, client_email_id, employee_id) VALUES (@name, @emailid, @employeeid);  SELECT LAST_INSERT_ID() ";
            var parameters = new[]
            {
                new MySqlParameter("@name", clientsData.company_name),
                new MySqlParameter("@emailid", clientsData.client_email_id),
                new MySqlParameter("employeeid", clientsData.employee_id)
            };
            var id = await _context.ExecuteSqlQueryAsync(sql, parameters);
            return CreatedAtAction(nameof(GetClients), new { id = id }, clientsData);
        }

        [HttpPatch]
        public async Task<ActionResult<dynamic>> UpdateClients([FromBody] ClientsData clientsData)
        {
            var sql = "UPDATE Clients SET company_name = @name, client_email_id = @emailid, employee_id = @employeeid WHERE client_id = @id";
            var parameters = new[]
           {
                new MySqlParameter("@name", clientsData.company_name),
                new MySqlParameter("@emailid", clientsData.client_email_id),
                new MySqlParameter("employeeid", clientsData.employee_id),
                new MySqlParameter("@id", clientsData.Id)
            };
            var id = await _context.ExecuteSqlQueryAsync(sql, parameters);
            return CreatedAtAction(nameof(GetClients), new { id = id }, clientsData);

        }
        // Need to define some sql joints

    }
}
