using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NUnit_Result_Comparison_Tool
{
    class ReadResults
    {
        public static Dictionary<string, Tuple<string, string, string>> GetTestCaseInformation(string XmlLocation)
        {
            var xml = XDocument.Load(XmlLocation);
            var testFixtureQuery = GetTestFixtures(xml);
            IEnumerable<Tuple<string, string, string, string>> testResultEnumerable;

            if (!Program.isNUnit3) {
                testResultEnumerable = GetTestCaseQueryResults_NUnit2(xml, testFixtureQuery);
            }
            else {
                testResultEnumerable = GetTestCaseQueryResults_NUnit3(xml, testFixtureQuery);
            }

            // Key = TestCasePath, Item1 = TestCaseName, Item2 = TestFixtureName, Item3 = Result
            Dictionary<string, Tuple<string, string, string>> testResult = testResultEnumerable.ToDictionary(x => x.Item1, 
                x => new Tuple<string, string, string>(x.Item2, x.Item3, x.Item4));

            GetTestCaseName(testResult);
            return testResult;
        }

        private static Dictionary<string, Tuple<string, string, string>> GetTestCaseName(Dictionary<string, Tuple<string, string, string>> testResults)
        {
            var tempList = new List<string>(testResults.Keys);

            foreach (var key in tempList) {
                var testCaseName = testResults[key].Item1.Split('.');
                testResults[key] = new Tuple<string, string, string>(testCaseName[testCaseName.Count() - 1], testResults[key].Item2, testResults[key].Item3);
            }
            return testResults;
        }

        private static IEnumerable<string> GetTestFixtures(XDocument xml)
        {
            var testFixtureQuery = from a in xml.Root.Descendants("test-suite")
                                   where a.Attribute("type").Value == "TestFixture"
                                   select a.Attribute("name").Value;

            return testFixtureQuery;
        }

        private static IEnumerable<Tuple<string, string, string, string>> GetTestCaseQueryResults_NUnit2(XDocument xml, IEnumerable<string>  testFixtures)
        {
            IEnumerable<Tuple<string, string, string, string>> results = new Tuple<string, string, string, string>[0];

            foreach (var fixture in testFixtures) {
                var testCaseQuery = from a in xml.Root.Descendants("test-case")
                                    where a.Attribute("name").Value.Contains(fixture)
                                    select new Tuple<string, string, string, string>(a.Attribute("name").Value,
                                        a.Attribute("name").Value, fixture, a.Attribute("result").Value);
                results = results.Union(testCaseQuery);
            }

            return results;
        }

        private static IEnumerable<Tuple<string, string, string, string>> GetTestCaseQueryResults_NUnit3(XDocument xml, IEnumerable<string> testFixtures)
        {
            IEnumerable<Tuple<string, string, string, string>> results = new Tuple<string, string, string, string>[0];

            foreach (var fixture in testFixtures) {
                var testCaseQuery = from a in xml.Root.Descendants("test-case")
                                    where a.Attribute("fullname").Value.Contains(fixture)
                                    select new Tuple<string, string, string, string>(a.Attribute("fullname").Value,
                                        a.Attribute("name").Value, fixture, a.Attribute("result").Value);
                results = results.Union(testCaseQuery);
            }

            return results;
        }
    }
}
