

using CFB_Ranker.Persistence.Serializable_Models;
using CFB_Ranker.Persistence.Serialization;
using Newtonsoft.Json;

namespace CFB_Ranker.Persistence
{
    public class PersistenceManager
    {
        private readonly string dir = @"C:\Users\rloup\Main Folder\Programming\Workspaces\VS-workspace\CFB_Ranker\" +
                        @"CFB_Ranker\Persistence\Team_Data\";
        private readonly string filename = "file.json";

        public Season LoadData()
        {
            if (!File.Exists(dir + filename))
            {
                Season season = new SeasonMapper().BuildSeason();
                JSONSerializer.WriteJsonToFile<Season>(dir + filename, season);
            }
            return JSONSerializer.ReadJsonFromFile<Season>(dir + filename)!;
        }
    }
}
