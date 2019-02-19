using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestResultRepoModels;

namespace TestResultRepoWebSite.Models
{
    public class TestCaseFailure
    {
        public TestCaseFailure(Failure failure, TestCase testCase)
        {
            Failure = failure;
            TestCase = testCase;
        }

        public Failure Failure { get; set; }

        public TestCase TestCase { get; set; }
    }
}