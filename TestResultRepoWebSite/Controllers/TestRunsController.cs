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
    public class TestRunsController : Controller
    {
        string Baseurl = ConfigurationManager.AppSettings["APIBaseUrl"];

        // GET: TestRun
        public async Task<ActionResult> Index(string Id)
        {
            if (Id == null)
            {
                List<TestRun> testRuns = null;
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync($"api/testruns");

                    if (response.IsSuccessStatusCode)
                    {
                        var json = response.Content.ReadAsStringAsync().Result;
                        testRuns = JsonConvert.DeserializeObject<List<TestRun>>(json).OrderBy(x => x.Name).ToList(); ;

                        ViewBag.Message = "Here are the TestRuns you were looking for:";
                        ViewBag.testRuns = testRuns;
                    }
                    else
                    {
                        ViewBag.Message = "No such TestRun found.";
                    }
                }

                return View("All");
            }

            TestRun testRun = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"api/testrun/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    var json = response.Content.ReadAsStringAsync().Result;
                    testRun = JsonConvert.DeserializeObject<List<TestRun>>(json).FirstOrDefault();

                    ViewBag.Message = "Here is the TestRun you were looking for:";
                    ViewBag.TestRunId = Id;
                    if (testRun != null) ViewBag.TestRunName = testRun.Name;
                }
                else
                {
                    ViewBag.Message = "No such TestRun found.";
                }
            }

            return View();
        }
    }
}