using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace InvoiceAssembler
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
#if !DEBUG
            var connstring = Configuration.GetConnectionString("DefaultConnection");
            var server = Environment.GetEnvironmentVariable("LS_PSQL_NODE_PORT_5432_TCP_ADDR");
            var port = Environment.GetEnvironmentVariable("LS_PSQL_NODE_PORT_5432_TCP_PORT");
            var user = Environment.GetEnvironmentVariable("LS_PSQL_NODE_ENV_POSTGRES_USER");
            var password = Environment.GetEnvironmentVariable("LS_PSQL_NODE_ENV_POSTGRES_PASSWORD");

            connstring = connstring.Replace("$Server", server).Replace("$Port", port).Replace("$User", user).Replace("$Password", password);
#else
            var connstring = Configuration.GetConnectionString("DebugConnection");
#endif

            services.AddControllers();

            services.AddDbContext<northwindContext>(options => options.UseNpgsql(connstring));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, northwindContext context)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            context.RecreateDatabase();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
