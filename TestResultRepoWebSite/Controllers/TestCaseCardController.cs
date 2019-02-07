using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class TestCaseCardController : Controller
    {
        // GET: TestCaseCard
        public async Task<ActionResult> Index(string Id)
        {
            TestCase testCase = await TestResultRepoApiHelper.GetTestCase(Id);
            return PartialView("TestCaseCard", testCase);
        }

        public  ActionResult RenderViewFromId(string Id, bool showDate = true)
        {
            ViewBag.showDate = showDate;
            TestCase testCase = TestResultRepoApiHelper.GetTestCaseSync(Id);
            return PartialView("TestCaseCard", testCase);
        }

        public ActionResult RenderViewFromTestCase(TestCase testCase, bool showDate = true)
        {
            ViewBag.showDate = showDate;
            return PartialView("TestCaseCard", testCase);
        }
    }
}