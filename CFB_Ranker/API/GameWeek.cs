using CFB_Ranker.API.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CFB_Ranker.API
{
    public class GameWeek
    {
        public int Week { get; set; }
        public Dictionary<string, TeamStatsDTO> TeamStatsDTOMap { get; } = new();
        public Dictionary<string, GameInfoDTO> GameInfoDTOsMap { get; } = new();

        public GameWeek(int week, List<TeamStatsDTO> teamStatsDTOs, List<GameInfoDTO> gameInfoDTOs)
        {
            Week = week;

            AddToTeamStatsMap(teamStatsDTOs);
            AddToGameInfoDTOsMap(gameInfoDTOs);
        }

        private void AddToTeamStatsMap(List<TeamStatsDTO> teamStats)
        {
            foreach (var stats in teamStats)
            {
                TeamStatsDTOMap[stats.Id] = stats;
            }
        }
        private void AddToGameInfoDTOsMap(List<GameInfoDTO> gameInfos)
        {
            foreach (var gameInfo in gameInfos)
            {
                GameInfoDTOsMap[gameInfo.Id] = gameInfo;
            }
        }
    }
}
