using System.Xml;

namespace Gateway.Model.ErrorResponse
{
    public static class XmlErrorResponse
    {
        public static XmlDocument GetXmlErrorResponse(string errorCode, string errorMessage)
        {
            var xmlDocument = new XmlDocument();
            //var xmlDeclaration = xmlDocument.CreateXmlDeclaration("1.0", "UTF-16", null);
            //xmlDocument.AppendChild(xmlDeclaration);

            var root = xmlDocument.CreateElement("ApplyTransactionResponse");
            //add namespace xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns: xsi = "http://www.w3.org/2001/XMLSchema-instance" to root element
            root.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");

            xmlDocument.AppendChild(root);

            var code = xmlDocument.CreateElement("ResponseCode");
            code.InnerText = errorCode;
            code.SetAttribute("xmlns", "http://cps.huawei.com/cpsinterface/goa/at");
            root.AppendChild(code);

            var message = xmlDocument.CreateElement("ResponseDesc");
            message.SetAttribute("xmlns", "http://cps.huawei.com/cpsinterface/goa/at");
            message.InnerText = errorMessage;
            root.AppendChild(message);

            return xmlDocument;
        }   
    }
}
