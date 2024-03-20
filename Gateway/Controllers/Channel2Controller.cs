using Microsoft.AspNetCore.Mvc;
using SOAP.Model;
using SOAP;
using SOAP.Controllers;
using DotNetSOAPStarter.Model;
using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Application.ThirdPartyChannel;
using Domain.RequestC;
using Gateway.MappingArray;
using Microsoft.Extensions.Caching.Memory;
using Gateway.Model.Password;
using Application.StanService;
using Domain.DatabaseC;
using System.Net;
using NuGet.Protocol;
using Gateway.MiddleWare;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Xml;
using Gateway.Model.ErrorResponse;

namespace Gateway.Controllers;
    
[ApiController]
[SOAPController(SOAPVersion.v1_2)]
public class Channel2Controller : SOAPControllerBase
{
    private readonly ILogger<ChannelController> _logger;
    private readonly UserManager<AppUser> _userManager;

    public Channel2Controller(UserManager<AppUser> userManager, ILogger<ChannelController> logger, IWebHostEnvironment env) : base(logger, env)
    {
        _logger = logger;
        _userManager = userManager;


    }

    [HttpPost]
    //[PayloadRequired]
    [Consumes("application/xml")]
    [Produces("application/soap+xml")]
    public async Task<XmlDocument> OperationSelector(SOAP1_2RequestEnvelope env)
    {
        
        _logger.LogInformation("OperationSelectior is called ");
        await Mediator.Send(new StanUpdate.Command { Stan = "OK" });
        if (!env.Body.ApplyTransactionRequest.Header.LoginID.IsNullOrEmpty())
        {
            var user = _userManager.Users.Where(s => s.UserID == env.Body.ApplyTransactionRequest.Header.LoginID).FirstOrDefault();

            if (user != null)
            {
                var Password = PasswordDecryptor.GetPassword(env.Body.ApplyTransactionRequest.Header.Password);
                var result = await _userManager.CheckPasswordAsync(user, Password);
                _logger.LogInformation("Login");
                //var result1 = await _userManager.CheckPasswordAsync(user, "PassNGOPay@321#");
                if (result)
                {
                    _logger.LogInformation("Login successfull");
                    if (env?.Body?.ApplyTransactionRequest is not null)
                    {
                        if (env.Body.ApplyTransactionRequest.Header.CommandID.Trim() == "CheckInformation")
                        {

                            //return new CustomXmlResult( GetApplyTransactionRequest(env.Body.ApplyTransactionRequest));
                            return await GetApplyTransactionRequest(env.Body.ApplyTransactionRequest);
                            //return await new SoapResult(soapXmlDocument);
                        }
                        else if (env.Body.ApplyTransactionRequest.Header.CommandID.Trim() == "ValidationPayment")
                        {
                            return await GetApplyValidationRequest(env.Body.ApplyTransactionRequest);
                        }
                        else if (env.Body.ApplyTransactionRequest.Header.CommandID.Trim() == "PaymentConfirmation")
                        {
                            return await GetApplyPaymentConfirmation(env.Body.ApplyTransactionRequest);
                        }
                        else
                        {
                            _logger.LogInformation("Command not specified " + env + "");
                            return XmlErrorResponse.GetXmlErrorResponse("BEC0004", "Invalid CommandID");
                        }
                    }
                    return XmlErrorResponse.GetXmlErrorResponse("BECP003", "Sorry, no data found in your requested time");
                    
                }
                else
                {
                    _logger.LogInformation("User not authorized " + env + "");
                    return XmlErrorResponse.GetXmlErrorResponse("BEC0006", "Invalid Password");
                }
            }
            else
            {
                _logger.LogInformation("User not found " + env + "");
                return XmlErrorResponse.GetXmlErrorResponse("BEC0006", "Invalid Password");
            }


        }
        else
        {
            _logger.LogInformation("User required " + env + "");
            return XmlErrorResponse.GetXmlErrorResponse("BEC0006", "Invalid Password");
        }
    }


    protected async Task<XmlDocument> GetApplyTransactionRequest(Model.bKashRequest.ApplyTransactionRequest req)
    {
        var xml = new XmlDocument();
        try
        {
            var ConversationID = req.Header.ConversationID;
            var command = req.Header.CommandID;
            var timestamp = req.Header.Timestamp;
            var version = req.Header.Version;
            var loginID = req.Header.LoginID;
            var Password = req.Header.Password;

            var ParentId = req.Body.Parameters.Parameter.Where(s => s.Key == "InitiatorID").FirstOrDefault();
            var d = ParentId.Value.ToString();

            ReqCheckInformation myObject = new ReqCheckInformation();

            // Map the array to the class
            MappingArrayToClass.MapArrayToClass(req.Body.Parameters.Parameter, myObject);
            _logger.LogInformation("Check Request values are " + myObject.ToJson() + "");
            var DataArray = await Mediator.Send(new CheckInformation.Query { AccountID = myObject.AccountID, InitiatorId = myObject.InitiatorID, RequesterMSISDN = myObject.RequesterMSISDN, Timestamp = timestamp, Version = version.ToString(), ConversationID = ConversationID, LoginID = loginID, Password = Password, Command = command, reqCheckInformation = myObject });
            _logger.LogInformation("Check response before envelope " + DataArray.ToJson() + "");
            //var res = CreateSOAPResponseEnvelope();
            //_logger.LogInformation("Check response create envelope ");

            var datalist = new Model.ApplyTransactionResponse();
            //res.Header.Timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            //res.Header.CommandID = command;
            //res.Header.Version = String.Format("{0:0.0}", version);
            //res.Header.ConversationID = ConversationID;



            datalist.ResponseCode = DataArray.ResponseCode;
            datalist.ResponseDesc = DataArray.ResponseDesc;
            datalist.Parameters = DataArray.Parameters;


            

            //produce utf-16 xml
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-16", null));



            //var root = xml.CreateElement("WeatherForecasts");
            var root = xml.CreateElement("soapenv", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            xml.AppendChild(root);
            //add at,tem,goa namspace to root element
            XmlAttribute xmlnsat = xml.CreateAttribute("xmlns:at");
            xmlnsat.Value = "http://cps.huawei.com/cpsinterface/goa/at";
            root.Attributes.Append(xmlnsat);
            XmlAttribute xmlnstem = xml.CreateAttribute("xmlns:tem");
            xmlnstem.Value = "http://tempuri.org/";
            root.Attributes.Append(xmlnstem);
            XmlAttribute xmlnsgoa = xml.CreateAttribute("xmlns:goa");
            xmlnsgoa.Value = "http://cps.huawei.com/cpsinterface/goa";
            root.Attributes.Append(xmlnsgoa);

            var header = xml.CreateElement("soapenv", "Header", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(header);

            var Version = xml.CreateElement("Version");
            Version.InnerText = String.Format("{0:0.0}", version);
            header.AppendChild(Version);

            var CommandID = xml.CreateElement("CommandID");
            CommandID.InnerText = command;
            header.AppendChild(CommandID);

            var ConversationIDElement = xml.CreateElement("ConversationID");
            ConversationIDElement.InnerText = ConversationID;
            header.AppendChild(ConversationIDElement);

            var Timestamp = xml.CreateElement("Timestamp");
            Timestamp.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");
            header.AppendChild(Timestamp);


            var body = xml.CreateElement("soapenv", "Body", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(body);
            //add ApplyTransactionResponse in body
            //var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://tempuri.org/");
            var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://tempuri.org/");
            body.AppendChild(ApplyTransactionResponse);
            //add response code and responsedescription in ApplyTransactionResponse
            //var responseCode = xml.CreateElement("at", "ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
            var responseCode = xml.CreateElement( "ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
            responseCode.InnerText = datalist.ResponseCode;
            ApplyTransactionResponse.AppendChild(responseCode);
            //var responseDescription = xml.CreateElement("at", "ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
            var responseDescription = xml.CreateElement( "ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
            responseDescription.InnerText = datalist.ResponseDesc;
            ApplyTransactionResponse.AppendChild(responseDescription);
            //add Parementers in ApplyTransactionResponse
            //var parameters = xml.CreateElement("at", "Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
            var parameters = xml.CreateElement("Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
            ApplyTransactionResponse.AppendChild(parameters);


            foreach (var data in DataArray.Parameters)
            {
                //<goa:Parameter>< goa:Key > PayMode </ goa:Key >< goa:Value > 02 </ goa:Value ></ goa:Parameter > generate this type of xml

                //XmlElement parameterElement = xml.CreateElement("goa", "Parameter", "http://cps.huawei.com/cpsinterface/goa");
                XmlElement parameterElement = xml.CreateElement("Parameter", "http://cps.huawei.com/cpsinterface/goa");
                parameters.AppendChild(parameterElement);
                // Create Key element
                //XmlElement keyElement = xml.CreateElement("goa", "Key", "http://cps.huawei.com/cpsinterface/goa");
                XmlElement keyElement = xml.CreateElement("Key", "http://cps.huawei.com/cpsinterface/goa");
                keyElement.InnerText = data.Key;



                XmlElement valueElement = xml.CreateElement( "Value", "http://cps.huawei.com/cpsinterface/goa");
                valueElement.InnerText = data.Value;

                // Append Key and Value elements to Parameter element
                parameterElement.AppendChild(keyElement);
                parameterElement.AppendChild(valueElement);

                // Append Parameter element to XmlDocument
                //parameterElement.AppendChild(parameterElement);

                

                //var parameter = xml.CreateElement("goa", "Parameter", "http://cps.huawei.com/cpsinterface/goa");
                //parameters.AppendChild(parameter);
                //var Key = xml.CreateElement("goa", "Key");
                //Key.InnerText = data.Key;
                //parameter.AppendChild(Key);
                //var Value = xml.CreateElement("goa", "Value");
                //Value.InnerText = data.Value;
                //parameter.AppendChild(Value);
            }

            //res.Body.ApplyTransactionResponse = datalist;
            //_logger.LogInformation("Check Response values are " + res.ToJson() + "");
            return xml;
        }
        catch (Exception ex)
        {

            _logger.LogError("Issue in Channel Controller check method" + ex.InnerException + " and request values are " + req + "");

            return xml;
        }

    }
    protected async Task<XmlDocument> GetApplyValidationRequest(Model.bKashRequest.ApplyTransactionRequest req)
    {
        var xml = new XmlDocument();
        try
        {
            //var ParentId = req.Body.Parameters.Parameter.Where(s => s.Key == "InitiatorID").FirstOrDefault();
            var ConversationID = req.Header.ConversationID;
            var command = req.Header.CommandID;
            var timestamp = req.Header.Timestamp;
            var version = req.Header.Version;
            var loginID = req.Header.LoginID;
            var Password = req.Header.Password;
            // var d = ParentId.Value.ToString();

            ReqValidationPayment myObject = new ReqValidationPayment();

            // Map the array to the class
            MappingArrayToClass.MapArrayToClass(req.Body.Parameters.Parameter, myObject);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            };

            _logger.LogInformation("Validation Request values are " + myObject.ToJson() + "");
            var DataArray = await Mediator.Send(new ValidationPayment.Query { AccountID = myObject.AccountID, InitiatorId = myObject.InitiatorID, RequesterMSISDN = myObject.RequesterMSISDN, Timestamp = timestamp, ActualAmount = myObject.ActualAmount, Version = version.ToString(), ConversationID = ConversationID, LoginID = loginID, Password = Password, Command = command, reqValidationPayment = myObject });


            //produce utf-16 xml
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-16", null));



            //var root = xml.CreateElement("WeatherForecasts");
            var root = xml.CreateElement("soapenv", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            xml.AppendChild(root);
            //add at,tem,goa namspace to root element
            XmlAttribute xmlnsgoa = xml.CreateAttribute("xmlns:goa");
            xmlnsgoa.Value = "http://cps.huawei.com/cpsinterface/goa";
            root.Attributes.Append(xmlnsgoa);
            XmlAttribute xmlnsat = xml.CreateAttribute("xmlns:at");
            xmlnsat.Value = "http://cps.huawei.com/cpsinterface/goa/at";
            root.Attributes.Append(xmlnsat);
            XmlAttribute xmlnstem = xml.CreateAttribute("xmlns:tem");
            xmlnstem.Value = "http://tempuri.org/";
            root.Attributes.Append(xmlnstem);
            

            var header = xml.CreateElement("soapenv", "Header", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(header);

            var Version = xml.CreateElement("Version");
            Version.InnerText = String.Format("{0:0.0}", version);
            header.AppendChild(Version);

            var CommandID = xml.CreateElement("CommandID");
            CommandID.InnerText = command;
            header.AppendChild(CommandID);

            var ConversationIDElement = xml.CreateElement("ConversationID");
            ConversationIDElement.InnerText = ConversationID;
            header.AppendChild(ConversationIDElement);

            var Timestamp = xml.CreateElement("Timestamp");
            Timestamp.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");
            header.AppendChild(Timestamp);


            var body = xml.CreateElement("soapenv", "Body", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(body);
            //add ApplyTransactionResponse in body
            //var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://cps.huawei.com/cpsinterface/goa");
            var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://tempuri.org/");
            body.AppendChild(ApplyTransactionResponse);
            //add response code and responsedescription in ApplyTransactionResponse
            var responseCode = xml.CreateElement("at", "ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
            //var responseCode = xml.CreateElement("at","ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
            responseCode.InnerText = DataArray.ResponseCode;
            ApplyTransactionResponse.AppendChild(responseCode);
            var responseDescription = xml.CreateElement("at", "ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
            //var responseDescription = xml.CreateElement("ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
            responseDescription.InnerText = DataArray.ResponseDesc;
            ApplyTransactionResponse.AppendChild(responseDescription);
            //add Parementers in ApplyTransactionResponse
            var parameters = xml.CreateElement("at", "Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
            //var parameters = xml.CreateElement("Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
            ApplyTransactionResponse.AppendChild(parameters);


            foreach (var data in DataArray.Parameters)
            {
                //<goa:Parameter>< goa:Key > PayMode </ goa:Key >< goa:Value > 02 </ goa:Value ></ goa:Parameter > generate this type of xml

                XmlElement parameterElement = xml.CreateElement("goa", "Parameter", "http://cps.huawei.com/cpsinterface/goa");
                //XmlElement parameterElement = xml.CreateElement("Parameter", "http://cps.huawei.com/cpsinterface/goa");
                parameters.AppendChild(parameterElement);
                // Create Key element
                XmlElement keyElement = xml.CreateElement("goa", "Key", "http://cps.huawei.com/cpsinterface/goa");
                //XmlElement keyElement = xml.CreateElement("Key", "http://cps.huawei.com/cpsinterface/goa");
                keyElement.InnerText = data.Key;


                XmlElement valueElement = xml.CreateElement("goa", "Value", "http://cps.huawei.com/cpsinterface/goa");
                //XmlElement valueElement = xml.CreateElement("Value", "http://cps.huawei.com/cpsinterface/goa");
                valueElement.InnerText = data.Value;

                // Append Key and Value elements to Parameter element
                parameterElement.AppendChild(keyElement);
                parameterElement.AppendChild(valueElement);

                // Append Parameter element to XmlDocument
                //parameterElement.AppendChild(parameterElement);



                //var parameter = xml.CreateElement("goa", "Parameter", "http://cps.huawei.com/cpsinterface/goa");
                //parameters.AppendChild(parameter);
                //var Key = xml.CreateElement("goa", "Key");
                //Key.InnerText = data.Key;
                //parameter.AppendChild(Key);
                //var Value = xml.CreateElement("goa", "Value");
                //Value.InnerText = data.Value;
                //parameter.AppendChild(Value);
            }

            //res.Body.ApplyTransactionResponse = datalist;
            //_logger.LogInformation("Check Response values are " + res.ToJson() + "");
            return xml;
        }
        catch (Exception ex)
        {

            _logger.LogError("Issue in Channel Controller confirm method" + ex.InnerException + " and request values are " + req + "");
            return xml;
        }


    }

    protected async Task<XmlDocument> GetApplyPaymentConfirmation(Model.bKashRequest.ApplyTransactionRequest req)
    {
        var xml = new XmlDocument();

        try
        {
            //var ParentId = req.Body.Parameters.Parameter.Where(s => s.Key == "InitiatorID").FirstOrDefault();
            var ConversationID = req.Header.ConversationID;
            var command = req.Header.CommandID;
            var version = req.Header.Version;
            var timestamp = req.Header.Timestamp;

            var loginID = req.Header.LoginID;
            var Password = req.Header.Password;
            // var d = ParentId.Value.ToString();

            ReqPaymentConfirmation myObject = new ReqPaymentConfirmation();

            // Map the array to the class
            MappingArrayToClass.MapArrayToClass(req.Body.Parameters.Parameter, myObject);
            //myObject.ConversationID = ConversationID.Value;
            _logger.LogInformation("Payment Request values are " + myObject.ToJson() + "");

            var DataArray = await Mediator.Send(new PaymentConfirm.Query { Timestamp = timestamp, Version = version.ToString(), ConversationID = ConversationID, LoginID = loginID, Password = Password, Command = command, reqPaymentConfirmation = myObject });

            //produce utf-16 xml
            xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-16", null));



            //var root = xml.CreateElement("WeatherForecasts");
            var root = xml.CreateElement("soapenv", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
            xml.AppendChild(root);
            //add at,tem,goa namspace to root element
            XmlAttribute xmlnsgoa = xml.CreateAttribute("xmlns:goa");
            xmlnsgoa.Value = "http://cps.huawei.com/cpsinterface/goa";
            root.Attributes.Append(xmlnsgoa);
            XmlAttribute xmlnsat = xml.CreateAttribute("xmlns:at");
            xmlnsat.Value = "http://cps.huawei.com/cpsinterface/goa/at";
            root.Attributes.Append(xmlnsat);
            XmlAttribute xmlnstem = xml.CreateAttribute("xmlns:tem");
            xmlnstem.Value = "http://tempuri.org/";
            root.Attributes.Append(xmlnstem);


            var header = xml.CreateElement("soapenv", "Header", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(header);

            var Version = xml.CreateElement("Version");
            Version.InnerText = String.Format("{0:0.0}", version);
            header.AppendChild(Version);

            var CommandID = xml.CreateElement("CommandID");
            CommandID.InnerText = command;
            header.AppendChild(CommandID);

            var ConversationIDElement = xml.CreateElement("ConversationID");
            ConversationIDElement.InnerText = ConversationID;
            header.AppendChild(ConversationIDElement);

            var Timestamp = xml.CreateElement("Timestamp");
            Timestamp.InnerText = DateTime.Now.ToString("yyyyMMddHHmmss");
            header.AppendChild(Timestamp);


            var body = xml.CreateElement("soapenv", "Body", "http://www.w3.org/2003/05/soap-envelope");
            root.AppendChild(body);
            //add ApplyTransactionResponse in body
            //var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://cps.huawei.com/cpsinterface/goa");
            var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://tempuri.org/");
            body.AppendChild(ApplyTransactionResponse);
            //add response code and responsedescription in ApplyTransactionResponse
            var responseCode = xml.CreateElement("at", "ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
            //var responseCode = xml.CreateElement("at","ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
            responseCode.InnerText = DataArray.ResponseCode;
            ApplyTransactionResponse.AppendChild(responseCode);
            var responseDescription = xml.CreateElement("at", "ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
            //var responseDescription = xml.CreateElement("ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
            responseDescription.InnerText = DataArray.ResponseDesc;
            ApplyTransactionResponse.AppendChild(responseDescription);
            //add Parementers in ApplyTransactionResponse
            var parameters = xml.CreateElement("at", "Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
            //var parameters = xml.CreateElement("Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
            ApplyTransactionResponse.AppendChild(parameters);


            foreach (var data in DataArray.Parameters)
            {
                //<goa:Parameter>< goa:Key > PayMode </ goa:Key >< goa:Value > 02 </ goa:Value ></ goa:Parameter > generate this type of xml

                XmlElement parameterElement = xml.CreateElement("goa", "Parameter", "http://cps.huawei.com/cpsinterface/goa");
                //XmlElement parameterElement = xml.CreateElement("Parameter", "http://cps.huawei.com/cpsinterface/goa");
                parameters.AppendChild(parameterElement);
                // Create Key element
                XmlElement keyElement = xml.CreateElement("goa", "Key", "http://cps.huawei.com/cpsinterface/goa");
                //XmlElement keyElement = xml.CreateElement("Key", "http://cps.huawei.com/cpsinterface/goa");
                keyElement.InnerText = data.Key;


                XmlElement valueElement = xml.CreateElement("goa", "Value", "http://cps.huawei.com/cpsinterface/goa");
                //XmlElement valueElement = xml.CreateElement("Value", "http://cps.huawei.com/cpsinterface/goa");
                valueElement.InnerText = data.Value;

                // Append Key and Value elements to Parameter element
                parameterElement.AppendChild(keyElement);
                parameterElement.AppendChild(valueElement);

                // Append Parameter element to XmlDocument
                //parameterElement.AppendChild(parameterElement);



                //var parameter = xml.CreateElement("goa", "Parameter", "http://cps.huawei.com/cpsinterface/goa");
                //parameters.AppendChild(parameter);
                //var Key = xml.CreateElement("goa", "Key");
                //Key.InnerText = data.Key;
                //parameter.AppendChild(Key);
                //var Value = xml.CreateElement("goa", "Value");
                //Value.InnerText = data.Value;
                //parameter.AppendChild(Value);
            }

            //res.Body.ApplyTransactionResponse = datalist;
            //_logger.LogInformation("Check Response values are " + res.ToJson() + "");
            return xml;
        }
        catch (Exception ex)
        {

            _logger.LogError("Issue in Channel Controller Validate method" + ex.InnerException + " and request values are " + req + "");
            return xml;
        }
    }
}
