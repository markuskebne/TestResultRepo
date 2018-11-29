using System.IO;
using Newtonsoft.Json;
using TestResultRepoData;

namespace Output
{
    public static class Json
    {
        public static void OutputToJson(TestRun testRun)
        {
            using (StreamWriter file = File.CreateText(@"C:\temp\testresultrepo\jsonOutput.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }

        }
    }
}
