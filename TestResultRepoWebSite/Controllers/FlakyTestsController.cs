using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using TestResultRepoWebSite.HelperMethods;
using TestResultRepoWebSite.Models;

namespace TestResultRepoWebSite.Controllers
{
    public class FlakyTestsController : Controller
    {
        // GET: FlakyTests
        public ActionResult FlakyTests()
        {
            List<FlakyTestsGroup> flakyTestsGroups = new List<FlakyTestsGroup>();

            // Get testcases grouped by name
            List<string> uniqueTestCaseNamesList = TestResultRepoApiHelper.GetUniqueTestCaseNamesSync();

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