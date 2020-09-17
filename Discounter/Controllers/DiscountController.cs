using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            int[] discount = new int[deserializedProducts.Count()];
            if (coin)
            {
                for (int i = 0; i < discount.Length; i++)
                {
                    discount[i] = 25;
                }
            }
            else
            {
                for (int i = 0; i < discount.Length; i++)
                {
                    discount[i] = 125;
                }
            }
            coin = !coin;
            return JsonConvert.SerializeObject(discount);
        }
    }
}
