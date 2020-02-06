using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IowaCaucusResultsParser
{
    class ResultsParser
    {       
        public string ResultsHtml { get; set; }

        private void setResultsHTML(string filepath) 
        {
            ResultsHtml = File.ReadAllText(filepath);
        }

        public ResultsParser(string filepath) 
        {
            setResultsHTML(filepath);
        }

        public string ParseHtml() 
        {
            string csv = "";

            if (string.IsNullOrEmpty(ResultsHtml))
                return "";
            //countiesBlocksFromHtml(ResultsHtml);
            var html = new HtmlDocument();
            html.LoadHtml(ResultsHtml);
            //Parse headers

            var upperHeadersRow = html.DocumentNode.Descendants()
                                            .Where(x => x.Attributes["Class"]?.Value == "thead")
                                            ?.FirstOrDefault();
            List<string> upperHeaders = upperHeadersRow.Descendants()
                                                .Where(x => x.Name == "li")
                                                .Select(x => x.InnerHtml).ToList();
            csv += string.Join(",", upperHeaders) + Environment.NewLine;

            //subheadings
            var lowerheadersRows = html.DocumentNode.Descendants()
                                            .Where(x => x.Attributes["Class"]?.Value == "sub-head")
                                            ?.FirstOrDefault();
            List<string> lowerHeaders = lowerheadersRows.Descendants()
                                                .Where(x => x.Name == "li")
                                                .Select(x => x.InnerHtml).ToList();
            csv += string.Join(",", lowerHeaders);

            // parse data
            var counties = html.DocumentNode.Descendants()
                                            .Where(x => x.Attributes.Contains("Class") && x.Attributes["Class"].Value.Contains("precinct-rows"));
            
            foreach (var county in counties) 
            {
                string countyName = county.Descendants().Where(x => x.Attributes["Class"]?.Value == "precinct-county").FirstOrDefault().InnerText;

                var precinctData = county.Descendants()
                                    .Where(x => x.Attributes["Class"]?.Value == "precinct-data")
                                    .FirstOrDefault().Descendants()
                                    .Where(x => x.Name == "li")
                                    .Select(x => x.InnerText)
                                    .ToList();

                foreach (string column in precinctData)
                {
                    float colData = 0;
                    if (float.TryParse(column, out colData))
                        csv += colData;
                    else
                        csv += Environment.NewLine + countyName + "," + column;
                    csv += ",";
                }
            }
            return csv;
        }



        private HtmlNode countiesFromHtml(HtmlDocument html, string xpath)
        {
            return html.DocumentNode.SelectSingleNode(xpath);
        }
    }
}
