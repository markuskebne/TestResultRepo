using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoData;

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
                var groupedSuites = getGroupedTestSuites(testSuites);

                if (testSuites != null)
                {
                    ViewBag.Message = "Here are the TestSuites you were looking for:";
                    ViewBag.testSuites = testSuites;
                }
                else
                {
                    ViewBag.Message = "No such TestSuite found.";
                }

                ViewBag.groups = groupedSuites;

                return View("TestSuiteCollectionCard", groupedSuites);
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

        private List<GroupedTestSuites> getGroupedTestSuites(List<TestSuite> testSuites)
        {
            var groupedTestSuites = new List<GroupedTestSuites>();
            var uniqueSuiteNames = testSuites.Select(ts => ts.Name).Distinct();
            foreach (var uniqueSuiteName in uniqueSuiteNames)
            {
                var group = new GroupedTestSuites();

                group.Name = uniqueSuiteName;
                group.TestSuites = testSuites.Where(ts => ts.Name == uniqueSuiteName).ToList();
                group.Total = group.TestSuites.Count();
                group.Passed = testSuites.Count(ts => ts.Result == Result.Passed);
                group.Failed = testSuites.Count(ts => ts.Result == Result.Failed);

                groupedTestSuites.Add(group);
            }
            
            //var groupedList = testSuites.GroupBy(ts => ts.Name).Select(name => name.ToList()).ToList();

            return groupedTestSuites;
        } 
    }
}