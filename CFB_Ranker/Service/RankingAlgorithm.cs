using CFB_Ranker.AbstractModels;
using CFB_Ranker.Persistence.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace CFB_Ranker.Service
{

    //TODO Add Data Mulitplier then corss refrence Java Results, May have bug in Strength of schedule
    public class RankingAlgorithm
    {
        public Season Season { get; }
        public RankedSeason UnWeightedRankedSeason { get; }
        public RankedSeason WeightedRankedSeason { get; }

        private readonly WeightDistributor _weightDistributor;

        private readonly string regularSeasonString = "regular";
        private readonly string postSeasonString = "postseason";

        [Range(1, 1)]
        private readonly int _firstWeek = 1;
        private readonly int _finalWeek;

        private const string _getWins = "GetWins";
        private const string _getLosses = "GetLosses";

        private const string _getTotalOffensePerGame = "GetTotalOffensePerGame";
        private const string _getTotalDefensePerGame = "GetTotalDefensePerGame";

        private const string _getPointsForPerGame = "GetPointsForPerGame";
        private const string _getPointsAllowedPerGame = "GetPointsAllowedPerGame";

        private const string _getStrengthOfSchedule = "GetStrengthOfSchedule";
        private const string _pollInteria = "PollIntertia";

        public RankingAlgorithm(Season season, WeightDistributor weightDistributor)
        {
            UnWeightedRankedSeason = new();
            WeightedRankedSeason = new();
            Season = season;
            _finalWeek = Season.Games.Select(g => Convert.ToInt32(g.Week)).Max();
            _weightDistributor = weightDistributor;
        }

        public void RankTeams()
        {
            for (int i = _firstWeek; i <= _finalWeek; i++)
            {
                UnWeightedRankedSeason.Weeks.Add(LinkTeamsToGamesForWeek(i, regularSeasonString));
            }
            if (Season.Games.Any(g => g.Season_Type == postSeasonString))
            {
                UnWeightedRankedSeason.Weeks.Add(LinkTeamsToGamesForWeek(1, postSeasonString));
            }

            for (int i = 0; i < UnWeightedRankedSeason.Weeks.Count; i++)
            {
                List<WeightedTeam>? week = UnWeightedRankedSeason.Weeks[i];

                WeightedRankedSeason.Weeks.Add(SetWeightAndRank(week, i));
            }
        }

        private List<WeightedTeam> SetWeightAndRank(List<WeightedTeam> teams, int index)
        {
            List<WeightedTeam> teamsRankedByWins = teams.OrderByDescending(t => t.Record.Wins).ToList();
            List<WeightedTeam> teamsRankedByLosses = teams.OrderBy(t => t.Record.Losses).ToList();

            List<WeightedTeam> teamsRankedByPFPG = teams.OrderByDescending(t => t.GetPointsForPerGame()).ToList();
            List<WeightedTeam> teamsRankedByPAPG = teams.OrderBy(t => t.GetPointsAllowedPerGame()).ToList();

            List<WeightedTeam> teamsRankedByOffPG = teams.OrderByDescending(t => t.GetTotalOffensePerGame()).ToList();
            List<WeightedTeam> teamsRankedByDFPG = teams.OrderBy(t => t.GetTotalDefensePerGame()).ToList();

            Dictionary<WeightedTeam, double> winMap = SetWeightOfRankings(teamsRankedByWins, _getWins);
            Dictionary<WeightedTeam, double> lossMap = SetWeightOfRankings(teamsRankedByLosses, _getLosses);

            Dictionary<WeightedTeam, double> offMap = SetWeightOfRankings(teamsRankedByOffPG, _getTotalOffensePerGame);
            Dictionary<WeightedTeam, double> defMap = SetWeightOfRankings(teamsRankedByDFPG, _getTotalDefensePerGame);

            Dictionary<WeightedTeam, double> pFMap = SetWeightOfRankings(teamsRankedByPFPG, _getPointsForPerGame);
            Dictionary<WeightedTeam, double> pAMap = SetWeightOfRankings(teamsRankedByPAPG, _getPointsAllowedPerGame);

            //Poll Inertia
            List<WeightedTeam> lastWeeksPolls;
            Dictionary<WeightedTeam, double> pollInteriaMap = null!;
            if (index != 0)
            {
                lastWeeksPolls = WeightedRankedSeason.Weeks[index - 1];
                pollInteriaMap = SetWeightOfRankings(lastWeeksPolls, _pollInteria);
            }

            List<WeightedTeam> wTeams = new(teams);

            //Set Weight
            foreach (var team in wTeams)
            {
                double weight = 0;
                weight += winMap[team];
                weight += lossMap[team];
                weight += offMap[team];
                weight += defMap[team];
                weight += pFMap[team];
                weight += pAMap[team];
                if (index != 0)
                {
                    WeightedTeam mapTeam = pollInteriaMap.Keys.FirstOrDefault(t => t.School.Id == team.School.Id)!;
                    weight += pollInteriaMap[mapTeam];
                }
                team.Weight = weight;
            }
            wTeams = wTeams.OrderBy(t => t.Weight).ToList();


            CalculateStrengthOfSchedule(wTeams);

            List<WeightedTeam> teamsRankedBySS = wTeams.OrderBy(t => t.GetStrengthOfSchedule()).ToList();

            Dictionary<WeightedTeam, double> SSMap = SetWeightOfRankings(teamsRankedBySS, _getStrengthOfSchedule);

            wTeams.ForEach(t => t.Weight += SSMap[t]);

            wTeams = wTeams.OrderBy(t => t.Weight).ToList();

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
                    WeightedTeam? opponent = null;

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
            //Sets multiplier
            int multiplier = methodSignature switch
            {
                _getWins => _weightDistributor.Wins,
                _getLosses => _weightDistributor.Losses,
                _getPointsForPerGame => _weightDistributor.PointsFor,
                _getPointsAllowedPerGame => _weightDistributor.PointsAllowed,
                _getTotalOffensePerGame => _weightDistributor.TotalOffense,
                _getTotalDefensePerGame => _weightDistributor.TotalDefense,
                _getStrengthOfSchedule => _weightDistributor.ScheduleStrength,
                _pollInteria => _weightDistributor.PollInertia,
                _ => 1,
            };

            Dictionary<WeightedTeam, double> teamWeightMap = new();
            if (methodSignature == _pollInteria)
            {

                for (int i = 0, rankWeight = 1; i < teams.Count; i++)
                {
                    WeightedTeam t = teams[i];
                    teamWeightMap.Add(t, rankWeight++ * multiplier);
                }
            } else
            {

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
                    teamWeightMap.Add(currTeam, rankWeight * multiplier);
                    prevTeam = currTeam;
                }
            }
            return teamWeightMap;
        }

        private List<WeightedTeam> LinkTeamsToGamesForWeek(int week, string season)
        {
            //Filter games
            List<SerGame> games = Season.Games.Where(g => Convert.ToInt32(g.Week) == week).Where(g => g.Season_Type == season).ToList();

            //Create Teams
            List<WeightedTeam> weightedTeams = season == postSeasonString ?
                weightedTeams = BuildTeamsForWeek(_finalWeek + 1) :
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
