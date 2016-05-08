using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace NUnit_Result_Comparison_Tool
{
    class ResultsFile
    {

        public static void ResultFile()
        {
            var ComparisonResults = ResultComparison.GetComparisonResults();
            if (Program.comparisonView.Equals("all", StringComparison.InvariantCultureIgnoreCase)) {
                ComparisonViews.TXTFileView.ResultFile(ComparisonResults);
                ComparisonViews.CSVFileView.ResultFile(ComparisonResults);
                ComparisonViews.CmdLineView.ResultFile(ComparisonResults);
            }
            else if (Program.comparisonView.Equals("cmd", StringComparison.InvariantCultureIgnoreCase)) {
                ComparisonViews.CmdLineView.ResultFile(ComparisonResults);
            }
            else if (Program.comparisonView.Equals("csv", StringComparison.InvariantCultureIgnoreCase)) {
                ComparisonViews.CSVFileView.ResultFile(ComparisonResults);
            }
            else if (Program.comparisonView.Equals("txt", StringComparison.InvariantCultureIgnoreCase)) {
                ComparisonViews.TXTFileView.ResultFile(ComparisonResults);
            }
            else {
                ComparisonViews.CmdLineView.ResultFile(ComparisonResults);
            }
        }
        
    }
        
}
