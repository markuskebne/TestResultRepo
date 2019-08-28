using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class AnalyzeController : Controller
    {
        // GET: Analyze
        public async Task<ActionResult> Index(string id = null)
        {
            ViewBag.ShowFlakyTests = true;

            if (id == null)
            {
                var latestTestRun = await TestResultRepoApiHelper.GetLatestTestRun();
                id = latestTestRun._Id;
                ViewBag.ShowFlakyTests = true;
            }

            var testRun = await TestResultRepoApiHelper.GetTestRunWithChildren(id);

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

        public async Task<ActionResult> Latest(string category = null)
        {
            var latestTestRun = await TestResultRepoApiHelper.GetLatestTestRun(category);
            return await Index(latestTestRun._Id);
            //return RedirectToAction("Index", "TestRuns", new {id = testRun._Id});
        }
    }
}