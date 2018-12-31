using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Core.Services.WebHooks
{
    [Route("webhooks/example")]
    [ApiController]
    public class ExampleWebhook : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            return Ok();
        }

        [HttpPost]
        public ActionResult Post(ExampleWebhookEvent exampleWebhookEvent)
        {
            //------------------------------------
            // Sample POST call:
            //------------------------------------
            // Content-Type: application/json
            // REQUEST BODY:{"Id":"1234","Type":"Example"}
            //--------------------------------------

            // Unpack and process the ExampleWebhookEvent...

            //Return our response...
            return Ok();
        }
    }

    public class ExampleWebhookEvent
    {
        public string Id { get; set; }
        public string Type { get; set; }
    }
}
