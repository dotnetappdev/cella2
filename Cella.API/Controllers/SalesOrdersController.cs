using Cella.Infrastructure;
using Cella.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalesOrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SalesOrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<SalesOrder>> GetOrders()
        {
            return _context.Set<SalesOrder>()
                .Include(o => o.Lines)
                .ToList();
        }

        [HttpGet("customers")]
        public ActionResult<IEnumerable<Customer>> GetCustomers()
        {
            return _context.Set<Customer>().ToList();
        }

        [HttpGet("billofmaterials")]
        public ActionResult<IEnumerable<BillOfMaterials>> GetBillOfMaterials()
        {
            return _context.Set<BillOfMaterials>().Include(b => b.Items).ToList();
        }

        [HttpPost]
        public IActionResult AddOrder([FromBody] SalesOrder order)
        {
            //if (order.Lines != null)
            //{
            //    foreach (var line in order.Lines)
            //    {
            //        if (line.BillOfMaterialsId.HasValue)
            //        {
            //            line.BillOfMaterials = _context.Set<BillOfMaterials>().Include(b => b.Items).FirstOrDefault(b => b.Id == line.BillOfMaterialsId);
            //        }
            //    }
            //}
            _context.Set<SalesOrder>().Add(order);
            _context.SaveChanges();
            return Ok(order);
        }

        [HttpGet("{id}/pdf")]
        public IActionResult GetOrderPdf(int id)
        {
            var order = _context.Set<SalesOrder>().FirstOrDefault(o => o.Id == id);
            if (order == null) return NotFound();
            var stream = new MemoryStream();
            Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.Header().Text($"Sales Order #{order.Id}").FontSize(20).Bold();
                    page.Content().Column(col =>
                    {
                        col.Item().Text($"Order Date: {order.CreatedOn:yyyy-MM-dd}");
                        col.Item().Text($"Order Total: {order.OrderTotal:C}");
                        col.Item().Text($"Customer: {order.Customer}");
                        // Add more order details as needed
                    });
                });
            }).GeneratePdf(stream);
            stream.Position = 0;
            return File(stream.ToArray(), "application/pdf", $"Order_{id}.pdf");
        }
    }
}
