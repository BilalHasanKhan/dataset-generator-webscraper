using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web.UI;

namespace ds.DataDictionary
{
    public class ddContracts
    {
        public const string En = "1";
        public const string Fr = "2";

        public System.IO.Stream pTXT(string langID, string filename)
        {
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            if (langID.Equals(Fr))
                processpTXTData(sb1, Fr, "fr-CA", filename);
            else
                processpTXTData(sb1, En, "en-CA", filename);

            byte[] resultBytes = Encoding.UTF8.GetBytes(sb1.ToString());
            return new MemoryStream(resultBytes);
        }

        public System.Text.StringBuilder processpTXTData(System.Text.StringBuilder sb1, string langID, string culture, string filename)
        {
            string header = Resources.Contracts.ddHeader;

            sb1.Append(header);
            sb1.Append(System.Environment.NewLine);

            if (filename.Contains("Contracts"))
            {
                sb1.Append("\"" + Resources.Contracts.ddDate + "\"" + ',' + "\"" + Resources.Contracts.ddDateDescription + "\"" + ',' + "\"" + "date" + "\"" + ',' + "\"" + 10 + "\"");
                sb1.Append(System.Environment.NewLine);
                sb1.Append("\"" + Resources.Contracts.ddVendorName + "\"" + ',' + "\"" + Resources.Contracts.ddVendorNameDescription + "\"" + ',' + "\"" + "char" + "\"" + ',' + "\"" + 50 + "\"");
                sb1.Append(System.Environment.NewLine);
                sb1.Append("\"" + Resources.Contracts.ddDescription + "\"" + ',' + "\"" + Resources.Contracts.ddDescriptionDescription + "\"" + ',' + "\"" + "char" + "\"" + ',' + "\"" + 100 + "\"");
                sb1.Append(System.Environment.NewLine);
                sb1.Append("\"" + Resources.Contracts.ddValue + "\"" + ',' + "\"" + Resources.Contracts.ddValueDescription + "\"" + ',' + "\"" + "char" + "\"" + ',' + "\"" + 25 + "\"");
            }
            else
            {
                sb1.Append("\"" + Resources.Contracts.ddDate + "\"" + ',' + "\"" + Resources.Contracts.ddDateDescription + "\"" + ',' + "\"" + "date" + "\"" + ',' + "\"" + 10 + "\"");
                sb1.Append(System.Environment.NewLine);
                sb1.Append("\"" + Resources.Contracts.ddRecipientName + "\"" + ',' + "\"" + Resources.Contracts.ddRecipientNameDescription + "\"" + ',' + "\"" + "char" + "\"" + ',' + "\"" + 250 + "\"");
                sb1.Append(System.Environment.NewLine);
                sb1.Append("\"" + Resources.Contracts.ddLocation + "\"" + ',' + "\"" + Resources.Contracts.ddLocationDescription + "\"" + ',' + "\"" + "char" + "\"" + ',' + "\"" + 100 + "\"");
                sb1.Append(System.Environment.NewLine);
                sb1.Append("\"" + Resources.Contracts.ddValue + "\"" + ',' + "\"" + Resources.Contracts.ddValueDescription2 + "\"" + ',' + "\"" + "char" + "\"" + ',' + "\"" + 25 + "\"");
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = "text";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "attachment; filename=" + filename + ".txt");

            return sb1;
        }
    }
}
