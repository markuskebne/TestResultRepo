﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TestResultRepoModels;

namespace TestResultRepoWebSite.HelperMethods
{
    public static class TestResultRepoApiHelper
    {
        private static readonly string Baseurl = ConfigurationManager.AppSettings["APIBaseUrl"];

        #region TestRuns
        public static async Task<List<TestRun>> GetAllTestRuns()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/testruns");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testRuns = JsonConvert.DeserializeObject<List<TestRun>>(json).OrderBy(x => x.Name).ToList();

                return testRuns;
            }
        }

        public static async Task<TestRun> GetTestRun(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testrun/{id}");

                if (!response.IsSuccessStatusCode) return null;
                var json = response.Content.ReadAsStringAsync().Result;
                var testRun = JsonConvert.DeserializeObject<List<TestRun>>(json).FirstOrDefault();

                return testRun;

            }
        }

        public static async Task<List<TestRun>> GetTestRunsByName(string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testrun/?name={name}");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testRuns = JsonConvert.DeserializeObject<List<TestRun>>(json);

                return testRuns;

            }
        }

        public static async Task<List<TestRun>> GetTestRunsByCategory(string category)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testrun/?category={category}");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testRuns = JsonConvert.DeserializeObject<List<TestRun>>(json);

                return testRuns;

            }
        }

        public static async Task<List<string>> GetUniqueTestRunNames()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/testruns/unique");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testRuns = JsonConvert.DeserializeObject<List<string>>(json).OrderBy(x => x).ToList();

                return testRuns;
            }
        }

        public static async Task<TestRun> GetLatestTestRun(string category = null)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestUrl = category == null 
                    ? "api/testrun/latest" 
                    : $"api/testrun/latest/{category}";

                var response = await client.GetAsync($"{requestUrl}");

                if (!response.IsSuccessStatusCode) return null;
                var json = response.Content.ReadAsStringAsync().Result;
                var testRun = JsonConvert.DeserializeObject<TestRun>(json);

                return testRun;

            }
        }

        #endregion

        #region TestSuites
        public static async Task<List<TestSuite>> GetAllTestSuites()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/testsuites");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testSuites = JsonConvert.DeserializeObject<List<TestSuite>>(json).OrderBy(x => x.Name).ToList();

                return testSuites;
            }
        }

        public static async Task<TestSuite> GetTestSuite(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testsuite/{id}");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testSuite = JsonConvert.DeserializeObject<List<TestSuite>>(json).FirstOrDefault();

                return testSuite;

            }
        }

        public static  TestSuite GetTestSuiteSync(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync($"api/testsuite/{id}").Result;

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testSuite = JsonConvert.DeserializeObject<List<TestSuite>>(json).FirstOrDefault();

                return testSuite;

            }
        }

        public static async Task<List<TestSuite>> GetTestSuitesByName(string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testsuites/?name={name}");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testSuites = JsonConvert.DeserializeObject<List<TestSuite>>(json);

                return testSuites;

            }
        }

        public static async Task<List<TestSuite>> GetTestSuitesByCategory(string category)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testsuite/?category={category}");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testSuites = JsonConvert.DeserializeObject<List<TestSuite>>(json);

                return testSuites;

            }
        }

        public static async Task<List<string>> GetUniqueTestSuiteNames()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/testsuites/unique");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testRuns = JsonConvert.DeserializeObject<List<string>>(json).OrderBy(x => x).ToList();

                return testRuns;
            }
        }

        #endregion

        #region TestCases

        public static async Task<List<TestCase>> GetAllTestCases()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/testcases");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testCases = JsonConvert.DeserializeObject<List<TestCase>>(json).OrderBy(x => x.Name).ToList();

                return testCases;
            }
        }

        public static async Task<TestCase> GetTestCase(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testcase/{id}");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testCase = JsonConvert.DeserializeObject<List<TestCase>>(json).FirstOrDefault();

                return testCase;

            }
        }

        public static  TestCase GetTestCaseSync(string id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync($"api/testcase/{id}").Result;

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testCase = JsonConvert.DeserializeObject<List<TestCase>>(json).FirstOrDefault();

                return testCase;

            }
        }

        public static async Task<List<TestCase>> GetTestCasesByName(string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testcases/?name={name}");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testCases = JsonConvert.DeserializeObject<List<TestCase>>(json);

                return testCases;
            }
        }

        public static List<TestCase> GetTestCasesByNameSync(string name)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync($"api/testcases/?name={name}").Result;

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testCases = JsonConvert.DeserializeObject<List<TestCase>>(json);

                return testCases;
            }
        }

        public static async Task<List<TestCase>> GetTestCasesByCategory(string category)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync($"api/testcase/?category={category}");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testCases = JsonConvert.DeserializeObject<List<TestCase>>(json);

                return testCases;
            }
        }

        public static async Task<List<string>> GetUniqueTestCaseNames()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync("api/testcases/unique");

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testRuns = JsonConvert.DeserializeObject<List<string>>(json).OrderBy(x => x).ToList();

                return testRuns;
            }
        }

        public static List<string> GetUniqueTestCaseNamesSync()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync("api/testcases/unique").Result;

                if (!response.IsSuccessStatusCode) return null;

                var json = response.Content.ReadAsStringAsync().Result;
                var testRuns = JsonConvert.DeserializeObject<List<string>>(json).OrderBy(x => x).ToList();

                return testRuns;
            }
        }

        #endregion

        #region Shared

        public static async Task<TestRun> GetTestRunWithChildren(string id)
        {
            var testRun = await GetTestRun(id);

            foreach (var testSuiteId in testRun.TestSuiteIds)
            {
                var testSuite = await GetTestSuite(testSuiteId);
                foreach (var testCaseId in testSuite.TestCaseIds)
                {
                    testSuite.TestCases.Add(await GetTestCase(testCaseId));
                }

                testRun.TestSuites.Add(testSuite);
            }

            return testRun;
        }

        public static async Task<TestSuite> GetTestSuiteWithChildren(string id)
        {
            var testsuite = await GetTestSuite(id);

            foreach (var testCaseId in testsuite.TestCaseIds)
            {
                testsuite.TestCases.Add(await GetTestCase(testCaseId));
            }
            
            return testsuite;
        }

        #endregion
    }
}