using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace ds.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ConractsContrats" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ConractsContrats.svc or ConractsContrats.svc.cs at the Solution Explorer and start debugging.
    public class ContractsContrats : IContractsContrats
    {
        public const string En = "1";
        public const string Fr = "2";

        String h1Tag = String.Empty;

        CultureInfo cultureEn = CultureInfo.CreateSpecificCulture("en-CA");
        CultureInfo cultureFr = CultureInfo.CreateSpecificCulture("fr-CA");

        public XmlDocument dsXMLEn(string url, string yr, string q)
        {
            XmlDocument doc = writeXML(url + "&yr=" + yr + "&q=" + q, q, En);

            return doc;
        }

        public XmlDocument dsXMLFr(string url, string yr, string q)
        {
            XmlDocument doc = writeXML(url + "&yr=" + yr + "&q=" + q, q, Fr);

            return doc;
        }


        public XmlDocument writeXML(string url, string quarter, string langid)
        {
            XmlNode rootNode;
            XmlNode node1, node2, node3, node4;
            String _quarter = String.Empty;
            String _topLevelNode = String.Empty;
            String _column = String.Empty;
            String _header1, _header2, _header3, _header4;
            int rowCounter = 0;

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            List<Classes.DataColumns> genericResults = new List<Classes.DataColumns>();
            ScraperClasses.Contracts sc = new ScraperClasses.Contracts();
            sc.getHTMLContents(genericResults, url);

            XmlNode topLevelNode = doc.CreateElement(Resources.Contracts.topLevelNode);
            doc.AppendChild(topLevelNode);

            // the first row contains the header information and is used to build the xml nodes
            _header1 = genericResults[0].column1;
            _header2 = genericResults[0].column2;
            _header3 = genericResults[0].column3;
            _header4 = genericResults[0].column4;

            foreach (Classes.DataColumns s in genericResults)
            {
                if (!rowCounter++.Equals(0))
                {
                    rootNode = doc.CreateElement(getRootNode(quarter));
                    topLevelNode.AppendChild(rootNode);

                    node1 = doc.CreateElement(_header1);
                    node1.AppendChild(doc.CreateTextNode(s.column1));
                    rootNode.AppendChild(node1);

                    node2 = doc.CreateElement(_header2);
                    node2.AppendChild(doc.CreateTextNode(s.column2));
                    rootNode.AppendChild(node2);

                    node3 = doc.CreateElement(_header3);
                    node3.AppendChild(doc.CreateTextNode(s.column3));
                    rootNode.AppendChild(node3);

                    node4 = doc.CreateElement(_header4);
                    node4.AppendChild(doc.CreateTextNode(s.column4.Replace(",", String.Empty).Replace("$", String.Empty)));
                    rootNode.AppendChild(node4);
                }
            }
            return doc;
        }

        public System.IO.Stream dsCSV(string url, string yr, string quarter, string langID)
        {
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            UTF8Encoding encoding = new UTF8Encoding(false);

            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
            }
            processCSVData(sb1, url + "&yr=" + yr + "&q=" + quarter);
            byte[] resultBytes = Encoding.UTF32.GetBytes(sb1.ToString());

            return new MemoryStream(resultBytes);
        }

        public System.Text.StringBuilder processCSVData(System.Text.StringBuilder sb1, string url)
        {
            List<Classes.DataColumns> genericResults = new List<Classes.DataColumns>();
            ScraperClasses.Contracts sc = new ScraperClasses.Contracts();
            int rowCounter = 0;

            sc.getHTMLContents(genericResults, url);
            sb1.Append(genericResults[0].column1 + ",");
            sb1.Append(genericResults[0].column2 + ",");
            sb1.Append(genericResults[0].column3 + ",");
            sb1.Append(genericResults[0].column4);
            sb1.Append(System.Environment.NewLine);

            foreach (Classes.DataColumns s in genericResults)
            {
                if (!rowCounter++.Equals(0))
                {
                    sb1.Append(String.Format("\"{0}\"", s.column1));
                    sb1.Append(",");
                    sb1.Append(String.Format("\"{0}\"", s.column2));
                    sb1.Append(",");
                    sb1.Append(String.Format("\"{0}\"", s.column3));
                    sb1.Append(",");
                    sb1.Append(String.Format("\"{0}\"", s.column4));
                    sb1.Append(System.Environment.NewLine);
                }
            }
                            
            WebOperationContext.Current.OutgoingResponse.ContentType = "text/csv";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "attachment; filename=" + Resources.Contracts.filename + ".csv");

            return sb1;
        }

        public System.IO.Stream dsTXT(string langID)
        {
            DataDictionary.ddContracts dd = new DataDictionary.ddContracts();
            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
            }
            return (dd.pTXT(langID, Resources.Contracts.filename));
        }

        string getRootNode (string quarter)
        {
            switch (quarter)
            {
                case "1":
                    return Resources.Contracts.q1;
                case "2":
                    return Resources.Contracts.q2;
                case "3":
                    return Resources.Contracts.q3;
                case "4":
                    return Resources.Contracts.q4;
            }
            return "";
        }   
    
    }
}
