using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using TestResultRepoData;

namespace Output.Parsers
{
    public static class NunitParser
    {
        public static TestRun Parse(string filepath)
        {
            var doc = XDocument.Load(filepath);
            var filename = Path.GetFileNameWithoutExtension(filepath);

            var report = new TestRun();

            report.Name = filename;
            report.TestCaseCount = doc.Descendants("test-case").Count();
            report.Passed = GetPassed(doc.Root);
            report.Failed = GetFailed(doc.Root);
            report.Inconclusive = GetInconclusive(doc.Root);
            report.Skipped = GetSkipped(doc.Root);

            report.StartTime = GetStartTime(doc.Root);
            report.EndTime = GetEndTime(doc.Root);
            report.Duration =
                (DateTime.Parse(report.EndTime) - DateTime.Parse(report.StartTime)).ToString(@"hh\:mm\:ss");

            // TestSuites
            var suites = doc
                .Descendants("test-suite")
                .Where(x => x.Attribute("type").Value.Equals("TestFixture", StringComparison.CurrentCultureIgnoreCase));

            suites.AsParallel().ToList().ForEach(ts =>
            {
                var testSuite = new TestSuite();
                testSuite.TestRun = report;
                testSuite.TestRunId = report._Id;
                testSuite.Name = ts.Attribute("name")?.Value;

                // Suite Time Info
                testSuite.StartTime = GetStartTime(ts);
                testSuite.EndTime = GetEndTime(ts);
                testSuite.Duration = (DateTime.Parse(testSuite.EndTime) - DateTime.Parse(testSuite.StartTime)).ToString(@"hh\:mm\:ss");

                testSuite.TestCaseCount = ts.Descendants("test-case").Count();

                testSuite.Passed = GetPassed(ts);
                testSuite.Failed = GetFailed(ts);
                testSuite.Inconclusive = GetInconclusive(ts);
                testSuite.Skipped = GetSkipped(ts);


                // any error messages and/or stack-trace
                var testSuiteFailure = ts.Element("failure");
                if (testSuiteFailure != null)
                {
                    var message = testSuiteFailure.Element("message");
                    if (message != null)
                    {
                        testSuite.StatusMessage = message.Value;
                    }

                    var stackTrace = testSuiteFailure.Element("stack-trace");
                    if (stackTrace != null && !string.IsNullOrWhiteSpace(stackTrace.Value))
                    {
                        testSuite.StatusMessage = string.Format(
                            "{0}\n\nStack trace:\n{1}", testSuite.StatusMessage, stackTrace.Value);
                    }
                }

                var output = ts.Element("output") != null ? ts.Element("output")?.Value : null;
                if (!string.IsNullOrWhiteSpace(output))
                {
                    testSuite.StatusMessage += "\n\nOutput:\n" + output;
                }

                // get test suite level categories
                //var suiteCategories = GetCategories(ts, false);

                // Test Cases
                ts.Descendants("test-case").AsParallel().ToList().ForEach(tc =>
                {
                    var test = new TestCase();
                    test.TestSuite = testSuite;
                    test.TestSuiteId = testSuite._Id;
                    test.Name = tc.Attribute("name")?.Value;
                    test.Result = ResultExtensions.ToStatus(tc.Attribute("result")?.Value);

                    // TestCase Time Info
                    test.StartTime = GetStartTime(tc);
                    test.EndTime = GetEndTime(tc);
                    test.Duration =
                        (DateTime.Parse(test.EndTime) - DateTime.Parse(test.StartTime)).ToString(@"hh\:mm\:ss");

                    // description
                    var description =
                        tc.Descendants("property")
                            .Where(c => c.Attribute("name").Value
                                .Equals("Description", StringComparison.CurrentCultureIgnoreCase));
                    test.Description =
                        description.Any()
                            ? description.ToArray()[0].Attribute("value")?.Value
                            : "";

                    // Categories
                    var categoryProperties =
                        tc.Descendants("property")
                            .Where(c => c.Attribute("name").Value
                                .Equals("Category", StringComparison.CurrentCultureIgnoreCase));

                    foreach (var property in categoryProperties)
                    {
                        test.Categories.Add(property.Attribute("value")?.Value);
                    }

                    //error and other status messages
                    var testCaseFailure = new Failure
                    {
                        Message = tc.Element("failure") != null
                            ? tc.Element("failure")?.Element("message")?.Value.Trim()
                            : "",
                        StackTrace = tc.Element("failure") != null
                            ? tc.Element("failure")?.Element("stack-trace") != null
                                ? tc.Element("failure")?.Element("stack-trace")?.Value.Trim()
                                : ""
                            : ""
                    };


                    test.Failure = testCaseFailure;
                    
                    testSuite.TestCases.Add(test);
                    testSuite.TestCaseIds.Add(test._Id);
                });

                testSuite.Categories = AggregateCategories(testSuite);

                testSuite.Result = AggregateResults(testSuite.TestCases);

                report.TestSuites.Add(testSuite);
                report.TestSuiteIds.Add(testSuite._Id);
            });

            report.Categories = AggregateCategories(report);

            report.Result = AggregateResults(report.TestSuites);

            return report;
        }

        #region Variable parsing methods
        public static string GetStartTime(XElement element)
        {
            return element.Attribute("start-time") != null
                ? element.Attribute("start-time")?.Value
                : element.Attribute("date")?.Value + " " + element.Attribute("time")?.Value;
        }

        public static string GetEndTime(XElement element)
        {
            return element.Attribute("end-time") != null
                ? element.Attribute("end-time")?.Value
                : "";
        }

        public static int GetPassed(XElement element)
        {
            return element.Attribute("passed") != null
                ? int.Parse(element.Attribute("passed")?.Value)
                : element.Descendants("test-case").Where(x =>
                    x.Attribute("result").Value.Equals("success", StringComparison.CurrentCultureIgnoreCase)).Count();
        }

        public static int GetFailed(XElement element)
        {
            return element.Attribute("failed") != null
                ? int.Parse(element.Attribute("failed")?.Value)
                : int.Parse(element.Attribute("failures")?.Value);
        }

        public static int GetInconclusive(XElement element)
        {
            return element.Attribute("inconclusive") != null
                ? int.Parse(element.Attribute("inconclusive")?.Value)
                : int.Parse(element.Attribute("inconclusive")?.Value);
        }

        public static int GetSkipped(XElement element)
        {
            var skipped = element.Attribute("skipped") != null
                ? int.Parse(element.Attribute("skipped")?.Value)
                : int.Parse(element.Attribute("skipped")?.Value);
            var ignored = element.Attribute("ignored") != null
                ? int.Parse(element.Attribute("ignored")?.Value)
                : 0;

            return skipped + ignored;
        }

        #endregion

        #region HelperMethods
        public static Result AggregateResults(IEnumerable<TestSuite> testSuites)
        {
            return AggregateResults(testSuites.Select(t => t.Result).ToList());
        }

        public static Result AggregateResults(IEnumerable<TestCase> testCases)
        {
            return AggregateResults(testCases.Select(t => t.Result).ToList());
        }

        public static Result AggregateResults(List<Result> results)
        {
            if (results.Any(x => x == Result.Failed)) return Result.Failed;
            if (results.Any(x => x == Result.Error)) return Result.Error;
            if (results.Any(x => x == Result.Inconclusive)) return Result.Inconclusive;
            if (results.Any(x => x == Result.Passed)) return Result.Passed;
            if (results.Any(x => x == Result.Skipped)) return Result.Skipped;

            return Result.Unknown;
        }

        public static List<string> AggregateCategories(TestRun testRun)
        {
            var categoriesList = new List<string>();

            foreach (var testSuite in testRun.TestSuites)
            {
                if (!testSuite.Categories.Any())
                    testSuite.Categories = AggregateCategories(testSuite);
            }

            foreach (var testSuite in testRun.TestSuites)
            {
                foreach (var category in testSuite.Categories)
                {
                    if (!categoriesList.Contains(category))
                    {
                        categoriesList.Add(category);
                    }
                }
            }

            return categoriesList;
        }

        public static List<string> AggregateCategories(TestSuite testSuite)
        {
            var categoriesList = new List<string>();

            foreach (var testCases in testSuite.TestCases)
            {
                foreach (var category in testCases.Categories)
                {
                    if (!categoriesList.Contains(category))
                    {
                        categoriesList.Add(category);
                    }
                }
            }

            return categoriesList;
        }
        #endregion
    }
}
