using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TestResultRepoData;

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
                if (testRuns != null)
                {
                    ViewBag.Message = "Here are the TestRuns you were looking for:";
                    ViewBag.testRuns = testRuns;
                }
                else
                {
                    ViewBag.Message = "No such TestRun found.";
                }

                return View("All");
            }

            // Return the index view if id is provided
            var testRun = await HelperMethods.TestResultRepoApiHelper.GetTestRunWithChildren(id);

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
                //switch (testRun.Result)
                //{
                //    case Result.Passed:
                //        ViewBag.ResultColorClass = "bg-success";
                //        break;
                //    case Result.Failed:
                //        ViewBag.ResultColorClass = "bg-danger";
                //        break;
                //    default:
                //        ViewBag.ResultColorClass = "bg-warning";
                //        break;
                //}

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
    }
}