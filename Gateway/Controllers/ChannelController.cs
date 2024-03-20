using Microsoft.AspNetCore.Mvc;
using SOAP.Model;
using SOAP;
using SOAP.Controllers;

using Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Application.ThirdPartyChannel;


using Domain.RequestC;
using Gateway.MappingArray;
using Microsoft.Extensions.Caching.Memory;
using Gateway.Model.Password;
using Application.StanService;
using NuGet.Protocol;
using System.Text;
using System.Xml;
using Gateway.Model.ErrorResponse;
using System.Xml.Linq;
using Gateway.Model;

namespace Gateway.Controllers;

[ApiController]
[SOAPController(SOAPVersion.v1_2)]
public class ChannelController : SOAPControllerBase
{
    private readonly ILogger<ChannelController> _logger;
    private readonly UserManager<AppUser> _userManager;

    public ChannelController(UserManager<AppUser> userManager, ILogger<ChannelController> logger, IWebHostEnvironment env) : base(logger, env)
    {
        _logger = logger;
        _userManager = userManager;
    }

    [HttpPost]
    //[PayloadRequired]
    [Consumes("application/xml")]
    //[Produces("application/soap+xml")]
    public async Task<ContentResult> OperationSelector(SOAP1_2RequestEnvelope env)
    {

        _logger.LogInformation("OperationSelectior is called ");
        await Mediator.Send(new StanUpdate.Command { Stan = "OK" });
        if (!env.Body.ApplyTransactionRequest.Header.LoginID.IsNullOrEmpty())
        {
            var user = _userManager.Users.Where(s => s.UserID == env.Body.ApplyTransactionRequest.Header.LoginID).FirstOrDefault();
            if (user != null)
            {
                string Password = "";
                try
                {
                    Password = PasswordDecryptor.GetPassword(env.Body.ApplyTransactionRequest.Header.Password);
                }
                catch (Exception)
                {

                    return new ContentResult
                    {
                        Content = xmltostring(XmlErrorResponse.GetXmlErrorResponse("BEC0006", "Invalid Password")),
                        ContentType = "application/soap+xml",
                    };
                }
                    
                var result = await _userManager.CheckPasswordAsync(user, Password);
                _logger.LogInformation("Login");
                if (result)
                {
                    var contentResult = new ContentResult
                    {
                        
                        ContentType = "application/soap+xml",

                    };
                    _logger.LogInformation("Login successfull");
                    if (env?.Body?.ApplyTransactionRequest is not null)
                    {
                        if (env.Body.ApplyTransactionRequest.Header.CommandID.Trim() == "CheckInformation")
                        {

                            var soapXmlString = await GetApplyTransactionRequest(env.Body.ApplyTransactionRequest);
                            contentResult.Content = soapXmlString;
                        }
                        else if (env.Body.ApplyTransactionRequest.Header.CommandID.Trim() == "ValidationPayment")
                        {
                            var soapXmlString = await GetApplyValidationRequest(env.Body.ApplyTransactionRequest);
                            contentResult.Content = soapXmlString;
                        }
                        else if (env.Body.ApplyTransactionRequest.Header.CommandID.Trim() == "PaymentConfirmation")
                        {
                            var soapXmlString = await GetApplyPaymentConfirmation(env.Body.ApplyTransactionRequest);
                            contentResult.Content = soapXmlString;
                        }
                        else
                        {
                            _logger.LogInformation("Command not specified " + env + "");
                            var soapXmlString = xmltostring( XmlErrorResponse.GetXmlErrorResponse("BEC0004", "Invalid CommandID"));
                            contentResult.Content = soapXmlString;
                        }
                        var responseSetting = Convert.ToInt16(new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["ControllerResponse"]);
                        if (responseSetting == 1)
                            ResponseSaveToFile(contentResult.Content);

                        return contentResult;
                    }
                    else
                    {
                        _logger.LogInformation("No data found " + env + "");
                        var soapXmlString = xmltostring(XmlErrorResponse.GetXmlErrorResponse("BECP003", "Sorry, no data found in your requested time"));
                        contentResult.Content = soapXmlString;
                        return contentResult;
                    }
                }
                else
                {
                    _logger.LogInformation("User not authorized " + env + "");
                    return new ContentResult
                    {
                        Content = xmltostring(XmlErrorResponse.GetXmlErrorResponse("BEC0006", "Invalid Password")),
                        ContentType = "application/soap+xml",
                    };
                }
            }
            else
            {
                _logger.LogInformation("User not found " + env + "");
                return new ContentResult
                {
                    Content = xmltostring(XmlErrorResponse.GetXmlErrorResponse("BEC0006", "Invalid Password")),
                    ContentType = "application/soap+xml",
                };
            }
        }
        else
        {
            _logger.LogInformation("User required " + env + "");
            return new ContentResult
            {
                Content = xmltostring(XmlErrorResponse.GetXmlErrorResponse("BEC0006", "Invalid Password")),
                ContentType = "application/soap+xml",
            };
        }
    }


    protected async Task<string> GetApplyTransactionRequest(Model.bKashRequest.ApplyTransactionRequest req)
    {
        var xml = new XmlDocument();
        //content type withouth charset
        try
        {
            var ConversationID = req.Header.ConversationID;
            var command = req.Header.CommandID;
            var timestamp = req.Header.Timestamp;
            var version = req.Header.Version;
            var loginID = req.Header.LoginID;
            var Password = req.Header.Password;

            //var ParentId = req.Body.Parameters.Parameter.Where(s => s.Key == "InitiatorID").FirstOrDefault();
            //var d = ParentId.Value.ToString();

            ReqCheckInformation myObject = new ReqCheckInformation();

            // Map the array to the class
            MappingArrayToClass.MapArrayToClass(req.Body.Parameters.Parameter, myObject);
            _logger.LogInformation("Check Request values are " + myObject.ToJson() + "");
            var DataArray = await Mediator.Send(new CheckInformation.Query { AccountID = myObject.AccountID, InitiatorId = myObject.InitiatorID, RequesterMSISDN = myObject.RequesterMSISDN, Timestamp = timestamp, Version = version.ToString(), ConversationID = ConversationID, LoginID = loginID, Password = Password, Command = command, reqCheckInformation = myObject });
            _logger.LogInformation("Check response before envelope " + DataArray.ToJson() + "");
            var datalist = new Model.ApplyTransactionResponse();
            datalist.ResponseCode = DataArray.ResponseCode;
            datalist.ResponseDesc = DataArray.ResponseDesc;
            datalist.Parameters = DataArray.Parameters;

            if (DataArray.ResponseCode == "0")
            {
                //produce utf-16 xml
                xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-16", null));
                var root = xml.CreateElement("soap", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
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
                var header = xml.CreateElement("soap", "Header", "http://www.w3.org/2003/05/soap-envelope");
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
                var body = xml.CreateElement("soap", "Body", "http://www.w3.org/2003/05/soap-envelope");
                root.AppendChild(body);
                var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://tempuri.org/");
                body.AppendChild(ApplyTransactionResponse);
                var responseCode = xml.CreateElement("ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
                responseCode.InnerText = datalist.ResponseCode;
                ApplyTransactionResponse.AppendChild(responseCode);
                var responseDescription = xml.CreateElement("ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
                responseDescription.InnerText = datalist.ResponseDesc;
                ApplyTransactionResponse.AppendChild(responseDescription);
                var parameters = xml.CreateElement("Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
                ApplyTransactionResponse.AppendChild(parameters);


                foreach (var data in DataArray.Parameters)
                {
                    XmlElement parameterElement = xml.CreateElement("Parameter", "http://cps.huawei.com/cpsinterface/goa");
                    parameters.AppendChild(parameterElement);
                    // Create Key element
                    XmlElement keyElement = xml.CreateElement("Key", "http://cps.huawei.com/cpsinterface/goa");
                    keyElement.InnerText = data.Key;
                    XmlElement valueElement = xml.CreateElement("Value", "http://cps.huawei.com/cpsinterface/goa");
                    valueElement.InnerText = data.Value;
                    // Append Key and Value elements to Parameter element
                    parameterElement.AppendChild(keyElement);
                    parameterElement.AppendChild(valueElement);
                }

                return xmltostring(xml);
            }
            else
            {
                return xmltostring(XmlErrorResponse.GetXmlErrorResponse(DataArray.ResponseCode, DataArray.ResponseDesc));
            }


        }
        catch (Exception ex)
        {

            _logger.LogError("Issue in Channel Controller check method" + ex.InnerException.Message.ToArray() + " and request values are " + req.ToJson() + "");

            return xmltostring(XmlErrorResponse.GetXmlErrorResponse("BECP005", "Sorry, an error occurred"));
            //return xmltostring(xml);
        }

    }
    protected async Task<string> GetApplyValidationRequest(Model.bKashRequest.ApplyTransactionRequest req)
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
            ReqValidationPayment myObject = new ReqValidationPayment();
            // Map the array to the class
            MappingArrayToClass.MapArrayToClass(req.Body.Parameters.Parameter, myObject);
            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5),
            };
            _logger.LogInformation("Validation Request values are " + myObject.ToJson() + "");
            var DataArray = await Mediator.Send(new ValidationPayment.Query { AccountID = myObject.AccountID, InitiatorId = myObject.InitiatorID, RequesterMSISDN = myObject.RequesterMSISDN, Timestamp = timestamp, ActualAmount = myObject.ActualAmount, Version = version.ToString(), ConversationID = ConversationID, LoginID = loginID, Password = Password, Command = command, reqValidationPayment = myObject });
            if (DataArray.ResponseCode == "0")
            {
                //produce utf-16 xml
                xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-16", null));
                var root = xml.CreateElement("soap", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
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
                var header = xml.CreateElement("soap", "Header", "http://www.w3.org/2003/05/soap-envelope");
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
                var body = xml.CreateElement("soap", "Body", "http://www.w3.org/2003/05/soap-envelope");
                root.AppendChild(body);
                var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://tempuri.org/");
                body.AppendChild(ApplyTransactionResponse);
                //add response code and responsedescription in ApplyTransactionResponse
                var responseCode = xml.CreateElement("at", "ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
                responseCode.InnerText = DataArray.ResponseCode;
                ApplyTransactionResponse.AppendChild(responseCode);
                var responseDescription = xml.CreateElement("at", "ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
                responseDescription.InnerText = DataArray.ResponseDesc;
                ApplyTransactionResponse.AppendChild(responseDescription);
                //add Parementers in ApplyTransactionResponse
                var parameters = xml.CreateElement("at", "Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
                ApplyTransactionResponse.AppendChild(parameters);
                foreach (var data in DataArray.Parameters)
                {
                    XmlElement parameterElement = xml.CreateElement("goa", "Parameter", "http://cps.huawei.com/cpsinterface/goa");
                    parameters.AppendChild(parameterElement);
                    // Create Key element
                    XmlElement keyElement = xml.CreateElement("goa", "Key", "http://cps.huawei.com/cpsinterface/goa");
                    keyElement.InnerText = data.Key;
                    XmlElement valueElement = xml.CreateElement("goa", "Value", "http://cps.huawei.com/cpsinterface/goa");
                    valueElement.InnerText = data.Value;
                    // Append Key and Value elements to Parameter element
                    parameterElement.AppendChild(keyElement);
                    parameterElement.AppendChild(valueElement);
                }
                return xmltostring(xml);
            }
            else
            {
                return xmltostring(XmlErrorResponse.GetXmlErrorResponse(DataArray.ResponseCode, DataArray.ResponseDesc));
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError("Issue in Channel Controller confirm method" + ex.InnerException.Message.ToArray() + " and request values are " + req.ToJson() + "");
            return xmltostring(XmlErrorResponse.GetXmlErrorResponse("BECP005", "Sorry, an error occurred"));
        }


    }

    protected async Task<string> GetApplyPaymentConfirmation(Model.bKashRequest.ApplyTransactionRequest req)
    {
        var xml = new XmlDocument();

        try
        {
            var ConversationID = req.Header.ConversationID;
            var command = req.Header.CommandID;
            var version = req.Header.Version;
            var timestamp = req.Header.Timestamp;

            var loginID = req.Header.LoginID;
            var Password = req.Header.Password;
            ReqPaymentConfirmation myObject = new ReqPaymentConfirmation();
            // Map the array to the class
            MappingArrayToClass.MapArrayToClass(req.Body.Parameters.Parameter, myObject);
            _logger.LogInformation("Payment Request values are " + myObject.ToJson() + "");
            var DataArray = await Mediator.Send(new PaymentConfirm.Query { Timestamp = timestamp, Version = version.ToString(), ConversationID = ConversationID, LoginID = loginID, Password = Password, Command = command, reqPaymentConfirmation = myObject });
            if (DataArray.ResponseCode == "0")
            {
                //produce utf-16 xml
                xml.AppendChild(xml.CreateXmlDeclaration("1.0", "UTF-16", null));
                var root = xml.CreateElement("soap", "Envelope", "http://www.w3.org/2003/05/soap-envelope");
                xml.AppendChild(root);
                XmlAttribute xmlnsgoa = xml.CreateAttribute("xmlns:goa");
                xmlnsgoa.Value = "http://cps.huawei.com/cpsinterface/goa";
                root.Attributes.Append(xmlnsgoa);
                XmlAttribute xmlnsat = xml.CreateAttribute("xmlns:at");
                xmlnsat.Value = "http://cps.huawei.com/cpsinterface/goa/at";
                root.Attributes.Append(xmlnsat);
                XmlAttribute xmlnstem = xml.CreateAttribute("xmlns:tem");
                xmlnstem.Value = "http://tempuri.org/";
                root.Attributes.Append(xmlnstem);
                var header = xml.CreateElement("soap", "Header", "http://www.w3.org/2003/05/soap-envelope");
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
                var body = xml.CreateElement("soap", "Body", "http://www.w3.org/2003/05/soap-envelope");
                root.AppendChild(body);
                var ApplyTransactionResponse = xml.CreateElement("tem", "ApplyTransactionResponse", "http://tempuri.org/");
                body.AppendChild(ApplyTransactionResponse);
                //add response code and responsedescription in ApplyTransactionResponse
                var responseCode = xml.CreateElement("at", "ResponseCode", "http://cps.huawei.com/cpsinterface/goa/at");
                responseCode.InnerText = DataArray.ResponseCode;
                ApplyTransactionResponse.AppendChild(responseCode);
                var responseDescription = xml.CreateElement("at", "ResponseDesc", "http://cps.huawei.com/cpsinterface/goa/at");
                responseDescription.InnerText = DataArray.ResponseDesc;
                ApplyTransactionResponse.AppendChild(responseDescription);
                //add Parementers in ApplyTransactionResponse
                var parameters = xml.CreateElement("at", "Parameters", "http://cps.huawei.com/cpsinterface/goa/at");
                ApplyTransactionResponse.AppendChild(parameters);


                foreach (var data in DataArray.Parameters)
                {
                    XmlElement parameterElement = xml.CreateElement("goa", "Parameter", "http://cps.huawei.com/cpsinterface/goa");
                    parameters.AppendChild(parameterElement);
                    // Create Key element
                    XmlElement keyElement = xml.CreateElement("goa", "Key", "http://cps.huawei.com/cpsinterface/goa");
                    keyElement.InnerText = data.Key;
                    XmlElement valueElement = xml.CreateElement("goa", "Value", "http://cps.huawei.com/cpsinterface/goa");
                    valueElement.InnerText = data.Value;
                    // Append Key and Value elements to Parameter element
                    parameterElement.AppendChild(keyElement);
                    parameterElement.AppendChild(valueElement);
                }

                return xmltostring(xml);
            }
            else
            {
                return xmltostring(XmlErrorResponse.GetXmlErrorResponse(DataArray.ResponseCode, DataArray.ResponseDesc));
            }


            
        }
        catch (Exception ex)
        {

            _logger.LogError("Issue in Channel Controller Validate method" + ex.InnerException.Message.ToArray() + " and request values are " + req.ToJson() + "");
            return xmltostring(XmlErrorResponse.GetXmlErrorResponse("BECP005", "Sorry, an error occurred"));
        }
    }


    public string xmltostring(XmlDocument xml)
    {
        string str;
        using (StringWriter sw = new StringWriter())
        {
            using (XmlTextWriter writer = new XmlTextWriter(sw))
            {
                writer.Formatting = Formatting.Indented;
                xml.WriteTo(writer);
                str = sw.ToString();
                //Console.WriteLine(str);
            }
        }
        return str;
    }

    //write response to response file
    public void ResponseSaveToFile(string xml)
    {
        string fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssfff}.xml";

        // Combine the file name with the log directory to get the full file path
        string rootFolder = Directory.GetCurrentDirectory();
        string folderPath = Path.Combine(rootFolder, "ResponseFile");
        string filePath = Path.Combine(folderPath, fileName);
        //var filePath = Path.Combine(@"D:\Logs", fileName); // Specify your desired directory
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        // Write the XML data to the file
        System.IO.File.WriteAllText(filePath, xml);
    }
}
