using CFB_Ranker.Persistence.Serializable_Models;
using Newtonsoft.Json;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CFB_Ranker.Persistence
{
    public class JSONSerializer
    {
        public static void WriteJsonToFile<T>(string filePath, T t) where T : ISer
        {
            TextWriter? writer = null;
            try
            {
                string jsonString = JsonConvert.SerializeObject(t);
                writer = new StreamWriter(filePath, false);
                writer.Write(jsonString);
            } finally
            {
                writer?.Close();
            }
        }
        public static T? ReadJsonFromFile<T>(string filePath) where T : ISer
        {
            TextReader? reader = null;
            try
            {
                reader = new StreamReader(filePath);
                string fileString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<T>(fileString);
            } finally
            {
                reader?.Close();
            }
        }
    }
}
