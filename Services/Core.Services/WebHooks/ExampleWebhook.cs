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
            // REQUEST BODY:{"isSuccess":"True", "Id":"1234","Type":"Example"}
            //--------------------------------------

            // Unpack and process the ExampleWebhookEvent...
            Log.Information("Example webhook called {@exampleWebhookEvent}.", exampleWebhookEvent);


            //Return our response...
            if(exampleWebhookEvent.isSuccess)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
            
        }
    }

    public class ExampleWebhookEvent
    {
        public bool isSuccess { get; set; }
        public int Id { get; set; }
        public string Type { get; set; }
    }
}
