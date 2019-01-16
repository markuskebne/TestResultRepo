using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TestResultRepoModels;

namespace Output
{
    public static class Json
    {
        public static void OutputToJson(TestRun testRun)
        {
            using (StreamWriter file = File.CreateText(@"C:\temp\reportunit\jsonOutput.txt"))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, testRun);
            }
        }
    }
}
