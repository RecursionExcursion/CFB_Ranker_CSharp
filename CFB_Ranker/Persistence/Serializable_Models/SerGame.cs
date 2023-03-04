using CFB_Ranker.AbstractModels;
using CFB_Ranker.Persistence.Serializable_Models;
using Newtonsoft.Json;

namespace CFB_Ranker.Persistence.Serialization
{
    [Serializable]
    public class SerGame : AbstractGameInfo, ISer
    {
        [JsonConstructor]
        public SerGame(string id, string season, string week, string season_Type, string start_Date, string home_Id, string away_Id)
            : base(id, season, week, season_Type, start_Date, home_Id, away_Id) { }
        public SerGame(AbstractGameInfo game) :
            this(game.Id, game.Season, game.Week, game.Season_Type, game.Start_Date, game.Home_Id, game.Away_Id)
        { }

        public SerTeamStats TeamStats { get; set; }
    }
}