using System;
using System.Collections.Generic;
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
    public class TestCaseController : Controller
    {
        private string Baseurl = "http://testresultrepoapi/";

        // GET: TestCase
        public async Task<ActionResult> Index(string Id)
        {
            TestCase testCase = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync($"api/testcase/{Id}");

                if (response.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api   
                    var json = response.Content.ReadAsStringAsync().Result;

                    //Deserializing the response recieved from web api and storing into the Employee list  
                    testCase = JsonConvert.DeserializeObject<List<TestCase>>(json).FirstOrDefault();

                    ViewBag.Message = "Here is the TestCase you were looking for:";
                    ViewBag.TestCaseId = Id;
                    if (testCase != null) ViewBag.TestCaseName = testCase.Name;
                }
                else
                {
                    ViewBag.Message = "No such TestCase found.";
                }

            }

            return View();
        }
    }
}