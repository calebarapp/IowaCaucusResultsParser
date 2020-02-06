using HtmlAgilityPack;
using System.Collections.Generic;

namespace IowaCaucusResultsParser
{   
    
    class ElectionResults
    {

        private string _countyNameXpath = "//*[contiains(@class, 'precinct-county')] div";
        private string _precintDataXPath = "//*[contiains(@class, 'precinct-data')] ul";

        public struct County
        {
            public string Name { get; set; }
            public PrecinctRow precints { get; set; }
        }

        public List<County> Counties { get; set; }

        public void AddCountyHtml(HtmlNode countyNode) 
        {
            County county = new County();
            county.Name = countyNode.SelectSingleNode(_countyNameXpath).InnerText;

            var precincts = countyNode.SelectNodes(_precintDataXPath);
            
            foreach (var precinctNode in precincts) 
            {
                HtmlNodeCollection columns = precinctNode.SelectNodes("li");

            }
                                

        }
    }

    public class PrecinctRow
    {
        public string Precinct { get; set; }
        public CandidateStats Candidates { get; set; }
    }

    public class CandidateStats
    {
        public double FirstExpression { get; set; }
        public double FinalExpression { get; set; }
        public double SDE { get; set; }
    }
}