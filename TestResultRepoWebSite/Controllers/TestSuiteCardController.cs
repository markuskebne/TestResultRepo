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

        public ActionResult RenderViewFromTestSuite(TestSuite testSuite)
        {
            return PartialView("TestSuiteCard", testSuite);
        }
    }
}