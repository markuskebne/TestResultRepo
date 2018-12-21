using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class TestCaseCollectionController : Controller
    {
        // GET: /TestCaseCollection/
        public ActionResult TestCaseCollection()
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