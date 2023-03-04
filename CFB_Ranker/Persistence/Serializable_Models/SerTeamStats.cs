using CFB_Ranker.AbstractModels;
using CFB_Ranker.Persistence.Serializable_Models;
using Newtonsoft.Json;

namespace CFB_Ranker.Persistence.Serialization
{
    [Serializable]
    public class SerTeamStats : AbstractTeamStats, ISer
    {
        [JsonConstructor]
        public SerTeamStats(string id, SerTeam[] teams) : base(id, teams) { }
    }

    [Serializable]
    public class SerTeam : AbstractTeam, ISer
    {
        [JsonConstructor]
        public SerTeam(string school, int points, SerStat[] stats) : base(school, points, stats) { }
    }

    [Serializable]
    public class SerStat : AbstractStat, ISer   
    {
        [JsonConstructor]
        public SerStat(string category, string stat) : base(category, stat) { }
    }
}
