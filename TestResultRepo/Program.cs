using System;
using System.Configuration;
using System.IO;
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
        private static string APIBaseUrl = ConfigurationManager.AppSettings["APIBaseUrl"];

        static void Main(string[] args)
        {
        if (args.Length == 1)
            {
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
                            Console.WriteLine("[Error] Folder does not contain any test results.");
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
                Console.WriteLine("[Error] Incorrect number of arguments.");
                WriteUsageInfo();
            }
            
            Console.WriteLine("End program");
        }

        private static void WriteUsageInfo()
        {
            Console.WriteLine("[INFO] Usage 1:  TestResultRepo.exe \"path-to-result-file\"");
            Console.WriteLine("[INFO] Usage 2:  TestResultRepo.exe \"path-to-folder-containing-multiple-result-files\"");
            Console.ReadLine();
        }

        private static void SaveTestRun(TestRun testRun)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(APIBaseUrl);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var content = new StringContent(JsonConvert.SerializeObject(testRun), Encoding.UTF8, "application/json");

                try
                {
                    var response = client.PostAsync("api/testrun/save", content);

                    if (!response.Result.IsSuccessStatusCode)
                    {
                        Console.WriteLine("The request was sent successfully!");
                    }

                    Console.WriteLine($"Response code: {response.Result.StatusCode}");
                }
                catch (Exception e)
                {
                    Console.WriteLine("Something went wrong when posting the result\nException: " + e);
                }              
            }

        }
    }
}
