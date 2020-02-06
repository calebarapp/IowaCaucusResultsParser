using System;
using System.IO;

namespace IowaCaucusResultsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            string csv = new ResultsParser(@"C:\Users\caleb\source\repos\IowaCaucusResultsParser\IDP Caucus 2020.html").ParseHtml();

            string path = Environment.CurrentDirectory + "\\IowaCaucusResults.csv";
            File.Create(path).Close();
            File.WriteAllText(path,csv);
        }
    }
}
