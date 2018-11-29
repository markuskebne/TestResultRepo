using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Output;
using TestResultRepoData;

namespace TestResultRepoAPI.Controllers
{
    [RoutePrefix("api")]
    public class TestResultController : ApiController
    {
        [HttpGet]
        [Route("HealthCheck")]
        public HttpResponseMessage GetSystemHealth()
        {
            const string response = "All is OK!";

            return new HttpResponseMessage()
            {
                Content = new StringContent(response, System.Text.Encoding.UTF8, "application/string")
            };
        }

        [HttpGet]
        [Route("testruns")]
        public HttpResponseMessage GetAllTestRuns()
        {
            var testruns = MongoDb.GetAllTestRuns();
            var json = JsonConvert.SerializeObject(testruns);
            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpGet]
        [Route("testrun/{id}")]
        public HttpResponseMessage GetTestRunById(string id)
        {
            var testruns = MongoDb.GetTestRunById(id);
            var json = JsonConvert.SerializeObject(testruns);
            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpGet]
        [Route("testsuites/{category?}/{name?}")]
        public HttpResponseMessage GetAllTestSuites(string category = null, string name = null)
        {
            List<TestSuite> testsuites;
            if (category != null && name != null)
                testsuites = MongoDb.GetTestSuiteByCategoryAndName(category, name);
            
            else if (category != null)
                testsuites = MongoDb.GetTestSuiteByCategory(category);
            
            else if (name != null)
                testsuites = MongoDb.GetTestSuiteByName(name);
            
            else
                testsuites = MongoDb.GetAllTestSuites();
            
            
            var json = JsonConvert.SerializeObject(testsuites);
            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpGet]
        [Route("testsuite/{id}")]
        public HttpResponseMessage GetTestSuiteById(string id)
        {
            var testsuite = MongoDb.GetTestSuiteById(id);
            var json = JsonConvert.SerializeObject(testsuite);
            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpGet]
        [Route("testcases/{category?}/{name?}")]
        public HttpResponseMessage GetAllTestCases(string category = null, string name = null)
        {
            List<TestCase> testcases;
            if (category != null && name != null)
                testcases = MongoDb.GetTestCaseByCategoryAndName(category, name);

            else if (category != null)
                testcases = MongoDb.GetTestCaseByCategory(category);

            else if (name != null)
                testcases = MongoDb.GetTestCaseByName(name);

            else
                testcases = MongoDb.GetAllTestCases();


            var json = JsonConvert.SerializeObject(testcases);
            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }

        [HttpGet]
        [Route("testcase/{id}")]
        public HttpResponseMessage GetTestCaseById(string id)
        {
            var testcase = MongoDb.GetTestCaseById(id);
            var json = JsonConvert.SerializeObject(testcase);
            return new HttpResponseMessage()
            {
                Content = new StringContent(json, System.Text.Encoding.UTF8, "application/json")
            };
        }
    }
}
