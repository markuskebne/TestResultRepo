using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;


namespace TestResultRepoWebSite.Controllers
{
    public class TestRunsController : Controller
    {       
        // GET: TestRun
        [OutputCache(Duration = 3600, VaryByParam = "id")]
        public async Task<ActionResult> Index(string id)
        {
            // Return the listview if no id is provided
            if (id == null)
            {
                var testRuns = await TestResultRepoApiHelper.GetAllTestRuns();
                var orderedTestRuns = testRuns.OrderByDescending(run => run.EndTime).ToList();
                if (testRuns.Count != 0)
                {
                    ViewBag.Message = "Here are the TestRuns you were looking for:";
                    ViewBag.testRuns = orderedTestRuns;
                }
                else
                {
                    ViewBag.Message = "No such TestRun found.";
                }

                return View("All");
            }

            // Return the index view if id is provided
            var testRun = await TestResultRepoApiHelper.GetTestRunWithChildren(id);

            if (testRun != null)
            {
                PopulateViewBag(testRun);
            }

            else
            {
                ViewBag.Message = "No such TestRun found.";
            }

            return View();
        }

        public async Task<ActionResult> Latest(string category = null)
        {
            var latestTestRun = await TestResultRepoApiHelper.GetLatestTestRun(category);
            var testRun = await TestResultRepoApiHelper.GetTestRunWithChildren(latestTestRun._Id);
            PopulateViewBag(testRun);
            return View();
            //return RedirectToAction("Index", "TestRuns", new {id = testRun._Id});
        }

        [OutputCache(Duration = 3600, VaryByParam = "id")]
        public async Task<ActionResult> Previous(string id)
        {
            var testRuns = await TestResultRepoApiHelper.GetAllTestRuns();
            var orderedTestRuns = testRuns.OrderBy(tr => tr.EndTime).ToList();
            var indexOfCurrent = orderedTestRuns.FindIndex(tr => tr._Id == id);

            string idOfPrevious;
            try
            {
                idOfPrevious = orderedTestRuns[indexOfCurrent - 1]._Id;
            }
            catch (ArgumentOutOfRangeException)
            {
                PopulateViewBag(await TestResultRepoApiHelper.GetTestRunWithChildren(id));
                return View("Index");
            }
            
            var testRun = await TestResultRepoApiHelper.GetTestRunWithChildren(idOfPrevious);
            PopulateViewBag(testRun);
            return View("Index");
        }

        [OutputCache(Duration = 3600, VaryByParam = "id")]
        public async Task<ActionResult> Next(string id)
        {
            var testRuns = await TestResultRepoApiHelper.GetAllTestRuns();
            var orderedTestRuns = testRuns.OrderBy(tr => tr.EndTime).ToList();
            var indexOfCurrent = orderedTestRuns.FindIndex(tr => tr._Id == id);

            string idOfNext;
            try
            {
                idOfNext = orderedTestRuns[indexOfCurrent + 1]._Id;
            }
            catch (ArgumentOutOfRangeException)
            {
                PopulateViewBag(await TestResultRepoApiHelper.GetTestRunWithChildren(id));
                return View("Index");
            }

            var testRun = await TestResultRepoApiHelper.GetTestRunWithChildren(idOfNext);
            PopulateViewBag(testRun);
            return View("Index");
        }

        public void PopulateViewBag(TestRun testRun)
        {
            ViewBag.testRun = testRun;
            ViewBag.Message = "Here is the TestRun you were looking for:";
            ViewBag.TestRunId = testRun._Id;

            var totalTestsRun = testRun.TestCaseCount - testRun.Skipped;
            double passedPercent = ((double)testRun.Passed / totalTestsRun) * 100;
            double failedPercent = ((double)testRun.Failed / totalTestsRun) * 100;
            double inconclusivePercent = ((double)testRun.Inconclusive / totalTestsRun) * 100;

            ViewBag.totalTestsRun = totalTestsRun.ToString();
            ViewBag.passedPercent = passedPercent.ToString("N2").Replace(",", ".");
            ViewBag.failedPercent = failedPercent.ToString("N2").Replace(",", ".");
            ViewBag.inconclusivePercent = inconclusivePercent.ToString("N2").Replace(",", ".");

            ViewBag.TestRunName = testRun.Name;

            ViewBag.temptestrun = testRun;
            ViewBag.temptestsuite = testRun.TestSuites.FirstOrDefault();
            ViewBag.temptestcase = testRun.TestSuites.FirstOrDefault()?.TestCases.FirstOrDefault();
        }
    }
}