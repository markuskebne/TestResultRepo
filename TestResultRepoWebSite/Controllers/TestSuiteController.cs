using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using TestResultRepoData;

namespace TestResultRepoWebSite.Controllers
{
    public class TestSuiteController : Controller
    {
        // GET: TestSuite
        string Baseurl = ConfigurationManager.AppSettings["APIBaseUrl"];

        public async Task<ActionResult> Index(string Id)
        {
            TestSuite testSuite = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"api/testsuite/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    testSuite = JsonConvert.DeserializeObject<List<TestSuite>>(json).FirstOrDefault();

                    ViewBag.Message = "Here is the TestSuite you were looking for:";
                    ViewBag.TestSuiteId = Id;
                    if (testSuite != null) ViewBag.TestSuiteName = testSuite.Name;
                }
                else
                {
                    ViewBag.Message = "No such TestSuite found.";
                }
            }

            return View();
        }
    }
}