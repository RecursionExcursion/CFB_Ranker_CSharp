using CFB_Ranker.Persistence.Serialization;
using System.Security.Cryptography.X509Certificates;

namespace CFB_Ranker.Service
{
    public class WeightedTeam
    {
        public SerSchool School { get; private set; }
        public string SchoolName { get => School.School; }
        public int Rank { get; set; }
        public WinLoss Record { get; private set; }
        public double Weight { get; set; }

        public int TotalOffense { get; set; }
        public int TotalDefense { get; set; }
        public int TotalPointsFor { get; set; }
        public int TotalPointsAllowed { get; set; }
        public List<SerGame> Schedule { get; private set; }
        public double StrengthOfSchedule { get; set; }

        public static WeightedTeam CreateTeamFromSchool(SerSchool school)
        {
            return new WeightedTeam(school);
        }
        private WeightedTeam(SerSchool school)
        {
            School = school;
            Rank = 0;
            Record = new WinLoss();
            TotalOffense = 0;
            TotalDefense = 0;
            TotalPointsFor = 0;
            TotalPointsAllowed = 0;
            Weight = 0;
            Schedule = new List<SerGame>();
            StrengthOfSchedule = 0;
        }

        public static WeightedTeam CreateTeamFromWeightedTeam(WeightedTeam team)
        {
            return new WeightedTeam(team);
        }

        private WeightedTeam(WeightedTeam team)
        {
            School = team.School;
            Rank = team.Rank;
            Record = new WinLoss(team.Record);
            TotalOffense = team.TotalOffense;
            TotalDefense = team.TotalDefense;
            TotalPointsFor = team.TotalPointsFor;
            TotalPointsAllowed = team.TotalPointsAllowed;
            Weight = team.Weight;
            Schedule = new List<SerGame>(team.Schedule);
            StrengthOfSchedule = team.StrengthOfSchedule;
        }

        public double GetTotalOffensePerGame()
        {
            return (double) TotalOffense / Schedule.Count;
        }
        public double GetTotalDefensePerGame()
        {
            return (double) TotalDefense / Schedule.Count;
        }
        public double GetPointsForPerGame()
        {
            return (double) TotalPointsFor / Schedule.Count;
        }
        public double GetPointsAllowedPerGame()
        {
            return (double) TotalPointsAllowed / Schedule.Count;
        }

        public double GetWins()
        {
            return Record.Wins;
        }
        public double GetLosses()
        {
            return Record.Losses;
        }

        public double GetStrengthOfSchedule()
        {
            return StrengthOfSchedule;
        }

        public class WinLoss
        {
            public int Wins { get; set; }
            public int Losses { get; set; }

            public WinLoss()
            {
                Wins = 0;
                Losses = 0;
            }

            public WinLoss(WinLoss winLoss)
            {
                Wins = winLoss.Wins;
                Losses = winLoss.Losses;
            }
        }
    }
}