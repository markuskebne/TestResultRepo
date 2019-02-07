using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class TestSuiteGroupListController : Controller
    {
        // GET: TestSuiteCollection
        public ActionResult TestSuiteGroupList()
        {
            return View();
        }

        [HttpGet]
        [OutputCache(Duration = 3600, VaryByParam = "name")]
        public async Task<ActionResult> RenderTestSuiteCardList(string name)
        {
            var testSuites = await TestResultRepoApiHelper.GetTestSuitesByName(name);
            var orderedTestSuites = testSuites.OrderByDescending(ts => ts.EndTime);
            return PartialView("TestSuiteCardList", orderedTestSuites);
        }
    }
}