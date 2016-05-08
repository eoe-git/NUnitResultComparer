using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Collections.Specialized;

namespace NUnit_Result_Comparison_Tool
{
    class ResultComparison
    {
        public static Dictionary<string, Tuple<string, string, string, string, string, bool>> GetComparisonResults()
        {
            Dictionary<string, Tuple<string, string, string>> currentFileResults = ReadResults.GetTestCaseInformation(Program.currentResultsPath);
            Dictionary<string, Tuple<string, string, string>> previousFileResults = ReadResults.GetTestCaseInformation(Program.previousResultsPath);

            //Key = TestCasePath, Item1 = TestCaseName, Item2 = TestFixtureName, Item3 = PreviousResult, 
            //                    Item4 = CurrentResult, Item5 = CompareResult, Item6 = IsChanged
            Dictionary<string, Tuple<string, string, string, string, string, bool>> ComparedResults = new Dictionary<string, Tuple<string, string, string, string, string, bool>>();

            foreach (var entry in previousFileResults) {
                Tuple<string, bool> results = CompareResults(currentFileResults[entry.Key].Item3, entry.Value.Item3);
                Tuple<string, string, string, string, string, bool> fullResults = new Tuple<string, string, string, string, string, bool>(
                    entry.Value.Item2, entry.Value.Item1, entry.Value.Item3, currentFileResults[entry.Key].Item3, results.Item1, results.Item2);
                ComparedResults.Add(entry.Key, fullResults);
            }

            return ComparedResults;
        }

        public static Dictionary<string, Tuple<string, string, string, string, string, bool>> GetChangedComparisonResults(Dictionary<string, Tuple<string, string, string, string, string, bool>> comparisonResults)
        {
            var tempList = new List<string>(comparisonResults.Keys);

            foreach (var key in tempList) {
                bool isChanged = comparisonResults[key].Item6;
                if (!isChanged) {
                    comparisonResults.Remove(key);
                }
            }

            return comparisonResults;
        }

        private static Tuple<string, bool> CompareResults(string previousResult, string currentResult)
        {
            if (!Program.isNUnit3) {
                previousResult = ModifyResultForNUnit2(previousResult);
                currentResult = ModifyResultForNUnit2(currentResult);
            }

            int previousResultValue = Int16.Parse(ConfigurationManager.AppSettings[previousResult]);
            int currentResultValue = Int16.Parse(ConfigurationManager.AppSettings[currentResult]);

            bool resultsAreDifferent = ResultsAreDifferent(previousResult, currentResult); 

            if (IgnoreResultIfSetToZero(previousResultValue, currentResultValue)) {
                return Tuple.Create("Ignored", false);
            }
            else if (ResultsDidNotChange(previousResultValue, currentResultValue)) {
                return Tuple.Create(currentResult, resultsAreDifferent);
            }
            else if (ResultWasFixed(previousResultValue, currentResultValue)) {
                return Tuple.Create("Fixed", resultsAreDifferent);
            }
            else if (ResultHasRegressed(previousResultValue, currentResultValue)) {
                return Tuple.Create("Regression", resultsAreDifferent);
            }
            else {
                return Tuple.Create("Undefined", resultsAreDifferent);
            }
        }

        private static bool ResultsAreDifferent(string previousResult, string currentResult)
        {
            bool resultsAreDifferent = false;
            if (previousResult != currentResult) {
                resultsAreDifferent = true;
            }

            return resultsAreDifferent;
        }

        private static bool IgnoreResultIfSetToZero(int previousResultValue, int currentResultValue)
        {
            if (previousResultValue == 0) {
                return true;
            }
            else if (currentResultValue == 0) {
                return true;
            }
            else {
                return false;
            }
        }

        private static bool ResultsDidNotChange(int previousResultValue, int currentResultValue)
        {
            if (previousResultValue == currentResultValue) {
                return true;
            }
            else {
                return false;
            }
        }

        private static bool ResultWasFixed(int previousResultValue, int currentResultValue)
        {
            int successValue = Int16.Parse(ConfigurationManager.AppSettings["Passed"]);

            if (currentResultValue == successValue && previousResultValue != successValue) {
                return true;
            }
            else {
                return false;
            }
        }

        private static bool ResultHasRegressed(int previousResultValue, int currentResultValue)
        {
            if (currentResultValue - previousResultValue < 0) {
                return true;
            }
            else {
                return false;
            }
        }

        private static string ModifyResultForNUnit2(string result)
        {
            if (result == "Success") {
                result = "Passed";
            }
            else if (result == "Failure") {
                result = "Failed";
            }
            return result;
        }
    }
}
