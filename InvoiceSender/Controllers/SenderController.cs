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
        public SenderController()
        {
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
