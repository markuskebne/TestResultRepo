using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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

        public  ActionResult RenderViewFromId(string Id)
        {
            TestCase testCase = TestResultRepoApiHelper.GetTestCaseSync(Id);
            return PartialView("TestCaseCard", testCase);
        }

        public ActionResult RenderViewFromTestCase(TestCase testCase)
        {
            return PartialView("TestCaseCard", testCase);
        }
    }
}