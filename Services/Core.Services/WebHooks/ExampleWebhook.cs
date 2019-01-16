using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Services.WebHooks
{
    [ApiExplorerSettings(IgnoreApi = true)] //<-- We do not publish our Webhooks to our OpenAPI/Swagger documentation
    [Route("webhooks/example")]
    [ApiController]
    public class ExampleWebhook : ControllerBase
    {
        [HttpPost]
        public ActionResult Post(ExampleWebhookEvent exampleWebhookEvent)
        {
            //------------------------------------
            // Sample POST call: /webhooks/example
            //------------------------------------
            // Content-Type: application/json
            // REQUEST BODY:{"Requester":"CustodialProcessor", "Id":"123456","Type":"CloseInactiveAccounts"}
            //--------------------------------------

            // Unpack and process the ExampleWebhookEvent...
            Log.Information("Webhook called {@exampleWebhookEvent}.", exampleWebhookEvent);


            //Return our response...
            return Ok();
        }
    }

    public class ExampleWebhookEvent
    {
        public string Requester { get; set; }
        public int Id { get; set; }
        public string Action { get; set; }
    }
}
