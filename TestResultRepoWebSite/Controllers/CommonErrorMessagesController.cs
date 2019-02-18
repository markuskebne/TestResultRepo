using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.Models;

namespace TestResultRepoWebSite.Controllers
{
    public class CommonErrorMessagesController : Controller
    {
        // GET: CommonErrorMessages
        public ActionResult CommonErrorMessages(TestRun testRun)
        {
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

            return PartialView("CommonErrorMessagesView", groupedFailureList);
        }
    }
}