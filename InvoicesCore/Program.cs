using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace InvoicesCore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).UseUrls("http://localhost:5400/").Build().Run();
        }

        #region NoTimeTravel
        public static void ProcessDiscounts(string discounts)
        {
            if (JsonConvert.DeserializeObject<int[]>(discounts)[0] > 100)
            {
                RevDeBugAPI.Snapshot.RecordSnapshot("discountSnapshot");
            }
        }
        #endregion

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
