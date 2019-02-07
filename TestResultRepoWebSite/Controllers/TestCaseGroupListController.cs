using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class TestCaseGroupListController : Controller
    {
        // GET: /TestCaseCollection/
        public ActionResult TestCaseGroupList()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GetPartialView(string name)
        {
            var testCases = await TestResultRepoApiHelper.GetTestCasesByName(name);
            var orderedTestCases = testCases.OrderByDescending(ts => ts.EndTime);
            return PartialView("TestCaseCardList", orderedTestCases);
        }
    }
}