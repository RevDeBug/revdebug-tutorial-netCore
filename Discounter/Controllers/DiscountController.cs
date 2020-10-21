using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using InvoiceAssembler.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Discounter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiscountController : ControllerBase
    {
        private static bool coin = false;
        public DiscountController()
        {
        }

        [HttpGet("Get")]
        public string Get(string products)
        {
            var deserializedProducts = JsonConvert.DeserializeObject<short[]>(products);
            Discounts discounts = new Discounts();
            if (coin)
            {
                for (int i = 0; i < deserializedProducts.Length; i++)
                {
                    discounts.Values.Add(new Discounts.Discount(deserializedProducts[i], 25));
                }
            }
            else
            {
                Random random = new Random();
                for (int i = 0; i < deserializedProducts.Length; i++)
                {
                    discounts.Values.Add(new Discounts.Discount(deserializedProducts[i], random.Next(108, 150)));
                }
            }
            coin = !coin;
            return JsonConvert.SerializeObject(discounts);
        }
    }
}
