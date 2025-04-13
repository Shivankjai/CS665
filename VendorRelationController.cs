
using Microsoft.AspNetCore.Mvc;
using MyDbApp.Data;
using MyDbApp.Models;
using MySqlConnector;
using static System.Runtime.InteropServices.JavaScript.JSType;
//ing static System.Runtime.InteropServices.JavaScript.JSType;
namespace MyDbApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorRelationController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public VendorRelationController(ApplicationDbContext context) { _context = context; }
        [HttpGet("GetVendorRelation")]                                                            //read
        public async Task<ActionResult<IEnumerable<dynamic>>> GetVendorRelation()
        {
            var sql = "SELECT * FROM VendorRelation";
            var relations = await _context.ExecuteSqlQueryAsync(sql);
            return Ok(relations);
        }
        [HttpDelete]
        public async Task<ActionResult<dynamic>> DeleteVendorRelation([FromBody] VendorRelationData vendorRelationData)
        {
            var sql = "DELETE FROM VendorRelation WHERE branch_id = @branchId AND vendor_id = @vendorId";                    //delete
            var parameters = new[]
            {
                new MySqlParameter("@branchId", vendorRelationData.branch_id),
                new MySqlParameter("@vendorId", vendorRelationData.vendor_id)
            };
            await _context.ExecuteSqlQueryAsync(sql, parameters);
            return Ok();
        }
        [HttpPost]
        public async Task<ActionResult> CreateVendorRelation([FromBody] VendorRelationData vendorRelationData) //post
        {
            // Check if both IDs exist
            var branchExists = await _context.ExecuteSqlQueryAsync(
                "SELECT 1 FROM OfficeBranches WHERE branch_id = @id",
                 new MySqlParameter("@id", vendorRelationData.branch_id));

            var vendorExists = await _context.ExecuteSqlQueryAsync(
                "SELECT 1 FROM Vendor WHERE vendor_id = @id",
                new MySqlParameter("@id", vendorRelationData.vendor_id));

            if (!branchExists.Any())
                return BadRequest("Invalid branch_id");

            if (!vendorExists.Any())
                return BadRequest("Invalid vendor_id");
            // Insert 
            var sql = "INSERT INTO VendorRelation (branch_id, vendor_id) VALUES (@branchId, @vendorId)";
            var parameters = new[]
            {
                new MySqlParameter("@branchId", vendorRelationData.branch_id),
                new MySqlParameter("@vendorId", vendorRelationData.vendor_id)
            };

            await _context.ExecuteSqlQueryAsync(sql, parameters);
            return Ok("Relation added");

        }


    }

}
