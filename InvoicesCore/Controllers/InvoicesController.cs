using InvoiceAssembler;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Invoices.Controllers
{
    public class InvoicesController : Controller
    {
        string assemblerUrl;
        string discounterUrl;
        public InvoicesController(IConfiguration configuration)
        {
            assemblerUrl = Environment.GetEnvironmentVariable("ASSEMBLER_ADDRESS");
            if (string.IsNullOrEmpty(assemblerUrl))
            {
                assemblerUrl = configuration.GetValue<string>("WebAPIBaseUrl");
            }
            discounterUrl = Environment.GetEnvironmentVariable("DISCOUNTER_ADDRESS");
            if (string.IsNullOrEmpty(discounterUrl))
            {
                discounterUrl = configuration.GetValue<string>("DiscountWebAPIBaseUrl");
            }
        }

        // GET: Invoices
        public async Task<ActionResult> Index()
        {
            using (HttpClient client = new HttpClient())
            {
                string endpoint = assemblerUrl + "/Assembler/Get";

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
                string endpoint = assemblerUrl + "/Assembler/Details?id=" + id;

                using (var Response = await client.GetAsync(endpoint))
                {
                    var stringResponse = await Response.Content.ReadAsStringAsync();
                    Orders order = JsonConvert.DeserializeObject<Orders>(stringResponse);
                    return View(order);
                }
            }
        }

        public async Task<ActionResult> Reconcile(string id)
        {
            Orders order = null;
            using (HttpClient client = new HttpClient())
            {
                string endpoint = assemblerUrl + "/Assembler/Details?id=" + id;

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
                string endpoint = discounterUrl + "/Discount/Get?products=" + serialzedProducts + "&&customerId=" + order.CustomerId;
                using (var Response = await client.GetAsync(endpoint))
                {
                    discounts = await Response.Content.ReadAsStringAsync();
                }
            }

            using (HttpClient client = new HttpClient())
            {
                string endpoint = assemblerUrl + "/Assembler/PrepareInvoiceData?id=" + id + "&discountsString=" + discounts;

                using (var Response = await client.GetAsync(endpoint))
                {
                    var stringResponse = await Response.Content.ReadAsStringAsync();
                    order = JsonConvert.DeserializeObject<Orders>(stringResponse);
                }
            }

            return RedirectToAction("Details", new { id = id });
        }
    }
}
