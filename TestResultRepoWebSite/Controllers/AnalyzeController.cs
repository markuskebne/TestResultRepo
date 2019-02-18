using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;
using TestResultRepoWebSite.Models;

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

            ViewBag.failingTestSuites = failingSuites;

            ViewBag.testRun = testRun;
            ViewBag.TestRunId = testRun._Id;
            ViewBag.TestRunName = testRun.Name;

            return View();
        }
    }
}