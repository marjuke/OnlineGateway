using MediatR;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ResponseC;
using Domain.DatabaseC;
using Azure;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Domain.RequestC;
using static Application.ThirdPartyChannel.ValidationPayment;
using System.Net.Http;
using Microsoft.Extensions.Caching.Memory;
using Domain.CacheMemory;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;

namespace Application.ThirdPartyChannel
{
    public class ValidationPayment
    {
        public class Query : IRequest<bKashOuterResponseClass>
        {
            public required string AccountID { get; set; }
            public required string InitiatorId { get; set; }
            public required string RequesterMSISDN { get; set; }
            public required string Timestamp { get; set; }
            public required string ActualAmount { get; set; }
            public required string Version { get; set; }
            public required string ConversationID { get; set; }
            public required string Command { get; set; }
            public required string LoginID { get; set; }
            public required string Password { get; set; }
            public required ReqValidationPayment reqValidationPayment { get; set; }
        }
        public class Handler : IRequestHandler<Query, bKashOuterResponseClass>
        {
            private readonly DataContext _context;
            private readonly ILogger<ValidationPayment> _logger;
            public Handler(DataContext context, ILogger<ValidationPayment> logger)
            {
                _context = context;
                _logger = logger;
            }

            public async Task<bKashOuterResponseClass> Handle(Query request, CancellationToken cancellationToken)
            {
                var responseObj = new bKashOuterResponseClass();
                var responseObjArray = new List<Parameter>();
                try
                {
                    var stan = _context.stan.FirstOrDefault();
                    if (stan == null)
                    {
                        Stan stan1 = new Stan();
                        stan1.CounterDate = DateTime.Now.Date;
                        stan1.CounterValue = 0;
                        _context.stan.Add(stan1);
                        _context.SaveChanges();
                    }
                    else
                    {
                        if (stan.CounterDate != DateTime.Now.Date)
                        {
                            stan.CounterDate = DateTime.Now.Date;
                            stan.CounterValue = 0;
                            _context.Entry(stan).State = EntityState.Modified;
                            _context.SaveChanges();
                        }
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
                    //payamount must be greater than 0
                    if (Convert.ToDecimal(request.reqValidationPayment.PayAmount) <= 0)
                    {
                        _logger.LogWarning("Handling command {CheckInformation}", "PayAmount is less than 0");
                        responseObj = new bKashOuterResponseClass
                        {
                            ResponseCode = "BEC0001",
                            ResponseDesc = "PayAmount must be greater than 0. Please try again.",
                            Parameters = []
                        };
                        return responseObj;
                    }
                    // channelCode checking is mandatory
                    if (string.IsNullOrEmpty(request.reqValidationPayment.channelCode))
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
                    if (!_context.ChannelCode.Any(s => s.Code == request.reqValidationPayment.channelCode && s.channelId == 1 && s.IsActive == true))
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
                    var gatewaydata = _context.GatewayInfo.Where(s => s.TrxRef == request.reqValidationPayment.bKashTrxRef).FirstOrDefault();
                    if (gatewaydata != null)
                    {
                        if (gatewaydata.ResCode == "0")
                        {
                            responseObj.ResponseCode = "0";
                            responseObj.ResponseDesc = "Success";
                            request.reqValidationPayment.GatewayID = gatewaydata.GatewayId;
                            
                            responseObj.Parameters = param(request,gatewaydata.Event,gatewaydata.PartnerID);
                            return responseObj;
                        }
                        else
                        {
                            _logger.LogWarning("Handling command {GatewayResponse}", "The Status from FinApp is False account ID (" + request.AccountID + ")");

                            responseObj.ResponseCode = "BECP001";
                            responseObj.ResponseDesc = "This account is not allowed to see the data. Please contact with your branch office.";
                            responseObj.Parameters = [];
                            return responseObj;
                        }

                    }

                
                    GatewayInfo data = new GatewayInfo() { GatewayId = "", InitiatorID = "", BranchCode = "" };
                    data.GatewayId = "0";
                    data.ChannelListId = 1;
                    data.AccountID = request.AccountID;
                    data.ChannelReqDateTime = DateTime.ParseExact(request.Timestamp, "yyyyMMddHHmmss", null);
                    data.ServerReqDate = DateTime.Now.Date;
                    data.ActualAmount = request.ActualAmount;
                    data.BranchCode = request.AccountID.Substring(0, 3);
                    data.ChannelCode = "1";
                    data.Version = request.Version;
                    data.ConversationID = request.ConversationID;
                    data.LoginID = request.LoginID;
                    data.Password = request.Password;
                    data.Command = request.Command;
                    data.ChannelCode = request.reqValidationPayment.channelCode;
                    data.SysChannelReqDateTime = DateTime.Now;
                    data.SysSpReqDateTime = DateTime.Now;
                    data.InitiatorID = request.InitiatorId;
                    data.Timestamp = data.ChannelReqDateTime;
                    data.TrxRef = request.reqValidationPayment.bKashTrxRef;
                    data.ContactNo = request.reqValidationPayment.ContactNo;
                    var ip = _context.RoutingServerList.Where(s => s.BranchCode == data.BranchCode).FirstOrDefault();
                    if (ip == null)
                    {
                        _logger.LogError("Handling command {ValidationIpCheck}", "RoutingServerList table has no ip for branch ("+data.BranchCode+")");
                        responseObj.ResponseCode = "BECP005";
                        responseObj.ResponseDesc = "Sorry, an error occurred";
                        responseObj.Parameters = [];
                        return responseObj;
                    }
                    HttpClient client = new()
                    {
                        BaseAddress = new Uri("http://" + ip.Ip + "/")
                    };

                    //HttpResponseMessage response = await client.GetAsync($"api/AccountConfirmation/CheckValidatation?AccountID={request.reqValidationPayment.PaymentID}&payamount={request.reqValidationPayment.PayAmount}");
                    HttpResponseMessage response = await client.GetAsync($"api/AccountConfirmation/CheckValidatation?AccountID={request.reqValidationPayment.PaymentID}&payamount={request.reqValidationPayment.PayAmount}&MobileNo={request.reqValidationPayment.InitiatorID}&walletNumber={request.reqValidationPayment.PartnerID}&channelcode={request.reqValidationPayment.channelCode}");

                    data.ResCode = response.StatusCode.ToString();
                    if (response.IsSuccessStatusCode)
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
                        var rootObject = JsonConvert.DeserializeObject<ValidationPaymentRes>(responseBody);
                        data.SysSpResDateTime = DateTime.Now;
                        data.PayAmount = request.reqValidationPayment.PayAmount;
                        data.SysChannelResDateTime = DateTime.Now;
                        
                        if (rootObject == null)
                        {
                            _logger.LogError("Handling command {CheckValidatation}", "Data not found for account ("+data.AccountID+") from FinApp");
                            responseObj.ResponseCode = "BECP003";
                            responseObj.ResponseDesc = "Sorry, no data found in your requested time.";
                            responseObj.Parameters = [];
                            data.ResCode = null;
                            data.ResponseDesc = "No data found from FinApp";
                            _context.GatewayInfo.Add(data);
                            await _context.SaveChangesAsync();
                            return responseObj;
                        }
                        else
                        {
                            if (rootObject.status == "1")
                            {
                                _logger.LogWarning("Handling command {CheckValidatation}", "This account ("+data.AccountID+") is not allowed to see the data");
                                data.ResCode = "1";
                                responseObj.ResponseCode = "BECP001";
                                responseObj.ResponseDesc = "This account is not allowed to see the data. Please contact with your branch office.";
                                //responseObj.Parameters = param(request);
                                _context.GatewayInfo.Add(data);
                                await _context.SaveChangesAsync();
                                request.reqValidationPayment.GatewayID = data.GatewayId;
                                return responseObj;
                            }
                            else if(rootObject.status == "0")
                            {
                                responseObj.ResponseCode = "0";
                                responseObj.ResponseDesc = "Success";
                                data.Event = rootObject.BranchName;
                                data.PartnerID = rootObject.WalletNumber;
                                data.ResCode = "0";
                                data.PaymentID = request.reqValidationPayment.PaymentID;
                                _context.GatewayInfo.Add(data);
                                await _context.SaveChangesAsync();
                                request.reqValidationPayment.GatewayID = data.GatewayId;
                                responseObj.Parameters = param(request, data.Event, data.PartnerID );
                                return responseObj;
                            }
                            else
                            {
                                _logger.LogWarning("Handling command {CheckValidatation}", "Status is ("+rootObject.status+")");
                                data.ResCode = "1";
                                responseObj.ResponseCode = "BECP001";
                                responseObj.ResponseDesc = "This account is not allowed to see the data. Please contact with your branch office.";
                                //responseObj.Parameters = param(request);
                                _context.GatewayInfo.Add(data);
                                await _context.SaveChangesAsync();
                                request.reqValidationPayment.GatewayID = data.GatewayId;
                                return responseObj;
                            }
                            
                        }
                        
                    }
                    else
                    {
                        _logger.LogError("Handling command {CheckValidation}", "FinApp response code is "+ response.StatusCode+ " for this request "+response.RequestMessage+"");
                        // Handle the error response
                        _context.GatewayInfo.Add(data);
                        await _context.SaveChangesAsync();
                        responseObj.ResponseCode = "BECP003";
                        responseObj.ResponseDesc = "Sorry, no data found in your requested time.";
                        responseObj.Parameters = [];
                        return responseObj;
                    }


                }
                catch (HttpRequestException e)
                {
                    _logger.LogError("Handling command {CheckInformation}", e.Message + "-" + e.InnerException);
                    responseObj.ResponseCode = "BECP005";
                    responseObj.ResponseDesc = "Sorry, an error occurred";
                    responseObj.Parameters = [];
                    return responseObj;
                }


                
            }
            private static Parameter[] param(Query request, string branchName, string walletNumber)
            {
                var responseObj = new List<Parameter>
                {
                    new Parameter { Key = "InitiatorID", Value = request.InitiatorId },
                    new Parameter { Key = "PayMode", Value = "0" },
                    new Parameter { Key = "bKashTrxRef", Value = request.reqValidationPayment.bKashTrxRef },
                    //change after dadas end
                    new Parameter { Key = "PartnerIntentRef", Value = request.reqValidationPayment.GatewayID },
                    new Parameter { Key = "AccountID", Value = request.AccountID },
                    new Parameter { Key = "PaymentID", Value = request.reqValidationPayment.PaymentID },
                    new Parameter { Key = "ContactNo", Value = request.reqValidationPayment.ContactNo },
                    new Parameter { Key = "ActualAmount", Value = request.reqValidationPayment.ActualAmount },
                    new Parameter { Key = "PayAmount", Value = request.reqValidationPayment.PayAmount },
                    new Parameter { Key = "PartialAmountFlag", Value = request.reqValidationPayment.PartialAmountFlag },
                    new Parameter { Key = "AccountRemarks", Value = branchName }
                };
                return responseObj.ToArray();
            }
            
            
            

        }

    }
    public class RootObjectValidation
    {
        //public List<Memberbasicinfo> Memberbasicinfo { get; set; }
        public ValidationPaymentRes validationPaymentRes { get; set; }
    }
}
