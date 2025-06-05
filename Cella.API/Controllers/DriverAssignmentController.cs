using Cella.Infrastructure;
using Cella.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Cella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverAssignmentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public DriverAssignmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Assign driver to route based on customer postcode
        [HttpPost("assign-driver")]
        public IActionResult AssignDriverToRoute([FromBody] AssignDriverRequest request)
        {
            var order = _context.Set<SalesOrder>().FirstOrDefault(o => o.Id == request.OrderId);
            var customer = _context.Customers.FirstOrDefault(c => c.Id == order.Customer);
            var postcode = customer.Addresses.FirstOrDefault()?.PostCode;

            var driver = _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefault(d =>
                    d.CustomerType == Customer.TypeOfCustomer.Driver &&
                    d.Addresses.Any(a => a.PostCode == postcode));
            if (driver == null)
                return NotFound("No driver found for this postcode");
            // Here you would update the order/route assignment as needed
            return Ok(new { DriverId = driver.Id, DriverName = driver.FirstName + " " + driver.LastName });
        }
        public class AssignDriverRequest
        {
            public int OrderId { get; set; }
        }
    }
}
