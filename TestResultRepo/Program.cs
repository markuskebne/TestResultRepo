using System;
using System.IO;
using TestResultRepoIO;
using Output.Parsers;
using static System.IO.Path;

namespace TestResultRepo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 1)
            {
                // If argument is a nunit xml-file -> parse and save results
                // ReSharper disable once PossibleNullReferenceException
                if (GetExtension(args[0]).ToLower().Contains("xml"))
                {
                    var filename = args[0];
                    var testRun = NunitParser.Parse(filename);
                    MongoDb.SaveTestRun(testRun);
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
                            if (GetExtension(file).ToLower().Contains("xml"))
                            {
                                var testRun = NunitParser.Parse(file);
                                MongoDb.SaveTestRun(testRun);
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
            
            //var filename = "C:\\temp\\testresultrepo\\TestResult.xml";
            //TestRun testRun = NunitParser.Parse(filename);

            // Json.OutputToJson(testRun);

            //MongoDB.SaveTestRun(testRun);
            Console.WriteLine("End program");
            //Console.ReadLine();
        }

        private static void WriteUsageInfo()
        {
            Console.WriteLine("[INFO] Usage 1:  TestResultRepo \"path-to-result-file\"");
            Console.WriteLine("[INFO] Usage 2:  TestResultRepo \"path-to-folder-containing-multiple-result-files\"");
        }
    }
}
