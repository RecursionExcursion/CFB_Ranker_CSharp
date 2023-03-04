

using CFB_Ranker.Persistence.Serializable_Models;
using CFB_Ranker.Persistence.Serialization;
using Newtonsoft.Json;

namespace CFB_Ranker.Persistence
{
    public class PersistenceManager
    {
        private readonly string completeRelativePath = @"..\..\..\Persistence\Team_Data\";
        private readonly string filename = "file.json";

        public Season LoadData()
        {
            string completePath = Path.Combine(completeRelativePath, filename);
            if (!File.Exists(completePath))
            {
                Season season = new SeasonMapper().BuildSeason();
                JSONSerializer.WriteJsonToFile<Season>(completePath, season);
            }
            return JSONSerializer.ReadJsonFromFile<Season>(completePath)!;
        }
    }
}
