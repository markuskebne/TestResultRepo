using System.Threading.Tasks;
using System.Web.Mvc;

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
                var testCases = await HelperMethods.TestResultRepoApiHelper.GetAllTestCases();
                if (testCases != null)
                {
                    ViewBag.Message = "Here are the TestCases you were looking for:";
                    ViewBag.testCases = testCases;
                }
                else
                {
                    ViewBag.Message = "No such TestCase found.";
                }

                return View("All");
            }

            // Return the index view if id is provided
            var testCase = await HelperMethods.TestResultRepoApiHelper.GetTestCase(id);
            if (testCase != null)
            {
                ViewBag.testCase = testCase;
                ViewBag.Message = "Here is the TestCase you were looking for:";
                ViewBag.TestCaseId = id;

                ViewBag.TestCaseName = testCase.Name;
            }
            else
            {
                ViewBag.Message = "No such TestCase found.";
            }

            return View();
        }
    }
}