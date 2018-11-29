using MongoDB.Bson.Serialization.Attributes;

namespace TestResultRepoData
{
    public class TestCase : TestBase
    {
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
        public TestSuite TestSuite { get; set; }

        [BsonElement("testsuite")]
        public string TestSuiteId { get; set; }
    }
}
