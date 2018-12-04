using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;

namespace TestResultRepoData
{
    public class TestCase : TestBase
    {
        public TestCase()
        {
        }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("type")]
        public string Type { get; set; }

        [BsonElement("environment")]
        public string Environment { get; set; }

        [BsonElement("failure")]
        public Failure Failure { get; set; }

        //[BsonElement("testsuite")]
        [BsonIgnore]
        [JsonIgnore]
        public TestSuite TestSuite { get; set; }

        [BsonElement("testsuite")]
        public string TestSuiteId { get; set; }
    }
}
