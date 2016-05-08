using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_Result_Comparison_Tool.ComparisonViews
{
    class TXTFileView
    {
        static int spacing = 2;

        public static void ResultFile(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            string fullFilePath = Program.comparisonResultsFile + ".txt";
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
            string totalsLine = StatCalculation.TotalsTXT(comparisonResults);
            file.WriteLine(totalsLine);
            UnderliningColumns(file, totalsLine.Length, 0);
            MoveDownSpaces(file, 2);
        }

        private static Tuple<int, int, int, int ,int> GetColumnHeaderWidth(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            Tuple<int, int, int, int, int> columnHeaderWidths;
            // the 15 number is arbitray as long as it is longer than the largest result (currenly: "Inconclusive")
            int theseShouldBeSame = 15;

            int resultColumnWidth = theseShouldBeSame;
            int nameColumnWidth = comparisonResults.Values.Select(x => x.Item1).Aggregate("", (longest, current) => longest.Length > current.Length ? longest : current).Length;
            int fixtureColumnWidth = comparisonResults.Values.Select(x => x.Item2).Aggregate("", (longest, current) => longest.Length > current.Length ? longest : current).Length;
            int currentResultColumnWidth = theseShouldBeSame;
            int previousColumnWidth = theseShouldBeSame;

            columnHeaderWidths = Tuple.Create(resultColumnWidth, nameColumnWidth, fixtureColumnWidth, currentResultColumnWidth, previousColumnWidth);
            return columnHeaderWidths;
        }

        private static void ColumnHeaders(System.IO.StreamWriter file, Tuple<int, int, int, int ,int> columnWidths)
        {
            FirstColumnEntry(file, columnWidths.Item1, "Result");
            SingleColumnEntry(file, columnWidths.Item2, "Test Case Name");
            SingleColumnEntry(file, columnWidths.Item3, "Test Fixture");
            SingleColumnEntry(file, columnWidths.Item4, "Current Result");
            SingleColumnEntry(file, columnWidths.Item5, "Previous Result");

            MoveDownSpaces(file, 1);
        }

        private static void WriteCompareResults(System.IO.StreamWriter file, Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {

            Tuple<int, int, int, int, int> columnWidths = GetColumnHeaderWidth(comparisonResults);
            ColumnHeaders(file, columnWidths);

            UnderliningColumns(file, columnWidths.Item1, spacing);
            UnderliningColumns(file, columnWidths.Item2, spacing);
            UnderliningColumns(file, columnWidths.Item3, spacing);
            UnderliningColumns(file, columnWidths.Item4, spacing);
            UnderliningColumns(file, columnWidths.Item5, spacing);
            MoveDownSpaces(file, 1);


            foreach (var entry in comparisonResults) {
                FirstColumnEntry(file, columnWidths.Item1, entry.Value.Item5);
                SingleColumnEntry(file, columnWidths.Item2, entry.Value.Item1);
                SingleColumnEntry(file, columnWidths.Item3, entry.Value.Item2);
                SingleColumnEntry(file, columnWidths.Item4, entry.Value.Item4);
                
                SingleColumnEntry(file, columnWidths.Item5, entry.Value.Item3);
                MoveDownSpaces(file, 1);
            }
        }

        private static void FirstColumnEntry(System.IO.StreamWriter file, int columnWidth, string value)
        {
            file.Write(string.Format(value).PadRight(columnWidth));
        }

        private static void SingleColumnEntry(System.IO.StreamWriter file, int columnWidth, string value)
        {
            file.Write(string.Format(LineSpaces(spacing) + value).PadRight(columnWidth + spacing));
        }

        private static void UnderliningColumns(System.IO.StreamWriter file, int columnWidth, int separation)
        {
            for (int i = 0; i < columnWidth + separation; i++) {
                if (i >= columnWidth) {
                    file.Write(" ");
                    continue;
                }
                file.Write("=");
            }
        }

        private static string LineSpaces(int spaces)
        {
            string lineSpaces = "";
            for (int i = 0; i < spaces; i++) {
                lineSpaces = lineSpaces + " ";
            }
            return lineSpaces;
        }

        private static void MoveDownSpaces(System.IO.StreamWriter file, int numberOfSpaces)
        {
            for (int i = 0; i < numberOfSpaces; i++) {
                file.WriteLine("");
            }
        }
    }
}
