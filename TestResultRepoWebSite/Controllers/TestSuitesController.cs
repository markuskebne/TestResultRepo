using System.Threading.Tasks;
using System.Web.Mvc;

namespace TestResultRepoWebSite.Controllers
{
    public class TestSuitesController : Controller
    {
        // GET: TestSuite
        public async Task<ActionResult> Index(string id)
        {
            // Return the listview if no id is provided
            if (id == null)
            {
                var testSuites = await HelperMethods.TestResultRepoApiHelper.GetAllTestSuites();
                if (testSuites != null)
                {
                    ViewBag.Message = "Here are the TestSuites you were looking for:";
                    ViewBag.testSuites = testSuites;
                }
                else
                {
                    ViewBag.Message = "No such TestSuite found.";
                }

                return View("All");
            }

            // Return the index view if id is provided
            var testSuite = await HelperMethods.TestResultRepoApiHelper.GetTestSuiteWithChildren(id);

            if (testSuite != null)
            {
                ViewBag.testSuite = testSuite;
                ViewBag.Message = "Here is the TestSuite you were looking for:";
                ViewBag.TestSuiteId = id;

                var totalTestsRun = testSuite.TestCaseCount - testSuite.Skipped;
                double passedPercent = ((double)testSuite.Passed / totalTestsRun) * 100;
                double failedPercent = ((double)testSuite.Failed / totalTestsRun) * 100;
                double inconclusivePercent = ((double)testSuite.Inconclusive / totalTestsRun) * 100;

                ViewBag.totalTestsRun = totalTestsRun.ToString();
                ViewBag.passedPercent = passedPercent.ToString("N2").Replace(",", ".");
                ViewBag.failedPercent = failedPercent.ToString("N2").Replace(",", ".");
                ViewBag.inconclusivePercent = inconclusivePercent.ToString("N2").Replace(",", ".");

                ViewBag.TestSuiteName = testSuite.Name;
            }

            else
            {
                ViewBag.Message = "No such TestSuite found.";
            }

            return View("TestSuiteCard", testSuite);
        }
    }
}