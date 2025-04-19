//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MyDbApp.Data;
//using MySql.Data.MySqlClient;
//using System.Numerics;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using MyDbApp.Data;
using MyDbApp.Models;
using MySqlConnector;
namespace MyDbApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OfficeBranchesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public OfficeBranchesController(ApplicationDbContext context) { _context = context; }

        [HttpGet("GetOfficeBranches")] //read
        public async Task<ActionResult<IEnumerable<dynamic>>> GetOfficeBranches()
        {
            var sql = "SELECT * FROM OfficeBranches";
            var officeBranches = await _context.ExecuteSqlQueryAsync(sql);
            return Ok(officeBranches);
        }

        [HttpGet("{id}")] //read
        public async Task<ActionResult<dynamic>> GetofficeBranchesbyid(int id)
        {
            var sql = "SELECT * FROM officeBranches WHERE branch_id = @id";
            var parameters = new[] { new MySqlParameter("@id", id) };
            var officeBranches = await _context.ExecuteSqlQueryAsync(sql, parameters);
            if (officeBranches == null || !officeBranches.Any())
            {
                return NotFound();
            }

            return Ok(officeBranches.First());
        }
        [HttpDelete("{id}")]  //delete
        public async Task<ActionResult<dynamic>> DeleteofficeBranches(int id)
        {
            var sql = "DELETE FROM officeBranches WHERE branch_id = @id";
            var parameters = new[] {new MySqlParameter("@id", id) };
            await _context.ExecuteSqlQueryAsync(sql, parameters);
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult<dynamic>> CreateOfficeBranches([FromBody] OfficeBranchesData officeBranchesData)
        {
            var sql = "INSERT INTO OfficeBranches (country_name, branch_founder_name,  branch_name) VALUES (@countryname, @foundername, @branchname); SELECT LAST_INSERT_ID()"; //Create
            var parameters = new[]
            {
                new MySqlParameter("@countryname", officeBranchesData.country_name),
                new MySqlParameter("@foundername", officeBranchesData.branch_founder_name),
                new MySqlParameter("@branchname", officeBranchesData.branch_name)

            };
            var id = await _context.ExecuteSqlQueryAsync(sql, parameters);
            return CreatedAtAction(nameof(GetOfficeBranches), new { id = id }, officeBranchesData);

        }
        [HttpPatch]
        public async Task<ActionResult<dynamic>> UpdateOfficeBranches([FromBody] OfficeBranchesData officeBranchesData) //Update
        {
            var sql = "UPDATE OfficeBranches SET country_name = @countryname, branch_founder_name = @foundername, branch_name = @branchname WHERE branch_id = @id";
            var parameters = new[]
            {
                new MySqlParameter("@countryname", officeBranchesData.country_name),
                new MySqlParameter("@foundername", officeBranchesData.branch_founder_name),
                new MySqlParameter("@branchname", officeBranchesData.branch_name),
                new MySqlParameter("@id", officeBranchesData.branch_id)
            };
            var id = await _context.ExecuteSqlQueryAsync(sql, parameters);
            return CreatedAtAction(nameof(GetOfficeBranches), new { id = officeBranchesData.branch_id }, officeBranchesData);

        }
        //Need to define some sql joints
    }
}
