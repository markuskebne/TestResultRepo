using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestResultRepoModels
{
    public class GroupedTestCases
    {
        public GroupedTestCases()
        {
            TestSuites = new List<TestCase>();
        }

        public string Name;
        public List<TestCase> TestSuites;
        public int Passed;
        public int Failed;
        public int Total;
    }
}