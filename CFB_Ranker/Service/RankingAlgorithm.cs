using CFB_Ranker.AbstractModels;
using CFB_Ranker.Persistence.Serialization;
using System.ComponentModel.DataAnnotations;

namespace CFB_Ranker.Service
{

    //TODO Add Data Mulitplier then corss refrence Java Results, May have bug in Strength of schedule
    public class RankingAlgorithm
    {
        public Season Season { get; }
        public RankedSeason UnWeightedRankedSeason { get; }
        public RankedSeason WeightedRankedSeason { get; }

        [Range(1, 1)]
        private readonly int firstWeek = 1;
        private readonly int finalWeek;

        private readonly string GetWins = "GetWins";
        private readonly string GetLosses = "GetLosses";

        private readonly string GetTotalOffensePerGame = "GetTotalOffensePerGame";
        private readonly string GetTotalDefensePerGame = "GetTotalDefensePerGame";

        private readonly string GetPointsForPerGame = "GetPointsForPerGame";
        private readonly string GetPointsAllowedPerGame = "GetPointsAllowedPerGame";

        private readonly string GetStrengthOfSchedule = "GetStrengthOfSchedule";

        public RankingAlgorithm(Season season)
        {
            UnWeightedRankedSeason = new();
            WeightedRankedSeason = new();
            Season = season;
            finalWeek = Season.Games.Select(g => Convert.ToInt32(g.Week)).Max();
        }

        public void RankTeams()
        {
            for (int i = firstWeek; i <= finalWeek; i++)
            {
                UnWeightedRankedSeason.Weeks.Add(LinkTeamsToGamesForWeek(i, "regular"));
            }
            UnWeightedRankedSeason.Weeks.Add(LinkTeamsToGamesForWeek(1, "postseason"));

            foreach (var week in UnWeightedRankedSeason.Weeks)
            {
                WeightedRankedSeason.Weeks.Add(SetWeightAndRank(week));
            }



        }

        private List<WeightedTeam> SetWeightAndRank(List<WeightedTeam> teams)
        {
            List<WeightedTeam> teamsRankedByWins = teams.OrderByDescending(t => t.Record.Wins).ToList();
            List<WeightedTeam> teamsRankedByLosses = teams.OrderBy(t => t.Record.Losses).ToList();

            List<WeightedTeam> teamsRankedByPFPG = teams.OrderByDescending(t => t.GetPointsForPerGame()).ToList();
            List<WeightedTeam> teamsRankedByPAPG = teams.OrderBy(t => t.GetPointsAllowedPerGame()).ToList();

            List<WeightedTeam> teamsRankedByOffPG = teams.OrderByDescending(t => t.GetTotalOffensePerGame()).ToList();
            List<WeightedTeam> teamsRankedByDFPG = teams.OrderBy(t => t.GetTotalDefensePerGame()).ToList();


            Dictionary<WeightedTeam, double> winMap = SetWeightOfRankings(teamsRankedByWins, GetWins);
            Dictionary<WeightedTeam, double> lossMap = SetWeightOfRankings(teamsRankedByLosses, GetLosses);

            Dictionary<WeightedTeam, double> offMap = SetWeightOfRankings(teamsRankedByOffPG, GetTotalOffensePerGame);
            Dictionary<WeightedTeam, double> defMap = SetWeightOfRankings(teamsRankedByDFPG, GetTotalDefensePerGame);

            Dictionary<WeightedTeam, double> pFMap = SetWeightOfRankings(teamsRankedByPFPG, GetPointsForPerGame);
            Dictionary<WeightedTeam, double> pAMap = SetWeightOfRankings(teamsRankedByPAPG, GetPointsAllowedPerGame);

            List<WeightedTeam> wTeams = new(teams);

            //Set Weight
            foreach (var team in wTeams)
            {
                double weight = 0;
                weight = winMap[team];
                weight = lossMap[team];
                weight = offMap[team];
                weight = defMap[team];
                weight = pFMap[team];
                weight = pAMap[team];
                team.Weight = weight;
            }
            wTeams = wTeams.OrderBy(t => t.Weight).ToList();


            CalculateStrengthOfSchedule(wTeams);

            List<WeightedTeam> teamsRankedBySS = wTeams.OrderBy(t => t.GetStrengthOfSchedule()).ToList();

            Dictionary<WeightedTeam, double> SSMap = SetWeightOfRankings(teamsRankedBySS, GetStrengthOfSchedule);

            wTeams.ForEach(t => t.Weight += SSMap[t]);

            SetRankings(wTeams);
            return wTeams;
        }

        private void CalculateStrengthOfSchedule(List<WeightedTeam> teams)
        {
            foreach (var team in teams)
            {
                int schedStrength = 0;

                foreach (var game in team.Schedule)
                {
                    WeightedTeam opponent = null;

                    AbstractTeam thisTeam = game.TeamStats.Teams.Where(t => t.School == team.SchoolName).First();
                    AbstractTeam oppTeam = game.TeamStats.Teams.Where(t => t.School != team.SchoolName).First();

                    try
                    {
                        opponent = teams.Where(t => t.School.School == oppTeam.School).First();
                    } catch (Exception)
                    {
                        opponent = WeightedTeam.CreateTeamFromSchool(new SerSchool("-1", null, null, null, null, null));
                    }

                    //If opponent is not on list (not FBS School), give worst weight
                    int opponentStrIndex = teams.Contains(opponent) ? teams.IndexOf(opponent) + 1 : teams.Count + 1;
                    schedStrength += opponentStrIndex;
                }
                team.StrengthOfSchedule = schedStrength;
            }

        }

        private void SetRankings(List<WeightedTeam> teams)
        {
            int rank = 1;
            foreach (var team in teams)
            {
                team.Rank = rank++;
            }
        }

        private Dictionary<WeightedTeam, double> SetWeightOfRankings(List<WeightedTeam> teams, string methodSignature)
        {
            Dictionary<WeightedTeam, double> teamWeightMap = new();

            WeightedTeam prevTeam = null!;

            for (int i = 0, rankWeight = 1; i < teams.Count; i++)
            {
                WeightedTeam currTeam = teams[i];

                if (prevTeam != null)
                {
                    var method = currTeam.GetType().GetMethods().Where(m => m.Name == methodSignature).First();

                    double prevTeamVal = (double) method.Invoke(prevTeam, null);
                    double currTeamVal = (double) method.Invoke(currTeam, null);

                    if (prevTeamVal != currTeamVal)
                    {
                        rankWeight = i + 1;
                    }
                }
                teamWeightMap.Add(currTeam, rankWeight);
                prevTeam = currTeam;
            }
            return teamWeightMap;
        }

        private List<WeightedTeam> LinkTeamsToGamesForWeek(int week, string season)
        {
            //Filter games
            List<SerGame> games = Season.Games.Where(g => Convert.ToInt32(g.Week) == week).Where(g => g.Season_Type == season).ToList();

            //Create Teams
            List<WeightedTeam> weightedTeams = season == "postseason" ?
                weightedTeams = BuildTeamsForWeek(finalWeek + 1) :
                weightedTeams = BuildTeamsForWeek(week);

            //Link Games to Teams
            games.ForEach(g => GetTeamsAndCompileGames(g));

            return weightedTeams;

            void GetTeamsAndCompileGames(SerGame game)
            {
                string away_Id = game.Away_Id;
                string home_Id = game.Home_Id;
                List<WeightedTeam> teamsInGame = weightedTeams.Where(wt => wt.School.Id == away_Id || wt.School.Id == home_Id).ToList();
                teamsInGame.ForEach(t => StatCompilier.ComplileStatsForTeam(game, t));
            }

            List<WeightedTeam> BuildTeamsForWeek(int week) => week == 1 ?
                 Season.Schools.Select(s => WeightedTeam.CreateTeamFromSchool(s)).ToList() :
                 UnWeightedRankedSeason.Weeks[^1].Select(s => WeightedTeam.CreateTeamFromWeightedTeam(s)).ToList();
        }
    }
}
