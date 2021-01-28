using InvoiceAssembler.Model;
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
            var language = Environment.GetEnvironmentVariable("INVOICE_LANGUAGE");
            if(language != null && language.ToLower() == "python")
            {
                apiBaseUrl = Environment.GetEnvironmentVariable("PYTHON_INVOICE_ADDRESS");
            }
            else if(language != null && language.ToLower() == "nodejs")
            {
                apiBaseUrl = Environment.GetEnvironmentVariable("NODEJS_INVOICE_ADDRESS");

            }
            else
            {
                apiBaseUrl = Environment.GetEnvironmentVariable("JAVA_INVOICE_ADDRESS");
            }

            if (string.IsNullOrEmpty(apiBase
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

            var discounts = JsonConvert.DeserializeObject<Discounts>(discountsString);
            var order = db.Orders.Include(x => x.Customer).Include(x => x.OrderDetails).ThenInclude(x => x.Product).ThenInclude(x => x.Category).FirstOrDefault(x => x.OrderId == id);
            if (order.OrderDetails.Count != discounts.Values.Count)
            {
                return Problem("Discounts count does not match ordered products count");
            }

            foreach (var productDetail in order.OrderDetails)
            {
                productDetail.Discount = discounts.Values.FirstOrDefault(x => x.ProductId == productDetail.ProductId).Percent;
            }

            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/buildInvoice";
                string json = JsonConvert.SerializeObject(order);
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                using var Response = await client.PostAsync(endpoint, httpContent);
                var statusCode = (int)Response.StatusCode;
                return StatusCode(statusCode);
            }
        }
    }
}
