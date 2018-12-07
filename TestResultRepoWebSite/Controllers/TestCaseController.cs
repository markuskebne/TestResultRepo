﻿using System;
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
    public class TestCaseController : Controller
    {
        string Baseurl = ConfigurationManager.AppSettings["APIBaseUrl"];

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
                    var json = response.Content.ReadAsStringAsync().Result;
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