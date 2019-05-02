using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;

namespace TestResultRepoModels
{
    public class TestRun : TestBase
    {
        public TestRun()
        {
            TestSuites = new List<TestSuite>();
            TestSuiteIds = new List<string>();
        }

        [BsonElement("testcasecount")]
        public int TestCaseCount { get; set; }
        
        [BsonIgnore]
        public List<TestSuite> TestSuites { get; set; }

        [BsonElement("testsuites")]
        public List<string> TestSuiteIds { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

    }
}
