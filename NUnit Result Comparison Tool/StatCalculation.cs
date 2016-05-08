using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUnit_Result_Comparison_Tool
{
    class StatCalculation
    {
        public static string TotalsTXT(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparedResults)
        {
            Tuple<int, int, int, int> totalsStats = GetTotalsStats(comparedResults);
            int totalCount = totalsStats.Item1;
            int regressionCount = totalsStats.Item2;
            int fixedCount = totalsStats.Item3;
            int changedCount = totalsStats.Item4;

            string totals = string.Format("Total Test Cases ({0}):  Regressions ({1}), Fixed ({2}), Changed ({3})",
                    totalCount, regressionCount, fixedCount, changedCount);

            return totals;
        }

        public static string TotalsCSVHeader()
        {
            string header = "Total Test Cases, Regressions, Fixed, Changed";
            return header;
        }

        public static string TotalsCSVLine(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparedResults)
        {
            Tuple<int, int, int, int> totalsStats = GetTotalsStats(comparedResults);
            int totalCount = totalsStats.Item1;
            int regressionCount = totalsStats.Item2;
            int fixedCount = totalsStats.Item3;
            int changedCount = totalsStats.Item4;

            string totals = string.Format("{0}, {1}, {2}, {3}", totalCount, regressionCount, fixedCount, changedCount);

            return totals;
        }

        private static Tuple<int, int, int, int> GetTotalsStats(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparedResults)
        {
            int totalCount, fixedCount, regressionCount, changedCount;
            totalCount = fixedCount = regressionCount = changedCount = 0;

            foreach (var entry in comparedResults) {
                string comparisonResult = entry.Value.Item5;
                bool resultChanged = entry.Value.Item6;
                if (comparisonResult == "Ignored") {
                    continue;
                }
                if (resultChanged) {
                    changedCount++;
                }
                if (comparisonResult == "Fixed") {
                    fixedCount++;
                }
                else if (comparisonResult == "Regression") {
                    regressionCount++;
                }
                totalCount++;
            }

            Tuple<int, int, int, int> totalsStats = Tuple.Create(totalCount, regressionCount, fixedCount, changedCount);
            return totalsStats;
        }
    }
}
