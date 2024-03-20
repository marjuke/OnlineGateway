using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Domain.ResponseC;
public class Parameter
{
    public string? Key { get; set; }
    public string? Value { get; set; }

    //public int Version { get; set; }
    //public string? CommandID { get; set; }
    //public string? ConversationID { get; set; }
    //public DateTime TimeStamp { get; set; }
}
