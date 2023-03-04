namespace CFB_Ranker.AbstractModels
{
    [Serializable]
    public abstract class AbstractTeamStats
    {
        public string Id { get; set; }
        public AbstractTeam[] Teams { get; set; }
        protected AbstractTeamStats(string id, AbstractTeam[] teams)
        {
            Id = id;
            Teams = teams;
        }
    }

    [Serializable]
    public abstract class AbstractTeam
    {
        public string School { get; set; }
        public int Points { get; set; }
        public AbstractStat[] Stats { get; set; }
        protected AbstractTeam(string school, int points, AbstractStat[] stats)
        {
            School = school;
            Points = points;
            Stats = stats;
        }
    }

    [Serializable]
    public abstract class AbstractStat
    {
        public string Category { get; set; }
        public string Stat { get; set; }
        protected AbstractStat(string category, string stat)
        {
            Category = category;
            Stat = stat;
        }
    }
}
