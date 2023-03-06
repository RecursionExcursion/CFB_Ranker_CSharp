using CFB_Ranker.API;
using CFB_Ranker.API.DTO;
using CFB_Ranker.Persistence.Serialization;

namespace CFB_Ranker.Persistence
{
    public class SeasonMapper
    {
        private readonly APIManager apiManager = new();

        public Season BuildSeason()
        {
            return new Season()
            {
                Year = 2022,
                Schools = MapSchoolsToSerializableObjects(),
                Games = MapGamesToSerializableObjects()
            };
        }

        private List<SerSchool> MapSchoolsToSerializableObjects()
        {
            List<SchoolDTO> schoolDTOs = apiManager.GetAllSchools();
            List<SerSchool> schools = new();

            foreach (var dto in schoolDTOs)
            {
                SerSchool school = new(dto);
                schools.Add(school);
            }
            return schools;
        }

        private List<SerGame> MapGamesToSerializableObjects()
        {
            List<SerGame> serializableGames = new();
            apiManager.GetDataForSeason();

            List<GameWeek> gameWeeks = apiManager.GameWeeks;
            foreach (var week in gameWeeks)
            {
                Dictionary<string, GameInfoDTO> gameInfoDTOsMap = week.GameInfoDTOsMap;
                Dictionary<string, TeamStatsDTO> teamStatsDTOMap = week.TeamStatsDTOMap;

                List<SerGame> games = new();
                foreach (var gameId in gameInfoDTOsMap.Keys)
                {
                    //Constuct Serializable Game
                    SerGame game = new(gameInfoDTOsMap[gameId]);

                    TeamStatsDTO? teamStatsDTO;
                    if (teamStatsDTOMap.TryGetValue(gameId, out teamStatsDTO))
                    {
                        List<SerTeam> serTeamsArr = new();
                        foreach (var team in teamStatsDTO.Teams)
                        {
                            List<SerStat> stats = new();
                            foreach (var stat in team.Stats)
                            {
                                //Constuct Serializable Stat
                                SerStat s = new(stat.Category, stat.Stat);
                                stats.Add(s);
                            }
                            //Constuct Serializable Team
                            SerTeam t = new(team.School, team.Points, stats.ToArray());
                            serTeamsArr.Add(t);
                        }
                        //Constuct Serializable TeamStats (Both teams and game id)
                        SerTeamStats serTeamStats = new(teamStatsDTO.Id, serTeamsArr.ToArray());
                        game.TeamStats = serTeamStats;
                        serializableGames.Add(game);
                    }
                }
            }
            return serializableGames;
        }
    }
}
