using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestResultRepoModels
{
    public class GroupedTestSuites
    {
        public GroupedTestSuites()
        {
            TestSuites = new List<TestSuite>();
        }

        public string Name;
        public List<TestSuite> TestSuites;
        public int Passed;
        public int Failed;
        public int Total;
    }
}
