using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InvoiceAssembler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AssemblerController : ControllerBase
    {

        private northwindContext db;
        string apiBaseUrl;

        public AssemblerController(northwindContext invoicesContext, IConfiguration configuration)
        {
            apiBaseUrl = Environment.GetEnvironmentVariable("JAVA_INVOICE_ADDRESS");
            if (string.IsNullOrEmpty(apiBaseUrl))
            {
                apiBaseUrl = configuration.GetValue<string>("WebAPIBaseUrl");
            }
            db = invoicesContext;
        }

        [HttpGet("Get")]
        public IEnumerable<Orders> Get()
        {
            return db.Orders.Include(x => x.Customer).Include(x => x.OrderDetails).ThenInclude(x => x.Product).Take(10);
        }

        [HttpGet("Details")]
        public Orders Details(int id)
        {
            var order = db.Orders.Include(x => x.OrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.Category).FirstOrDefault(x => x.OrderId == id);
            return order;
        }

        [HttpGet("PrepareInvoiceData")]
        public async Task<IActionResult> PrepareInvoiceData(int id, string discountsString)
        {

            var discounts = JsonConvert.DeserializeObject<int[]>(discountsString);
            var order = db.Orders.Include(x => x.Customer).Include(x => x.OrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.Category).FirstOrDefault(x => x.OrderId == id);
            if (order.OrderDetails.Count != discounts.Length)
            {
                return Problem("Discounts count does not match ordered products count");
            }

            var i = 0;
            foreach (var productDetail in order.OrderDetails)
            {
                productDetail.Discount = discounts[i];
                i = i + 1;
            }


            Program.ProcessDiscounts(discountsString);

            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/buildInvoice";
                string json = JsonConvert.SerializeObject(order);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                using var Response = await client.PostAsync(endpoint, httpContent);
                return StatusCode((int)Response.StatusCode);
            }
        }
    }
}
