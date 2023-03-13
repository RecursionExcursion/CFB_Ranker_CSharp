

using CFB_Ranker.Persistence.Serializable_Models;
using CFB_Ranker.Persistence.Serialization;
using Newtonsoft.Json;

namespace CFB_Ranker.Persistence
{
    public class PersistenceManager
    {
        private readonly string _filePathFromRoot = @"\Persistence\Team_Data\file.json";

        //Get Relative Root Dir of project
        private string? GetRelativeDir() => Directory.GetParent(Directory.GetCurrentDirectory())?.Parent?.Parent?.FullName;

        public Season LoadData()
        {
            string completePath = GetRelativeDir() + _filePathFromRoot;
            if (!File.Exists(completePath))
            {
                Season season = new SeasonMapper().BuildSeason();
                JSONSerializer.WriteJsonToFile<Season>(completePath, season);
            }
            return JSONSerializer.ReadJsonFromFile<Season>(completePath)!;
        }
    }
}
