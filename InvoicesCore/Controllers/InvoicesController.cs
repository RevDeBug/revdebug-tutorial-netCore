using InvoiceAssembler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Invoices.Controllers
{
    public class InvoicesController : Controller
    {
        string apiBaseUrl;
        string discountApiBaseUrl;
        public InvoicesController(IConfiguration configuration)
        {
            apiBaseUrl = configuration.GetValue<string>("WebAPIBaseUrl");
            discountApiBaseUrl = configuration.GetValue<string>("DiscountWebAPIBaseUrl");
        }

        // GET: Invoices
        public async Task<ActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Assembler/Get";

                using (var Response = await client.GetAsync(endpoint))
                {
                    var stringResponse = await Response.Content.ReadAsStringAsync();
                    List<Orders> orders = JsonConvert.DeserializeObject<List<Orders>>(stringResponse);
                    return View(orders);
                }
            }
        }

        // GET: Invoices/Details/5
        public async Task<ActionResult> Details(string id)
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Assembler/Details?id=" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    var stringResponse = await Response.Content.ReadAsStringAsync();
                    Orders order = JsonConvert.DeserializeObject<Orders>(stringResponse);
                    return View(order);
                }
            }
        }

        public async Task<ActionResult> Reconsile(string id)
        {
            Orders order = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Assembler/Details?id=" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    var stringResponse = await Response.Content.ReadAsStringAsync();
                    order = JsonConvert.DeserializeObject<Orders>(stringResponse);
                }
            }

            short[] products = order.OrderDetails.Select(x => x.ProductId).ToArray();
            string serialzedProducts = JsonConvert.SerializeObject(products);
            string discounts;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = discountApiBaseUrl + "/Discount/Get?products=" + serialzedProducts;
                using (var Response = await client.GetAsync(endpoint))
                {
                    discounts = await Response.Content.ReadAsStringAsync();
                }
            }

            using (HttpClient client = new HttpClient())
            {
                string endpoint = apiBaseUrl + "/Assembler/PrepareInvoiceData?id=" + id + "&discountsString=" + discounts;

                using (var Response = await client.GetAsync(endpoint))
                {
                    var stringResponse = await Response.Content.ReadAsStringAsync();
                    order = JsonConvert.DeserializeObject<Orders>(stringResponse);
                }
            }

            Program.ProcessDiscounts(discounts);

            return RedirectToAction("Details", new { id = id });
        }
    }
}
