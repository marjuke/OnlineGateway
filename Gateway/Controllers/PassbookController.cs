using Application.ThirdPartyChannel;
using Azure.Core;
using Domain.RequestC;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging.Signing;
using SOAP.Model;
using System;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Gateway.Controllers
{
    [Route("DmcbGW/[controller]")]
    [ApiController]
    public class PassbookController : BaseApiController
    {
        //serilog

        private readonly ILogger<PassbookController> _logger;
        public PassbookController(ILogger<PassbookController> logger)
        {
            _logger = logger;
        }

        // GET: api/<PassbookController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PassbookController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<PassbookController>
        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ReqPassbook value)
        {
            //ReqPassbook myObject = new ReqPassbook();
            _logger.LogInformation("PassbookController PostAsync called");
            _logger.LogInformation("PassbookController PostAsync called with value: {@value}", value);
            var data = await Mediator.Send(new TransactionList.Query { reqPassbook = value });
            _logger.LogInformation("PassbookController PostAsync called with data: {@data}", data);
            return Ok(data);
        }

        // PUT api/<PassbookController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PassbookController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }

    //public class Passbook
    //{
    //    public int Version { get; set; }
        
    //    public string CommandID { get; set; }
    //    public string OriginatorConversationID { get; set; }
    //    public DateTime Timestamp { get; set; }
    //    public string PartnerID { get; set; }
    //    public string ParentID { get; set; }
    //    public string InitiatorID { get; set; }
    //    public string AccountID { get; set; }
    //    public string StartMonth { get; set; }
    //    public string EndMonth { get; set; }
    //}
}
