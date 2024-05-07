using Domain.CacheMemory;
using Domain.DatabaseC;
using Domain.RequestC;
using Domain.ResponseC;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Persistence;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Application.ThirdPartyChannel
{
    public class PaymentConfirm
    {
        public class Query : IRequest<bKashOuterResponseClass>
        {
            public required string Timestamp { get; set; }
            public required string Version { get; set; }
            public required string ConversationID { get; set; }
            public required string Command { get; set; }
            public required string LoginID { get; set; }
            public required string Password { get; set; }
            public required ReqPaymentConfirmation reqPaymentConfirmation { get; set; }
        }
        public class Handler : IRequestHandler<Query, bKashOuterResponseClass>
        {
            private readonly DataContext _context;
            private readonly ILogger<PaymentConfirm> _logger;
            public Handler(DataContext context, ILogger<PaymentConfirm> logger)
            {
                _context = context;
                _logger = logger;
                
            }

            public async Task<bKashOuterResponseClass> Handle(Query request, CancellationToken cancellationToken)
            {
                bKashOuterResponseClass responseObj = new bKashOuterResponseClass();
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
                var data = _context.GatewayInfo.Where(s=>s.GatewayId==request.reqPaymentConfirmation.PartnerIntentRef && s.TrxRef == request.reqPaymentConfirmation.bKashTrxRef).FirstOrDefault();
                if (data == null)
                {
                    _logger.LogWarning("Handling command {PaymentConfirm}", "Invalid bKash TrxRef ("+request.reqPaymentConfirmation.bKashTrxRef+") and partners PartnerIntentRef ("+request.reqPaymentConfirmation.PartnerIntentRef+")");
                    responseObj.ResponseCode = "BEC0003";
                    responseObj.ResponseDesc = "Invalid bKash TrxRef and partners PartnerIntentRef";
                    responseObj.Parameters = [];
                    return responseObj;
                }
                else
                {
                    if (data.PaymentConfirmResCode=="0")
                    {
                        responseObj.ResponseCode = "0";
                        responseObj.ResponseDesc = "Success";
                        responseObj.Parameters = param(request, data);
                        return responseObj;
                    }
                    else if (data.PaymentConfirmResCode == "1")
                    {
                        _logger.LogWarning("Handling command {PaymentConfirm}", "Permission not given for transctation GatewayID(" + data.GatewayId + ")");
                        responseObj.ResponseCode = "BECP001";
                        responseObj.ResponseDesc = "This account is not allowed to see the data. Please contact with your branch office.";
                        responseObj.Parameters = [];
                        return responseObj;
                    }
                    else if (data.PartnerID != request.reqPaymentConfirmation.PartnerID)
                    {
                        _logger.LogWarning("Handling command {PaymentConfirm}", "Partner ID did not matched (req: "+request.reqPaymentConfirmation.PartnerID+"), Gateway Saved data: "+data.PartnerID+"");
                        responseObj.ResponseCode = "BECP001";
                        responseObj.ResponseDesc = "This account is not allowed to see the data. Please contact with your branch office.";
                        responseObj.Parameters = [];
                        return responseObj;
                    }
                    else
                    {
                        data.PaymentConfirmReqDate = DateTime.Now;
                        try
                        {
                            var ip = _context.RoutingServerList.Where(s => s.BranchCode == data.BranchCode).FirstOrDefault();
                            if (ip == null)
                            {
                                _logger.LogError("Handling command {PaymentConfirm}", "IP not found");
                                responseObj.ResponseCode = "BECP005";
                                responseObj.ResponseDesc = "Sorry, an error occurred";
                                responseObj.Parameters = [];
                                return responseObj;
                            }
                            HttpClient client = new HttpClient();
                            client.BaseAddress = new Uri("http://" + ip.Ip + "/");
                            var reqtime = DateTime.Now.ToString("yyyyMMddHHmmss");
                            HttpResponseMessage response = await client.GetAsync($"api/paymentValidation/checkbyFinapp?AccountID={data.PaymentID}&ActAmount={data.ActualAmount}&payamount={data.PayAmount}&referenceNo={data.TrxRef}&reqTime={reqtime}&channelcode={data.ChannelCode}");

                            data.PaymentConfirmResCode = response.StatusCode.ToString();

                            if (response.IsSuccessStatusCode)
                            {
                                string responseBody = await response.Content.ReadAsStringAsync();
                                var dateTimeConverter = new IsoDateTimeConverter { DateTimeFormat = "dd-MM-yyyy" };
                                var rootObject = JsonConvert.DeserializeObject<PaymentConfirmRes>(responseBody, dateTimeConverter);
                                data.SysSpResDateTime = DateTime.Now;
                                if (rootObject == null)
                                {
                                    responseObj.ResponseCode = "BECP003";
                                    responseObj.ResponseDesc = "Sorry, no data found in your requested time.";
                                    responseObj.Parameters = [];
                                    data.FinAppResCode = "1";
                                    data.PaymentConfirmResCode = "1";
                                    data.PaymentConfirmResDesc = "Null received from FinApp";
                                    _context.Entry(data).State = EntityState.Modified;
                                    await _context.SaveChangesAsync();
                                    return responseObj;
                                }
                                else
                                {
                                    if (rootObject.status == "0")
                                    {
                                        responseObj.ResponseCode = "0";
                                        responseObj.ResponseDesc = "Success";
                                        responseObj.Parameters = param(request, data);
                                        data.PaymentConfirmResDate = DateTime.Now;
                                        data.SystemTrID = rootObject.trIDNo;
                                        data.PaymentConfirmResCode = "0";
                                        data.PaymentConfirmResDesc = "Success";
                                        _context.Entry(data).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                        return responseObj;
                                    }
                                    else if (rootObject.status == "1")
                                    {
                                        
                                        responseObj.ResponseCode = "BECP001";
                                        responseObj.ResponseDesc = "This account is not allowed to see the data. Please contact with your branch office.";
                                        responseObj.Parameters = [];
                                        data.PaymentConfirmResDate = DateTime.Now;
                                        data.SystemTrID = rootObject.trIDNo;
                                        data.PaymentConfirmResCode = rootObject.status;
                                        data.PaymentConfirmResDesc = "Success";
                                        _context.Entry(data).State = EntityState.Modified;
                                        await _context.SaveChangesAsync();
                                        _logger.LogWarning("Handling command {PaymentConfirm}", "Permission not given for transctation GatewayID(" + data.GatewayId + ")");
                                        return responseObj;
                                    }
                                    else
                                    {
                                        responseObj.ResponseCode = "BECP005";

                                        responseObj.ResponseDesc = "Sorry, an error occurred";
                                        responseObj.Parameters = [];
                                        _logger.LogWarning("Handling command {PaymentConfirm}", "Plese check the status from FinApp(" + rootObject.ToString() + ")");
                                        return responseObj;
                                    }
                                }
                                
                            }
                            else
                            {
                                // Handle the error response
                                data.PaymentConfirmResCode = response.StatusCode.ToString();
                                data.PaymentConfirmResDesc = response.Headers.ToString();
                                _context.Entry(data).State = EntityState.Modified;
                                await _context.SaveChangesAsync();
                                responseObj.ResponseCode = "BECP005";
                                responseObj.ResponseDesc = "Sorry, an error occurred";
                                responseObj.Parameters = [];
                                _logger.LogWarning("Handling command {PaymentConfirm}", "Plese check the status from Gatwayinfo. Gateway ID(" + data.GatewayId + ") response code is "+ response.StatusCode+ " for this request "+response.RequestMessage+"");
                                return responseObj;
                            }


                        }
                        catch (HttpRequestException e)
                        {
                            _logger.LogError("Handling command {PaymentConfirm}", e.Message + "-" + e.InnerException);
                            data.PaymentConfirmResDesc = e.Message;
                            _context.Entry(data).State = EntityState.Modified;
                            await _context.SaveChangesAsync();
                        }
                    }
                    return responseObj;
                }
                
            }
            private static Parameter[] param(Query request, GatewayInfo data)
            {
                var responseObj = new List<Parameter>
                {
                    new Parameter { Key = "InitiatorID", Value = request.reqPaymentConfirmation.InitiatorID },
                    new Parameter { Key = "PayMode", Value = "0" },
                    new Parameter { Key = "bKashTrxRef", Value = request.reqPaymentConfirmation.bKashTrxRef },
                    //change after dadas end
                    new Parameter { Key = "PartnerIntentRef", Value = request.reqPaymentConfirmation.PartnerIntentRef },
                    new Parameter { Key = "AccountID", Value = data.AccountID },
                    new Parameter { Key = "PaymentID", Value = data.PaymentID },
                    new Parameter { Key = "AccountRemarks", Value = data.Event }
                };
                return responseObj.ToArray();
            }
        }
    }
    public class RootObjectPaymentConfirm
    {
        public PaymentConfirmRes? paymentConfirmRes { get; set; }
    }
}
