namespace CFB_Ranker.AbstractModels
{
    [Serializable]
    public abstract class AbstractGameInfo
    {
        public string Id { get; set; }
        public string Season { get; set; }
        public string Week { get; set; }
        public string Season_Type { get; set; }
        public string Start_Date { get; set; }
        public string Home_Id { get; set; }
        public string Away_Id { get; set; }

        protected AbstractGameInfo(string id, string season, string week, string season_Type, string start_Date, string home_Id, string away_Id)
        {
            Id = id;
            Season = season;
            Week = week;
            Season_Type = season_Type;
            Start_Date = start_Date;
            Home_Id = home_Id;
            Away_Id = away_Id;
        }
    }
}
