using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_Result_Comparison_Tool.ComparisonViews
{
    class CmdLineView
    {
        public static void ResultFile(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            if (Program.showTotals) {
                Totals(comparisonResults);
            }
            if (Program.showChangedResultsOnly) {
                comparisonResults = ResultComparison.GetChangedComparisonResults(comparisonResults);
            }
            WriteCompareResults(comparisonResults);
            Console.ReadKey();
        }

        private static void Totals(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            string totalsLine = StatCalculation.TotalsTXT(comparisonResults); ;
            Console.WriteLine(totalsLine);
            MoveDownSpaces(1);
        }

        private static void WriteCompareResults(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            EntryHeader();

            Tuple<string, string, string, string, string> entryData;
            foreach (var entry in comparisonResults) {
                string compareResult = entry.Value.Item5;
                string testCaseName = entry.Value.Item1;
                string testFixture = entry.Value.Item2;
                string currentResult = entry.Value.Item4;
                string previousResult = entry.Value.Item3;
                entryData = Tuple.Create(compareResult, testCaseName, testFixture, currentResult, previousResult);
                WriteSingleEntry(entryData);
            }
        }

        private static void EntryHeader()
        {
            string header = "Result, Test Case Name, Test Fixture, Current Result, Previous Result";
            Console.WriteLine(header);
            MoveDownSpaces(1);
        }

        private static void WriteSingleEntry(Tuple<string, string, string, string, string> entryData)
        {
            string line = string.Format(string.Format("{0}, {1}, {2}, {3}, {4}",
                entryData.Item1, entryData.Item2, entryData.Item3, entryData.Item4, entryData.Item5));
            Console.WriteLine(line);
        }

        private static void MoveDownSpaces(int numberOfSpaces)
        {
            for (int i = 0; i < numberOfSpaces; i++) {
                Console.WriteLine("");
            }
        }
    }
}
