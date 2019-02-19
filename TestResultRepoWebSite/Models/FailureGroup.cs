using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TestResultRepoWebSite.Models
{
    public class FailureGroup
    {
        public FailureGroup(List<TestCaseFailure> failures, string name)
        {
            Failures = failures;
            Name = name;
        }

        public List<TestCaseFailure> Failures { get; set; }

        public string Name { get; set; }
    }
}