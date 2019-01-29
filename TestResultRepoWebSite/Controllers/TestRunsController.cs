using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;

namespace TestResultRepoWebSite.Controllers
{
    public class TestRunsController : Controller
    {
        // GET: TestRun
        public async Task<ActionResult> Index(string id)
        {
            // Return the listview if no id is provided
            if (id == null)
            {
                var testRuns = await HelperMethods.TestResultRepoApiHelper.GetAllTestRuns();
                List<TestRun> ordertestRuns = testRuns.OrderByDescending(run => run.StartTime).ToList();
                if (testRuns != null)
                {
                    ViewBag.Message = "Here are the TestRuns you were looking for:";
                    ViewBag.testRuns = ordertestRuns;
                }
                else
                {
                    ViewBag.Message = "No such TestRun found.";
                }

                return View("All");
            }

            // Return the index view if id is provided
            var testRun = await TestResultRepoApiHelper.GetTestRunWithChildren(id);

            if (testRun != null)
            {
                ViewBag.testRun = testRun;
                ViewBag.Message = "Here is the TestRun you were looking for:";
                ViewBag.TestRunId = id;

                var totalTestsRun = testRun.TestCaseCount - testRun.Skipped;
                double passedPercent = ((double)testRun.Passed / totalTestsRun) * 100;
                double failedPercent = ((double)testRun.Failed / totalTestsRun) * 100;
                double inconclusivePercent = ((double)testRun.Inconclusive / totalTestsRun) * 100;

                ViewBag.totalTestsRun = totalTestsRun.ToString();
                ViewBag.passedPercent = passedPercent.ToString("N2").Replace(",", ".");
                ViewBag.failedPercent = failedPercent.ToString("N2").Replace(",", ".");
                ViewBag.inconclusivePercent = inconclusivePercent.ToString("N2").Replace(",", ".");

                ViewBag.TestRunName = testRun.Name;

                ViewBag.temptestrun = testRun;
                ViewBag.temptestsuite = testRun.TestSuites.FirstOrDefault();
                ViewBag.temptestcase = testRun.TestSuites.FirstOrDefault().TestCases.FirstOrDefault();
            }

            else
            {
                ViewBag.Message = "No such TestRun found.";
            }

            return View();
        }

        public async Task<ActionResult> Latest()
        {
            var testRun = await TestResultRepoApiHelper.GetLatestTestRun();
            return RedirectToAction("Index", "TestRuns", new {id = testRun._Id});
        }
    }
}