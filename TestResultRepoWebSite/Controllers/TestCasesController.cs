using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class TestCasesController : Controller
    {
        // GET: TestCase
        public async Task<ActionResult> Index(string id)
        {
            // Return the listview if no id is provided
            if (id == null)
            {
                //var testCases = await HelperMethods.TestResultRepoApiHelper.GetAllTestCases();
                //var groupedTestCases = getGroupedTestCases(testCases);
                var uniqueNames = await HelperMethods.TestResultRepoApiHelper.GetUniqueTestCaseNames();
                if (uniqueNames == null)
                {
                    ViewBag.Message = "No such TestCase found.";
                }

                return View("TestCaseGroupList", uniqueNames);
            }

            // Return the index view if id is provided
            var testCase = await HelperMethods.TestResultRepoApiHelper.GetTestCase(id);
            if (testCase != null)
            {
                ViewBag.testCase = testCase;
                ViewBag.TestCaseName = testCase.Name;

                var testcases = await TestResultRepoApiHelper.GetTestCasesByName(testCase.Name);
                var orderedTestCases = testcases.OrderByDescending(tc => tc.EndTime);

                ViewBag.testCases = orderedTestCases;
            }
            else
            {
                ViewBag.Message = "No such TestCase found.";
            }

            return View();
            //return View("TestCaseCard", testCase);
        }

        public ActionResult RenderTestCaseCard(TestCase testCase, bool showDate = true)
        {
            ViewBag.showDate = showDate;
            return PartialView("TestCaseCard", testCase);
        }


        private List<GroupedTestCases> getGroupedTestCases(List<TestCase> testCases)
        {
            var groupedTestCases = new List<GroupedTestCases>();
            var uniqueNames = testCases.Select(ts => ts.Name).Distinct();
            foreach (var uniqueName in uniqueNames)
            {
                var group = new GroupedTestCases();

                group.Name = uniqueName;
                group.TestSuites = testCases.Where(ts => ts.Name == uniqueName).ToList();
                group.Total = group.TestSuites.Count();
                group.Passed = testCases.Count(ts => ts.Result == Result.Passed);
                group.Failed = testCases.Count(ts => ts.Result == Result.Failed);

                groupedTestCases.Add(group);
            }

            //var groupedList = testSuites.GroupBy(ts => ts.Name).Select(name => name.ToList()).ToList();

            return groupedTestCases;
        }
    }

}