using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TestResultRepoData
{
    public class TestSuite : TestBase
    {
        public TestSuite()
        {
            TestCases = new List<TestCase>();
            TestCaseIds = new List<string>();
        }

        [BsonElement("testcasecount")]
        public int TestCaseCount { get; set; }

        [BsonElement("statusmessage")]
        public string StatusMessage { get; set; }

        
        [BsonIgnore]
        public List<TestCase> TestCases { get; set; }

        [BsonElement("testcases")]
        public List<string> TestCaseIds { get; set; }

        //[BsonElement("testrun")]
        [BsonIgnore]
        [JsonIgnore]
        public TestRun TestRun { get; set; }

        [BsonElement("testrun")]
        public string TestRunId { get; set; }

    }
}
