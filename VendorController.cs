using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.SqlClient;
using MyDbApp.Data;
using MyDbApp.Models;
using MySqlConnector;



namespace MyDbApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public VendorController(ApplicationDbContext context) { _context = context; }  //dpendency injection

        [HttpGet("GetVendor")]   //read
        public async Task<ActionResult<IEnumerable<dynamic>>> GetVendor()
        {
            var sql = "SELECT * FROM Vendor";
            var vendors = await _context.ExecuteSqlQueryAsync(sql);
            return Ok(vendors);
        }
        // GET: api/Vendor/5
        [HttpGet("{id}")]  //read
        public async Task<ActionResult<dynamic>> GetVendorbyid(int id)
        {
            var sql = "SELECT * FROM Vendor WHERE vendor_id = @id";   
            var parameters = new[] { new MySqlParameter("@id", id) };
            var vendor = await _context.ExecuteSqlQueryAsync(sql, parameters);

            if (vendor == null || !vendor.Any())
            {
                return NotFound();
            }

            return Ok(vendor.First());
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<dynamic>> DeleteVendor(int id)
        { 
            var sql = "DELETE FROM Vendor WHERE vendor_id = @id";   //delete
            var parameters = new[] { new MySqlParameter("@id", id) };
            await _context.ExecuteSqlQueryAsync(sql, parameters);
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<dynamic>> CreateVendor([FromBody] VendorData vendorData)
        {
            var sql = "INSERT INTO Vendor (vendor_name, vendor_product_type) VALUES (@name, @productType); SELECT LAST_INSERT_ID()";  //create
            var parameters = new[]
            {
                new MySqlParameter("@name", vendorData.vendor_name),
                new MySqlParameter("@productType", vendorData.vendor_product_type)
            };
            var id = await _context.ExecuteSqlQueryAsync(sql, parameters);
            return CreatedAtAction(nameof(GetVendor), new { id = id }, vendorData);
        }

        [HttpPatch]

        public async Task<ActionResult<dynamic>> UpdateVendor([FromBody] VendorData vendorData)  //update
        {
            var sql = "UPDATE Vendor SET vendor_name = @name, vendor_product_type = @productType WHERE vendor_id = @id";
            var parameters = new[]
            {
                new MySqlParameter("@name", vendorData.vendor_name),
                new MySqlParameter("@productType", vendorData.vendor_product_type),
                new MySqlParameter("@id", vendorData.vendor_id)

            };
            var id = await _context.ExecuteSqlQueryAsync(sql, parameters);
            return CreatedAtAction(nameof(GetVendor), new { id = vendorData.vendor_id}, vendorData);

        }

       //Need to define some sql joints
    }

}
