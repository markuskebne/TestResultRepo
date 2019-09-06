using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Output.Parsers;
using TestResultRepoModels;

namespace TestResultRepoConsole
{
    class Program
    {
        // The API endpoint url is configured in the app.config
        private static readonly string ApiBaseUrl = ConfigurationManager.AppSettings["APIBaseUrl"];

        static void Main(string[] args)
        {
            CheckConnection();
            
            if (args.Length == 1)
            {
                if (args[0] == "delete")
                {
                    DeleteTestRuns();
                }
                // If argument is a nunit xml-file -> parse and save results
                // ReSharper disable once PossibleNullReferenceException
                if (Path.GetExtension(args[0]).ToLower().Contains("xml"))
                {
                    var filename = args[0];
                    var testRun = NunitParser.Parse(filename);
                    SaveTestRun(testRun);
                }

                // If argument is a folder
                // if the folder contains nunit xml-files -> parse and save results
                if (Directory.Exists(args[0]))
                {
                    try
                    {
                        var folderContainedXmlFiles = false;
                        var files = Directory.GetFiles(args[0]);

                        foreach (var file in files)
                        {
                            if (Path.GetExtension(file).ToLower().Contains("xml"))
                            {
                                var testRun = NunitParser.Parse(file);
                                SaveTestRun(testRun);
                                folderContainedXmlFiles = true;
                            }
                        }

                        if (!folderContainedXmlFiles)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("[Error] Folder does not contain any test results.");
                            Console.ForegroundColor = ConsoleColor.White;
                            WriteUsageInfo();
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                }
            }

            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[Error] Incorrect number of arguments.\n");
                Console.ForegroundColor = ConsoleColor.White;
                WriteUsageInfo();
            }
            
            Console.WriteLine("End program");
            Console.ReadLine();
        }

        private static void WriteUsageInfo()
        {
            Console.WriteLine("**************** Application usage *********************\n" +
                              "\n" +
                              "Running the application without argument will run a connection check and display application usage info.\n" +
                              "\n" +
                              "Giving a testresult file (in .xml) as an argument will parse the file and send the data to the API endpoint given in TestResultRepoConsole.exe.config\n" +
                              "\n" +
                              "Giving a folder containing testresult files (in .xml) as an argument will parse all files and send the data to the API endpoint given in TestResultRepoConsole.exe.config\n" +
                              "\n" +
                              "Giving 'delete' as the first argument will delete all data that is older than 2 months\n" +
                              "\n" +
                              "**********************************************************\n");
            Console.ReadLine();
        }

        private static void SaveTestRun(TestRun testRun)
        {
            using (var client = new HttpClient())
            {
                var PostTestRunEndpoint = "api/testrun/save";

                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(testRun), Encoding.UTF8, "application/json");

                try
                {
                    var response = client.PostAsync(PostTestRunEndpoint, content);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("The request was sent successfully!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine($"Response code: {response.Result.StatusCode}");
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Something went wrong when posting the result\nException: " + e);
                    Console.ForegroundColor = ConsoleColor.White;
                }              
            }

        }

        private static void CheckConnection()
        {
            using (var client = new HttpClient())
            {
                var HealthCheckEndpoint = "api/healthcheck";

                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                Console.WriteLine("**************** Verifying connection *********************\n" +
                                  "\n" +
                                  $"Base adress: {client.BaseAddress}\n" +
                                  $"HealthCheck endpoint {HealthCheckEndpoint}\n" +
                                  $"Endpoint: {client.BaseAddress}{HealthCheckEndpoint}\n" +
                                  "\n" +
                                  "Checking...\n");

                try
                {
                    var response = client.GetAsync(HealthCheckEndpoint);

                    if (response.Result.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("The request was sent successfully!");
                        Console.ForegroundColor = ConsoleColor.White;
                    }

                    Console.WriteLine($"Response code: {response.Result.StatusCode}");
                }
                catch (Exception exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Something went wrong when posting the result\nException: ");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(exception);
                }

                Console.WriteLine("\n**********************************************************\n");
            }
        }

        private static void DeleteTestRuns()
        {
            var GetTestRunsEndpoint = "api/testruns";
            var DeleteRunsEndpoint = "api/testrun/delete";
            IEnumerable<TestRun> testRunsToDelete;

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("!!!!    Warning    !!!!\n");
            Console.Write("All test-data older than 2 months will be deleted.\n");
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("Are you sure? Type 'yes' to proceed.\n");
            
            var userResponse = Console.ReadLine();

            if (userResponse != null && userResponse.ToLower() != "yes")
            {
                Console.WriteLine("Cancelling");
                return;
            }

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ApiBaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = client.GetAsync(GetTestRunsEndpoint).Result;

                var json = response.Content.ReadAsStringAsync().Result;
                var testRuns = JsonConvert.DeserializeObject<List<TestRun>>(json).OrderBy(x => x.Name).ToList();
                var deleteTestRunsBefore = DateTime.Now.AddMonths(-2);
                testRunsToDelete = testRuns.Where(tr => Convert.ToDateTime(tr.StartTime) < deleteTestRunsBefore).OrderBy(x => x.StartTime);                
            }

            foreach (var testRun in testRunsToDelete)
            {
                using (var client = new HttpClient())
                {
                    Console.WriteLine($"Deleting TestRun {testRun.Name} - {testRun._Id}");

                    client.BaseAddress = new Uri(ApiBaseUrl);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var response = client.DeleteAsync($"{DeleteRunsEndpoint}/{testRun._Id}").Result;

                    Console.WriteLine($"Response status code: {response.StatusCode} - {testRun._Id}");

                    if (response.IsSuccessStatusCode)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Delete OK");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"{response.Content.ReadAsStringAsync()}");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }
        }
    }
}
