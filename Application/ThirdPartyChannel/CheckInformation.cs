using Domain.DatabaseC;
using Domain.RequestC;
using Domain.ResponseC;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Persistence;

namespace Application.ThirdPartyChannel
{
    public class CheckInformation
    {
        //private readonly ILogger<CheckInformation> _logger;

        //public CheckInformation(ILogger<CheckInformation> logger)
        //{
        //    _logger = logger;
        //}
        public class Query : IRequest<bKashOuterResponseClass>
        {
            public required string AccountID { get; set; }
            public string InitiatorId { get; set; }
            public string RequesterMSISDN { get; set; }
            public string Timestamp { get; set; }
            public string Version { get; set; }
            public string ConversationID { get; set; }
            public string Command { get; set; }
            public string LoginID { get; set; }
            public string Password { get; set; }
            public required ReqCheckInformation reqCheckInformation { get; set; }
        }
        public class Handler : IRequestHandler<Query, bKashOuterResponseClass>
        {
            private readonly DataContext _context;
            private readonly ILogger<CheckInformation> _logger;
            //private readonly ILogger _logger;
            public Handler(DataContext context, ILogger<CheckInformation> logger/*, ILogger logger*/)
            {
                _context = context;
                _logger = logger;
                // _logger = logger;
            }

            public async Task<bKashOuterResponseClass> Handle(Query request, CancellationToken cancellationToken)
            {
                //var data = new Parameter[0];
                _logger.LogInformation("Check Information handle called");
                var responseObj = new bKashOuterResponseClass();
                try
                {
                    // AccountID checking is mandatory
                    if (string.IsNullOrEmpty(request.AccountID))
                    {
                        
                        _logger.LogWarning("Handling command {CheckInformation}", "Member ID not found");
                        responseObj = new bKashOuterResponseClass
                        {
                            ResponseCode = "BEC0001",
                            ResponseDesc = "Member ID does not exist. Please try again.",
                            Parameters = []
                        };
                        return responseObj;
                    }
                    // Get the value of I(InitiatorId checking) from the setting
                    var settingValue = Convert.ToInt16(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["I"]);
                    if (settingValue == 0)
                    {
                        //If the value of I is true InitiatorId checking is mandatory
                        if (string.IsNullOrEmpty(request.InitiatorId))
                        {
                            _logger.LogWarning("Handling command {CheckInformation}", "InitiatorId id not found");
                            responseObj = new bKashOuterResponseClass
                            {
                                ResponseCode = "BEC0001",
                                ResponseDesc = "Initiator ID does not exist. Please try again.",
                                Parameters = []
                            };
                            return responseObj;
                        }
                    }

                    // channelCode checking is mandatory
                    if (string.IsNullOrEmpty(request.reqCheckInformation.channelCode))
                    {
                        _logger.LogWarning("Handling command {CheckInformation}", "channelCode id not found");
                        responseObj = new bKashOuterResponseClass
                        {
                            ResponseCode = "BEC0001",
                            ResponseDesc = "ChannelCode does not exist. Please try again.",
                            Parameters = []
                        };
                        return responseObj;
                    }
                    if(!_context.ChannelCode.Any(s => s.Code == request.reqCheckInformation.channelCode&& s.channelId == 1&& s.IsActive == true))
                    {
                        _logger.LogWarning("Handling command {CheckInformation}", "channelCode not found");
                        responseObj = new bKashOuterResponseClass
                        {
                            ResponseCode = "BEC0001",
                            ResponseDesc = "ChannelCode not permitted. Please try again.",
                            Parameters = []
                        };
                        return responseObj;
                    }
                    //if (string.IsNullOrEmpty(request.reqCheckInformation.RequesterMSISDN))
                    //{
                    //    _logger.LogWarning("Handling command {CheckInformation}", "RequesterMSISDN id not found");
                    //    responseObj = new bKashOuterResponseClass
                    //    {
                    //        ResponseCode = "BEC0001",
                    //        ResponseDesc = "RequesterMSISDN does not exist. Please try again.",
                    //        Parameters = []
                    //    };
                    //    return responseObj;
                    //}
                    _logger.LogInformation("Check Information handle called "+ request.ToString());
                    GatewayCheckInfo data = new GatewayCheckInfo() { InitiatorID = "", BranchCode = "" };
                    data.InitiatorID = request.reqCheckInformation.InitiatorID!=null?request.reqCheckInformation.InitiatorID:"";
                    data.AccountID = request.AccountID;
                    data.ChannelReqDateTime = DateTime.ParseExact(request.Timestamp, "yyyyMMddHHmmss", null);
                    data.ServerReqDate = DateTime.Now.Date;
                    data.ActualAmount = "0";
                    data.BranchCode = request.AccountID.Substring(0, 3);
                    data.Version = request.Version;
                    data.ConversationID = request.ConversationID;
                    data.LoginID = request.LoginID;
                    data.Password = request.Password;
                    data.Command = request.Command;
                    data.ChannelCode = request.reqCheckInformation.channelCode;
                    data.SysChannelReqDateTime = DateTime.Now;
                    data.SysSpReqDateTime = DateTime.Now;
                    data.Timestamp = data.ChannelReqDateTime;
                    data.RequesterMSISDN = request.RequesterMSISDN;
                    data.InquiryType = string.IsNullOrEmpty(request.reqCheckInformation.InquiryType)?null: Convert.ToInt32( request.reqCheckInformation.InquiryType);
                    var ip = _context.RoutingServerList.Where(s => s.BranchCode == data.BranchCode).FirstOrDefault();
                    _logger.LogInformation("Check Information IP " + ip.ToString());
                    if (ip == null)
                    {
                        _logger.LogError("Handling command {CheckInformation}", "IP not found");
                        responseObj.ResponseCode = "BECP005";
                        responseObj.ResponseDesc = "Sorry, an error occurred";
                        responseObj.Parameters = [];
                        return responseObj;
                    }
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://" + ip.Ip + "/");

                    //HttpResponseMessage response = await client.GetAsync($"api/OtherGWCheckby/MemberIDNO?CustIDNO={request.AccountID}");
                    //HttpResponseMessage response = await client.GetAsync($"api/OtherGWCheckby/MemberIDNO?CustIDNO={request.AccountID}&MobileNo={request.InitiatorId}");
                    HttpResponseMessage response = await client.GetAsync($"api/OtherGWCheckby/MemberIDNO?CustIDNO={request.AccountID}&MobileNo={request.InitiatorId}&walletNumber={request.reqCheckInformation.PartnerID}&channelcode={request.reqCheckInformation.channelCode}");
                    _logger.LogWarning("Check Information response status from FinApp " + response.StatusCode);
                    


                    if (response.IsSuccessStatusCode)
                    {
                        data.ResCode = response.StatusCode.ToString();
                        data.SysSpResDateTime = DateTime.Now;
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var d = JsonConvert.DeserializeObject(responseBody);
                        if (d is (object)"1" or (object)"2" or (object)"3" or (object)"4")
                        {
                            responseObj = new bKashOuterResponseClass
                            {
                                ResponseCode = "BECP003",
                                ResponseDesc = "Sorry, no data found in your requested time.",
                                Parameters = []
                            };
                            return responseObj;
                        }
                        var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
                        var rootObject = JsonConvert.DeserializeObject<RootObject>(responseBody, dateTimeConverter);
                        _logger.LogInformation("Check Information response data from FinApp " + responseBody);
                        if (rootObject is null)
                        {
                            responseObj = new bKashOuterResponseClass
                            {
                                ResponseCode = "BECP003",
                                ResponseDesc = "Sorry, no data found in your requested time.",
                                Parameters = []
                            };
                        }
                        else
                        {
                            responseObj = new bKashOuterResponseClass
                            {
                                ResponseCode = "0",
                                ResponseDesc = "Success",
                                Parameters = param(request, rootObject)
                            };
                        }
                        
                        data.SysChannelResDateTime = DateTime.Now;
                        _context.GatewayCheckInfo.Add(data);
                        await _context.SaveChangesAsync();
                        _logger.LogInformation("Check Information data saved in database id"+ data.StanId+" "+data.ServerReqDate);

                        return responseObj;
                    }
                    else
                    {
                        data.ResCode = response.StatusCode.ToString();
                        data.ResponseDesc = response.ReasonPhrase;
                        _context.GatewayCheckInfo.Add(data);
                        await _context.SaveChangesAsync();
                        responseObj = new bKashOuterResponseClass
                        {
                            ResponseCode = "BECP005",
                            ResponseDesc = "Sorry, an error occurred",
                            Parameters = []
                        };
                        // Handle the error response
                        return responseObj;
                    }

                    
                }
                catch (HttpRequestException e)
                {
                    _logger.LogError("Check Infromation Error {CheckInformation}", e.Message + "-" + e.InnerException);
                    responseObj = new bKashOuterResponseClass
                    {
                        ResponseCode = "BECP005",
                        ResponseDesc = "Sorry, an error occurred",
                        Parameters = []
                    };
                    // Handle the error response
                    return responseObj;
                }
                
            }
        }
        private static Parameter[] param(Query request, RootObject rootObject)
        {
            var countInvestment = rootObject.Microcreditinfo.Count;
            var countInvestmentGL = rootObject.OtherInvestmentinfo.Count;
            var deposit = rootObject.DepositAccountinfo.Count;
            //var TotalInstallmentAmount = (rootObject.Microcreditinfo.Select(s => s.AmtOfInstment).Cast<decimal?>().Sum() + rootObject.OtherInvestmentinfo.Select(s => s.AmtOfInstment).Cast<decimal?>().Sum() + rootObject.DepositAccountinfo.Select(s => s.DepositInstAmt).Cast<decimal?>().Sum()).ToString();
            var responseObj = new List<Parameter>
            {
                new Parameter { Key = "PartnerID", Value = rootObject.Memberbasicinfo.First().WalletNumber == null ? "" : rootObject.Memberbasicinfo.First().WalletNumber },
                new Parameter { Key = "PayMode", Value = "0" },
                new Parameter { Key = "InitiatorID", Value = request.InitiatorId },
                //new Parameter { Key = "TotalAmount", Value = TotalInstallmentAmount },
                new Parameter { Key = "TotalPaymentPenalty", Value = "0" },
                new Parameter { Key = "AccountID", Value = request.AccountID },
                new Parameter { Key = "CusName", Value = rootObject.Memberbasicinfo.First().CustName == null ? "" : rootObject.Memberbasicinfo.First().CustName },
                new Parameter { Key = "AccountRemarks", Value = rootObject.Memberbasicinfo.First().CustName == null ? "" : rootObject.Memberbasicinfo.First().BranchName },
                new Parameter { Key = "ContactNo", Value = rootObject.Memberbasicinfo.First().Tel == null ? "" : rootObject.Memberbasicinfo.First().Tel },
                //PaymentRecipient not cleared its a mandatory field
                new Parameter { Key = "PaymentRecipient", Value = request.AccountID.Substring(0,3) }
            };
            var count = 1;
            decimal totalAmount = 0;
            var numberOfProduct = Convert.ToInt16(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["NumberOfProduct"]);
            if (countInvestment > 0 && count<= numberOfProduct)
            {
                foreach (var item in rootObject.Microcreditinfo)
                {
                    if (count > numberOfProduct)
                    {
                        break;
                    }
                    totalAmount += item.AmtOfInstment;
                    responseObj.Add(new Parameter { Key = "PartialAmountFlag" + count, Value ="1" });
                    responseObj.Add(new Parameter { Key = "ProductID" + count, Value = item.LoanNo });
                    responseObj.Add(new Parameter { Key = "MandatoryPayment" + count, Value = "0" });
                    responseObj.Add(new Parameter { Key = "ProductName" + count, Value = "Microcredit (" + item.LoanNo+")" });
                    responseObj.Add(new Parameter { Key = "ActualAmount" + count, Value = item.AmtOfInstment.ToString() });
                    responseObj.Add(new Parameter { Key = "RcvblAmount" + count, Value = item.AmtOfInstment.ToString() });
                    responseObj.Add(new Parameter { Key = "PaymentState" + count, Value = "Unpaid" });
                    count++;
                }


            }
            if (countInvestmentGL > 0 && count <= numberOfProduct)
            {

                foreach (var item in rootObject.OtherInvestmentinfo)
                {
                    if (count > numberOfProduct)
                    {
                        break;
                    }
                    totalAmount += item.AmtOfInstment;
                    responseObj.Add(new Parameter { Key = "PartialAmountFlag" + count, Value ="1" });
                    responseObj.Add(new Parameter { Key = "ProductID" + count, Value = item.LoanNo });
                    responseObj.Add(new Parameter { Key = "MandatoryPayment" + count, Value = "0" });
                    responseObj.Add(new Parameter { Key = "ProductName" + count, Value = "Other Investment ("+item.LoanNo+")" });
                    responseObj.Add(new Parameter { Key = "ActualAmount" + count, Value = item.AmtOfInstment.ToString() });
                    responseObj.Add(new Parameter { Key = "RcvblAmount" + count, Value = item.AmtOfInstment.ToString() });
                    responseObj.Add(new Parameter { Key = "PaymentState" + count, Value = "Unpaid" });
                    count++;
                }


            }

            if (deposit > 0 && count <= numberOfProduct)
            {
                foreach (var item in rootObject.DepositAccountinfo)
                {
                    if (count > numberOfProduct)
                    {
                        break;
                    }
                    totalAmount += item.DepositInstAmt;
                    responseObj.Add(new Parameter { Key = "PartialAmountFlag" + count, Value ="1" });
                    responseObj.Add(new Parameter { Key = "ProductID" + count, Value = item.CustAccNo });
                    responseObj.Add(new Parameter { Key = "MandatoryPayment" + count, Value = "0" });
                    responseObj.Add(new Parameter { Key = "ProductName" + count, Value = "Deposits ("+ item.CustAccNo+")" });
                    responseObj.Add(new Parameter { Key = "ActualAmount" + count, Value = item.DepositInstAmt.ToString() });
                    responseObj.Add(new Parameter { Key = "RcvblAmount" + count, Value = item.DepositInstAmt.ToString() });
                    responseObj.Add(new Parameter { Key = "PaymentState" + count, Value = "Unpaid" });
                    count++;
                }
            }
            responseObj.Add(new Parameter { Key = "TotalAmount", Value = totalAmount.ToString() });
            responseObj.Add(new Parameter { Key = "TotalPaymentActual", Value = totalAmount.ToString() });
            return responseObj.ToArray();
        }
    }
    public class RootObject
    {
        
        public List<DepositAccountinfo> DepositAccountinfo { get; set; }
        public List<Memberbasicinfo> Memberbasicinfo { get; set; }
        public List<Microcreditinfo> Microcreditinfo { get; set; }
        public List<OtherInvestmentinfo> OtherInvestmentinfo { get; set; }
    }
}
