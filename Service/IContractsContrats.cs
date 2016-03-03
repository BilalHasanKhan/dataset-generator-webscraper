using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Xml.Serialization;
using System.Xml;

namespace ds.Service
{
    [ServiceContract]
    [XmlSerializerFormat]

    public interface IContractsContrats
    {
        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "dsXMLEn?v1={url}&v2={yr}&v3={q}")]
        XmlDocument dsXMLEn(string url, string yr, string q);

        [OperationContract]
        [WebInvoke(Method = "GET", RequestFormat = WebMessageFormat.Xml, ResponseFormat = WebMessageFormat.Xml, BodyStyle = WebMessageBodyStyle.Bare, UriTemplate = "dsXMLFr?v1={url}&v2={yr}&v3={q}")]
        XmlDocument dsXMLFr(string url, string yr, string q);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "dsCSV?v1={url}&v2={yr}&v3={q}&v4={langID}")]
        System.IO.Stream dsCSV(string url, string yr, string q, string langID);

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "dsTXT?v1={langID}")]
        System.IO.Stream dsTXT(string langID);
    }
}
