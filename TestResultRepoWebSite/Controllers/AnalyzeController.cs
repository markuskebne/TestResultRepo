using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class AnalyzeController : Controller
    {
        // GET: Analyze
        public async Task<ActionResult> Index()
        {
            var latestTestRun = await TestResultRepoApiHelper.GetLatestTestRun();
            var testRun = await TestResultRepoApiHelper.GetTestRunWithChildren(latestTestRun._Id);

            var failingSuites = testRun.TestSuites.FindAll(ts => ts.Result == Result.Failed).ToList();
            foreach (var suite in failingSuites)
            {
                var failingTestCases = suite.TestCases.FindAll(tc => tc.Result != Result.Failed);
                suite.TestCases.RemoveAll(tc => tc.Result != Result.Failed);
                foreach (var tc in failingTestCases)
                {
                    suite.TestCaseIds.Remove(tc._Id);
                }
            }

            List<TestCaseFailure> failures = new List<TestCaseFailure>();
            foreach (TestSuite suite in failingSuites)
            {
                foreach (TestCase testCase in suite.TestCases)
                {
                    failures.Add(new TestCaseFailure(testCase.Failure, testCase));
                }
            }

            var groupedFailureList = failures
                .GroupBy(f => f.Failure.Message)
                .Select(grp => grp.ToList())
                .ToList().OrderByDescending(g => g.Count).ToList();

            ViewBag.failingTestSuites = failingSuites;

            ViewBag.testRun = testRun;
            ViewBag.TestRunId = testRun._Id;
            ViewBag.TestRunName = testRun.Name;
            ViewBag.GroupedFailures = groupedFailureList;

            return View();
        }

        public class TestCaseFailure
        {
            public TestCaseFailure(Failure failure, TestCase testCase)
            {
                Failure = failure;
                TestCase = testCase;
            }

            public Failure Failure { get; set; }

            public TestCase TestCase { get; set; }
        }

        public class FailureGroup
        {
            public FailureGroup(List<TestCaseFailure> failures, string name)
            {
                Failures = failures;
                Name = name;
            }

            public List<TestCaseFailure> Failures { get; set; }

            public string Name { get; set; }
        }

    }
}