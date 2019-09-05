using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestResultRepoModels;
using TestResultRepoWebSite.HelperMethods;
using TestResultRepoWebSite.Models;

namespace TestResultRepoWebSite.Controllers
{
    public class FlakyTestsController : Controller
    {
        // GET: FlakyTests
        public ActionResult FlakyTests(TestRun testRun)
        {
            List<FlakyTestsGroup> flakyTestsGroups = new List<FlakyTestsGroup>();

            List<string> uniqueTestCaseNamesList = new List<string>();

            foreach (var suite in testRun.TestSuites)
            {
                foreach (var testCase in suite.TestCases)
                {
                    if (testCase.Result != Result.Skipped)
                    {
                        uniqueTestCaseNamesList.Add(testCase.Name);
                    }  
                }
            }

            foreach (var uniqueName in uniqueTestCaseNamesList)
            {
                var testCases = TestResultRepoApiHelper.GetTestCasesByNameSync(uniqueName);

                testCases.RemoveAll(tc => DateTime.Parse(tc.EndTime) < DateTime.Now.AddDays(-21));

                if (testCases.Count == 0) { continue; }

                // only use the last 20 
                var orderedTestCases = testCases
                    .OrderByDescending(tc => tc.EndTime)
                    .Take(20);

                
                FlakyTestsGroup group = new FlakyTestsGroup(orderedTestCases.ToList());
                flakyTestsGroups.Add(group);
            }

            var orderedFlakyTestsGroups = flakyTestsGroups.OrderByDescending(g => g.FailureFactor).ToList();

            return PartialView("FlakyTests", orderedFlakyTestsGroups);
        }
    }
}