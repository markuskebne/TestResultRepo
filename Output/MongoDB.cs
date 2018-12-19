using System.Collections.Generic;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using TestResultRepoData;

namespace Output
{
    public static class MongoDb
    {
        private static string connectionString = "mongodb://localhost:27017";
        private static string databaseName = "TestResults";
        private static string testRunCollectionName = "test_runs";
        private static string testSuiteCollectionName = "test_suites";
        private static string testCaseCollectionName = "test_cases";

        #region Save Data
        public static void SaveTestRun(TestRun testRun)
        {
            foreach (var testSuite in testRun.TestSuites)
            {
                foreach (var testCase in testSuite.TestCases)
                {
                    Save(testCase);
                }
                Save(testSuite);
            }
            Save(testRun);       
        }

        public static void Save(TestRun testRun)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);

            IMongoCollection<TestRun> dbCollection = db.GetCollection<TestRun>(testRunCollectionName);

            dbCollection.InsertOne(testRun);
        }

        public static void Save(TestSuite testSuite)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);

            IMongoCollection<TestSuite> dbCollection = db.GetCollection<TestSuite>(testSuiteCollectionName);

            dbCollection.InsertOne(testSuite);
        }

        public static void Save(TestCase testCase)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);

            IMongoCollection<TestCase> dbCollection = db.GetCollection<TestCase>(testCaseCollectionName);

            dbCollection.InsertOne(testCase);
        }
        #endregion

        #region Delete Data
        public static void DeleteTestRunById(string testRunId)
        {
            TestRun testRun = GetTestRunWithChildren(testRunId);

            foreach (var testSuite in testRun.TestSuites)
            {
                foreach (var testCase in testSuite.TestCases)
                {
                    Delete(testCase);
                }
                Delete(testSuite);
            }
            Delete(testRun);
        }

        public static void Delete(TestRun testRun)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);

            IMongoCollection<TestRun> dbCollection = db.GetCollection<TestRun>(testRunCollectionName);
            var filter = Builders<TestRun>.Filter.Eq("_id", testRun._Id);

            dbCollection.DeleteOne(filter);
        }

        public static void Delete(TestSuite testSuite)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);

            IMongoCollection<TestSuite> dbCollection = db.GetCollection<TestSuite>(testSuiteCollectionName);

            var filter = Builders<TestSuite>.Filter.Eq("_id", testSuite._Id);

            dbCollection.DeleteOne(filter);
        }

        public static void Delete(TestCase testCase)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);

            IMongoCollection<TestCase> dbCollection = db.GetCollection<TestCase>(testCaseCollectionName);

            var filter = Builders<TestCase>.Filter.Eq("_id", testCase._Id);

            dbCollection.DeleteOne(filter);
        }
        #endregion

        #region TestRun
        public static List<TestRun> GetAllTestRuns()
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestRun> dbCollection = db.GetCollection<TestRun>(testRunCollectionName);

            var filter = Builders<TestRun>.Filter.Empty;

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestRun> GetTestRunById(string id)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestRun> dbCollection = db.GetCollection<TestRun>(testRunCollectionName);

            var filter = Builders<TestRun>.Filter.Eq("_id", id);

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestRun> GetTestRunByName(string name)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestRun> dbCollection = db.GetCollection<TestRun>(testRunCollectionName);

            var filter = Builders<TestRun>.Filter.Eq("name", name);

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<string> GetUniqueTestRunNames()
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestRun> dbCollection = db.GetCollection<TestRun>(testRunCollectionName);

            var filter = Builders<TestRun>.Filter.Empty;

            var result = dbCollection.Distinct<string>("name", filter).ToList();

            return result;
        }
        #endregion

        #region TestSuite
        public static List<TestSuite> GetAllTestSuites()
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestSuite> dbCollection = db.GetCollection<TestSuite>(testSuiteCollectionName);

            var filter = Builders<TestSuite>.Filter.Empty;

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestSuite> GetTestSuiteById(string id)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestSuite> dbCollection = db.GetCollection<TestSuite>(testSuiteCollectionName);

            var filter = Builders<TestSuite>.Filter.Eq("_id", id);

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestSuite> GetTestSuiteByName(string name)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestSuite> dbCollection = db.GetCollection<TestSuite>(testSuiteCollectionName);

            var filter = Builders<TestSuite>.Filter.Eq("name", name);
            
            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestSuite> GetTestSuiteByCategory(string category)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestSuite> dbCollection = db.GetCollection<TestSuite>(testSuiteCollectionName);

            var filter = Builders<TestSuite>.Filter.Eq("categories", category);

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestSuite> GetTestSuiteByCategoryAndName(string category, string name)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestSuite> dbCollection = db.GetCollection<TestSuite>(testSuiteCollectionName);

            var builder = Builders<TestSuite>.Filter;
            var filter = builder.And(builder.Eq("categories", category), builder.Eq("name", name));

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<string> GetUniqueTestSuiteNames()
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestSuite> dbCollection = db.GetCollection<TestSuite>(testSuiteCollectionName);

            var filter = Builders<TestSuite>.Filter.Empty;

            var result = dbCollection.Distinct<string>("name", filter).ToList();

            return result;
        }

        #endregion

        #region TestCase
        public static List<TestCase> GetAllTestCases()
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestCase> dbCollection = db.GetCollection<TestCase>(testCaseCollectionName);

            var filter = Builders<TestCase>.Filter.Empty;

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestCase> GetTestCaseById(string id)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestCase> dbCollection = db.GetCollection<TestCase>(testCaseCollectionName);

            var filter = Builders<TestCase>.Filter.Eq("_id", id);

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestCase> GetTestCaseByName(string name)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestCase> dbCollection = db.GetCollection<TestCase>(testCaseCollectionName);

            var filter = Builders<TestCase>.Filter.Eq("name", name);

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestCase> GetTestCaseByCategory(string category)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestCase> dbCollection = db.GetCollection<TestCase>(testCaseCollectionName);

            var filter = Builders<TestCase>.Filter.Eq("categories", category);

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<TestCase> GetTestCaseByCategoryAndName(string category, string name)
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestCase> dbCollection = db.GetCollection<TestCase>(testCaseCollectionName);

            var builder = Builders<TestCase>.Filter;
            var filter = builder.And(builder.Eq("categories", category), builder.Eq("name", name));

            var result = dbCollection.Find(filter).ToList();

            return result;
        }

        public static List<string> GetUniqueTestCaseNames()
        {
            var client = new MongoClient(connectionString);
            IMongoDatabase db = client.GetDatabase(databaseName);
            IMongoCollection<TestCase> dbCollection = db.GetCollection<TestCase>(testCaseCollectionName);

            var filter = Builders<TestCase>.Filter.Empty;

            var result = dbCollection.Distinct<string>("name", filter).ToList();

            return result;
        }


        #endregion

        #region Helper Methods
        public static TestRun GetTestRunWithChildren(string id)
        {
            TestRun testRun = GetTestRunById(id).FirstOrDefault();

            foreach (var testSuiteId in testRun.TestSuiteIds)
            {
                var testSuite = GetTestSuiteById(testSuiteId).FirstOrDefault();
                foreach (var testCaseId in testSuite.TestCaseIds)
                {
                    testSuite.TestCases.Add(GetTestCaseById(testCaseId).FirstOrDefault());
                }

                testRun.TestSuites.Add(testSuite);
            }

            return testRun;
        }

        public static TestSuite GetTestSuiteWithChildren(string id)
        {
            var testSuite = GetTestSuiteById(id).FirstOrDefault();

            foreach (var testCaseId in testSuite.TestCaseIds)
            {
                testSuite.TestCases.Add(GetTestCaseById(testCaseId).FirstOrDefault());
            }          

            return testSuite;
        }
        #endregion

    }
}
