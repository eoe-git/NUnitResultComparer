using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;
using System.Collections.Specialized;

namespace NUnit_Result_Comparison_Tool
{
    class Program
    {
        public static string comparisonResultsFile = ConfigurationManager.AppSettings["ComparisonResultPath"];
        public static string currentResultsPath = ConfigurationManager.AppSettings["CurrentResult"];
        public static string previousResultsPath = ConfigurationManager.AppSettings["PreviousResult"];
        public static string comparisonView = ConfigurationManager.AppSettings["ComparisonView"];
        public static bool showChangedResultsOnly = Boolean.Parse(ConfigurationManager.AppSettings["ShowChangedResultsOnly"]);
        public static bool showTotals = Boolean.Parse(ConfigurationManager.AppSettings["ShowTotals"]);
        public static bool isNUnit3 = Boolean.Parse(ConfigurationManager.AppSettings["IsNUnit3"]);

        static void Main(string[] args)
        {
            ResultsFile.ResultFile();
        }
    }
}
