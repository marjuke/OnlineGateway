using Domain.RequestC;
using Domain.ResponseC;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Persistence;

namespace Application.ThirdPartyChannel
{
    public class TransactionList
    {
        public class Query : IRequest<responsePassbook>
        {
            public required ReqPassbook reqPassbook { get; set; }
        }
        public class Handler : IRequestHandler<Query, responsePassbook>
        {
            private readonly DataContext _context;
            private readonly ILogger<TransactionList> _logger;
            public Handler(DataContext context, ILogger<TransactionList> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<responsePassbook> Handle(Query request, CancellationToken cancellationToken)
            {
                responsePassbook responseObj = new();
                //char ch = '-';
                
                if (string.IsNullOrEmpty(request.reqPassbook.AccountID))
                {
                    _logger.LogError("Handling command {Passbook}", "Account Id ("+ request.reqPassbook.AccountID + ") validatation issue");
                    responseObj.resultCode = "BECP002";
                    responseObj.resultDesc = "Invalid Request";
                    responseObj.initiatorID = request.reqPassbook.InitiatorID;
                    responseObj.accountID = request.reqPassbook.AccountID;
                    responseObj.subID = null;
                    //var data = responseObj.ToString();
                    //return JsonSerializer.Serialize<responsePassbook>(responseObj);
                    return responseObj;
                }
                var branchCode = request.reqPassbook.AccountID.Substring(0, 3);
                var ip = _context.RoutingServerList.Where(s => s.BranchCode == branchCode).FirstOrDefault();
                if (ip==null)
                {
                    _logger.LogError("Handling command {CheckInformation}", "IP not found");

                    responseObj.resultCode = "BECP005";
                    responseObj.resultDesc = "Sorry, an error occurred";
                    responseObj.initiatorID = request.reqPassbook.InitiatorID;
                    responseObj.accountID = request.reqPassbook.AccountID;
                    responseObj.subID = null;
                    //return JsonSerializer.Serialize<responsePassbook>(responseObj);
                    return responseObj;
                }
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://" + ip.Ip + "/");

                HttpResponseMessage response = await client.GetAsync($"api/MemberPassbookSummery?AccountID={request.reqPassbook.AccountID}&startmonth={request.reqPassbook.StartMonth}&endmonth={request.reqPassbook.EndMonth}&initiatorID={request.reqPassbook.InitiatorID}");


                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var d = JsonSerializer.Deserialize<responsePassbook>(data);
                    return d;
                }
                else
                {
                    _logger.LogError("Handling command {CheckInformation}", "Error from FinApp");
                    
                    responseObj.resultCode = "BECP005";
                    responseObj.resultDesc = "Sorry, an error occurred";
                    responseObj.initiatorID = request.reqPassbook.InitiatorID;
                    responseObj.accountID = request.reqPassbook.AccountID;
                    responseObj.subID = null;
                    //return JsonSerializer.Serialize<responsePassbook>(responseObj);
                    return responseObj;
                }

            }
        }
    }
    //public class responsePassbook
    //{
    //    public string resultCode { get; set; }
    //    public string resultDesc { get; set; }
    //    public string initiatorID { get; set; }
    //    public string accountID { get; set; }
    //    public string subID { get; set; }
    //    public string[] passBookData { get; set; } = [];
    //}   
}
