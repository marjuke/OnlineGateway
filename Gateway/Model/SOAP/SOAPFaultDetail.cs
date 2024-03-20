using System.Xml.Serialization;
using DotNetSOAPStarter.Model;
namespace SOAP.Model
{
    public partial class SOAPFaultDetail
    {
        // Custom Fault Detail Propoerties
        public Error[]? errors { get; set; }
        [XmlArrayItem("message")]
        public string?[]? messages { get; set; }

    }
}

namespace DotNetSOAPStarter.Model
{
    [XmlInclude(typeof(InputValidationError))]
    [XmlInclude(typeof(BusinessRuleError))]
    public class Error
    {
        public string? Message { get; set; }
    }

    public class InputValidationError : Error
    {
        public string? FieldName { get; set; }
    }

    public class BusinessRuleError : Error
    {
        public string? RuleName { get; set; }
    }

}