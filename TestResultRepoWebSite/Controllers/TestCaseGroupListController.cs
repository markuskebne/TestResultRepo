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
            return PartialView("TestCaseCardList", testCases);
        }
    }
}