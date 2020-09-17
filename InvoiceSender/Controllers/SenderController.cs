using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace InvoiceSender.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SenderController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<SenderController> _logger;

        public SenderController(ILogger<SenderController> logger)
        {
            _logger = logger;
        }

        [HttpGet("Send")]
        public ActionResult Send(string invoice)
        {
            if (string.IsNullOrEmpty(invoice))
            {
                throw new Exception("Empty Invoice!");
            }
            Program.SendInvoiceToRecipent(invoice);
            return Ok();
        }
    }
}
