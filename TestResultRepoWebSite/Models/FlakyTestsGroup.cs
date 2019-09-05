using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ApplicationInsights.DataContracts;
using TestResultRepoModels;

namespace TestResultRepoWebSite.Models
{
    public class FlakyTestsGroup
    {
        public FlakyTestsGroup(List<TestCase> testCases)
        {
            Name = testCases.FirstOrDefault().Name;
            TestCases = testCases;
            Passed = testCases.Count(tc => tc.Result == Result.Passed);
            Failed = testCases.Count(tc => tc.Result == Result.Failed);
            var Sum = Passed + Failed;
            FailureFactor = (double)Failed/(double)Sum;

            Stability = FailureFactor >= 0.75 ? Result.Failed : FailureFactor <= 0.1 ? Result.Passed : Result.Inconclusive;
        }

        public string Name { get; set; }
        public List<TestCase> TestCases { get; set; }

        public int Passed { get; set; }
        public int Failed { get; set; }
        public double FailureFactor { get; set; }

        public Result Stability { get; set; }
    }
}