using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class TestSuiteCardController : Controller
    {

        public async Task<ActionResult> Index(string Id)
        {
            TestSuite testSuite = await TestResultRepoApiHelper.GetTestSuite(Id);
            return PartialView("TestSuiteCard", testSuite);
        }

        public ActionResult RenderViewFromId(string Id)
        {
            TestSuite testSuite = TestResultRepoApiHelper.GetTestSuiteSync(Id);
            return PartialView("TestSuiteCard", testSuite);
        }

        public ActionResult RenderViewFromTestSuite(TestSuite testSuite, bool showDate = true)
        {
            ViewBag.showDate = showDate;
            return PartialView("TestSuiteCard", testSuite);
        }
    }
}