﻿using SOAP;
using System.Xml.Serialization;

namespace Gateway.Model.bKashRequest
{
    public class Parameters
    {
        [XmlElement("Parameter", Namespace = SOAPConstants.SOAPNamespacegoa)]
        public Parameter[] Parameter { get; set; }
    }
}
