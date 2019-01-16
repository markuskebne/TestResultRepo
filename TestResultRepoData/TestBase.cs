using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TestResultRepoModels
{
    public class TestBase
    {
        public TestBase()
        {
            _Id = ObjectId.GenerateNewId().ToString();
            Categories = new List<string>();
            Result = Result.Unknown;
        }

        [BsonId]
        // ReSharper disable once InconsistentNaming
        public string _Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("passed")]
        public int Passed { get; set; }

        [BsonElement("failed")]
        public int Failed { get; set; }

        [BsonElement("skipped")]
        public int Skipped { get; set; }

        [BsonElement("inconclusive")]
        public int Inconclusive { get; set; }

        [BsonElement("result")]
        public Result Result { get; set; }

        [BsonElement("starttime")]
        public string StartTime { get; set; }

        [BsonElement("endtime")]
        public string EndTime { get; set; }

        [BsonElement("duration")]
        public string Duration { get; set; }

        [BsonElement("categories")]
        public List<string> Categories { get; set; }
    }
}
