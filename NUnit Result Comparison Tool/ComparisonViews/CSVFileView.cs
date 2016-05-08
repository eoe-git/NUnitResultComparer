using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_Result_Comparison_Tool.ComparisonViews
{
    class CSVFileView
    {
        public static void ResultFile(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            string fullFilePath = Program.comparisonResultsFile + ".csv";
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(fullFilePath)) {
                if (Program.showTotals) {
                    Totals(file, comparisonResults);
                }
                if (Program.showChangedResultsOnly) {
                    comparisonResults = ResultComparison.GetChangedComparisonResults(comparisonResults);
                }
                WriteCompareResults(file, comparisonResults);
            }
        }

        private static void Totals(System.IO.StreamWriter file, Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            string totalsHeader = StatCalculation.TotalsCSVHeader();
            string totalsLine = StatCalculation.TotalsCSVLine(comparisonResults);

            file.WriteLine(totalsHeader);
            file.WriteLine(totalsLine);
        }

        private static void WriteCompareResults(System.IO.StreamWriter file, Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            EntryHeader(file);

            Tuple<string, string, string, string, string> entryData;
            foreach (var entry in comparisonResults) {
                string compareResult = entry.Value.Item5;
                string testCaseName = entry.Value.Item1;
                string testFixture = entry.Value.Item2;
                string currentResult = entry.Value.Item4;
                string previousResult = entry.Value.Item3;
                entryData = Tuple.Create(compareResult, testCaseName, testFixture, currentResult, previousResult);
                WriteSingleEntry(file, entryData);
            }
        }

        private static void EntryHeader(System.IO.StreamWriter file)
        {
            string header = "Result, Test Case Name, Test Fixture, Current Result, Previous Result";
            file.WriteLine(header);
        }

        private static void WriteSingleEntry(System.IO.StreamWriter file, Tuple<string, string, string, string, string> entryData)
        {
            string line = string.Format(string.Format("{0}, {1}, {2}, {3}, {4}", 
                entryData.Item1, entryData.Item2, entryData.Item3, entryData.Item4, entryData.Item5));
            file.WriteLine(line);
        }
    }
}
