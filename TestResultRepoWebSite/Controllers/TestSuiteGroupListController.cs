using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        public async Task<ActionResult> GetPartialView(string name)
        {
            var testSuites = await TestResultRepoApiHelper.GetTestSuitesByName(name);
            return PartialView("TestSuiteCardList", testSuites);
        }
    }
}