using CFB_Ranker.Persistence.Serialization;
using CFB_Ranker.Service;

namespace CFB_Ranker_Tests
{
    public class RankingAlgoTest
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            Season season = new();

            SerSchool s1 = new SerSchool("1", "Team 1", null, null, null, null);
            SerSchool s2 = new SerSchool("2", "Team 2", null, null, null, null);
            SerSchool s3 = new SerSchool("3", "Team 3", null, null, null, null);
            SerSchool s4 = new SerSchool("4", "Team 4", null, null, null, null);


            SerGame s1vs2 = new SerGame("01", "2022", "1", "regular", null, "1", "2");
            s1vs2.TeamStats = new SerTeamStats("01", new SerTeam[] {
                new SerTeam("Team 1", 20, new SerStat[]{new ("totalYards", "400") }),
                new SerTeam("Team 2", 17, new SerStat[]{new ("totalYards", "300") })
            });


            SerGame s3vs4 = new SerGame("02", "2022", "1", "regular", null, "3", "4");
            s3vs4.TeamStats = new SerTeamStats("02", new SerTeam[] {
                new SerTeam("Team 3", 10, new SerStat[]{new ("totalYards", "200") }),
                new SerTeam("Team 4", 7, new SerStat[]{new ("totalYards", "100") })
            });


            season.Schools.AddRange(new[] { s1, s2, s3, s4 });
            season.Games.AddRange(new[] { s1vs2, s3vs4 });

            WeightDistributor weightDistributor = new(1, 1, 1, 1, 1, 1, 1, 1);

            RankingAlgorithm rankingAlgorithm = new RankingAlgorithm(season, weightDistributor);


            //Act
            rankingAlgorithm.RankTeams();
            RankedSeason weightedRankedSeason = rankingAlgorithm.WeightedRankedSeason;
            //Assert

            List<string> teamNameListForWeek1 = weightedRankedSeason.Weeks.First().Select(t => t.SchoolName).ToList();
            List<string> teamNameListForWeek1Base = new()
            {
                "Team 1",
                "Team 3",
                "Team 2",
                "Team 4"
            };

            for(int i = 0; i < teamNameListForWeek1.Count; i++)
            {
                Assert.Equal(teamNameListForWeek1[i], teamNameListForWeek1Base[i]);
            }
        }
    }
}