using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Data;
using System.Xml;
using System.Text;
using System.Reflection;
using System.Xml.Serialization;
using System.Globalization;
using System.Threading;

namespace ds.ScraperClasses
{    
    public class Contracts
    {
        private const RegexOptions ExpressionOptions = RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.IgnoreCase;
        public const string En = "1";
        public const string Fr = "2";

        CultureInfo cultureEn = CultureInfo.CreateSpecificCulture("en-CA");
        CultureInfo cultureFr = CultureInfo.CreateSpecificCulture("fr-CA");

        // The code below scrapes the Disclosure of Contracts Over $10,000 web page: http://www.tbs-sct.gc.ca/scripts/contracts-contrats/reports-rapports-eng.asp?r=l&yr=2015&q=3 
        public List<Classes.DataColumns> getHTMLContents(List<Classes.DataColumns> results, string url)
        {
            String strResult;
            WebResponse objResponse;
            // send a web request to read the url
            WebRequest objRequest = HttpWebRequest.Create(url);
            objResponse = objRequest.GetResponse();

            if (url.Contains("-fra."))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
            }

            // store the contents of the web page into the String strResult
            using (StreamReader sr = new StreamReader(objResponse.GetResponseStream()))
            {
                strResult = sr.ReadToEnd();
                sr.Close();
            }
            // split the strResult String and grab the contents inside the <main tag
            string[] values = strResult.Split(new string[] { "<main", "</main>" }, StringSplitOptions.RemoveEmptyEntries);
            getDataSets(results, values[1]);

            return results;
        }

        List<Classes.DataColumns> getDataSets(List<Classes.DataColumns> results, String HTML)
        {
            string ColumnRegexExpression = String.Empty;                     
            string RowPattern = "<tr[^>]*>(.*?)</tr>";
            string CellPattern = "<th[^>]*>(.*?)</th>|<td[^>]*>(.*?)</td>";

            string headerString = String.Empty;
            int iCurrentColumn = 0;

            MatchCollection rows = Regex.Matches(HTML, RowPattern, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);

            // go through each row  with the <tr tag in the table
            foreach (Match row in rows)
            {                
                iCurrentColumn = 0;
                Classes.DataColumns dc = new Classes.DataColumns();
                MatchCollection cols = Regex.Matches(row.ToString(), CellPattern, RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnoreCase);
                // go through each column  with the <td tag in the table
                // remove unwanted tags                
                foreach (Match col in cols)
                {
                    string _column = col.Value.ToString();
                    if (_column.Contains("<strong>") && _column.Contains("</strong>"))
                    {
                        _column = _column.Split(new string[] { "<strong>", "</strong>" }, StringSplitOptions.RemoveEmptyEntries)[1].Replace(" ", "_").Trim().ToLower();
                    }
                    else if (_column.Contains("<td class='nowrap'>") && _column.Contains("</td>"))
                    {
                        _column = _column.Split(new string[] { "<td class='nowrap'>", "</td>" }, StringSplitOptions.RemoveEmptyEntries)[0];
                    }
                    if (_column.Contains("'>") && _column.Contains("</a></td>"))
                    {
                        _column = _column.Split(new string[] { "'>", "</a></td>" }, StringSplitOptions.RemoveEmptyEntries)[1];
                    }
                    if (_column.Contains("<td class='text-right nowrap'>") && _column.Contains("</td>"))
                    {
                        _column = _column.Replace("<td class='text-right nowrap'>", String.Empty).Replace("</td>", String.Empty);
                    }
                    if (_column.Contains("<td") && _column.Contains("</td>"))
                    {
                        _column = _column.Replace("<td>", String.Empty).Replace("</td>", String.Empty);
                    }
                    _column = _column.Replace("&nbsp;", " ").Replace("&#201;", "É").Replace("&#233;", "&").Replace("&amp;", "&").Replace("<td class=\"nowrap\">", String.Empty);   
                    // store each column read into the dc class
                    switch (++iCurrentColumn)
                    {
                        case 1:
                            dc.column1 = _column;
                            break;
                        case 2:
                            dc.column2 = _column;
                            break;
                        case 3:
                            dc.column3 = _column;
                            break;
                        case 4:
                            dc.column4 = _column;
                            break;
                    }
                }                
                results.Add(dc);                
            }
            return results;
        }
    }
}
