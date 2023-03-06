using CFB_Ranker.API.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CFB_Ranker.API
{
    public class APIManager
    {
        private readonly string bearer = "gLQdG5n7YtiTjzu/bxxxd+rdzzrhWftHTtIH7PAGVWlAQMOAA7h2ria3ai2Dl9zc";
        private readonly string allTeamsUrl = "https://api.collegefootballdata.com/teams/fbs?year=2022";

        private readonly string teamGamesUrlString = "https://api.collegefootballdata.com/games/teams?year=2022"/*&seasonType=regular&week=#*/;
        private readonly string[] gamesUrlString =
                {"https://api.collegefootballdata.com/games?year=2022" /*&week=#&seasonType=regular*/, "&division=fbs"};

        public List<GameWeek> GameWeeks { get; } = new();

        public List<SchoolDTO> GetAllSchools()
        {
            string teams = GetJSONFromAPI(allTeamsUrl);
            return JsonConvert.DeserializeObject<List<SchoolDTO>>(teams)!;
        }

        public void GetDataForSeason()
        {
            string regSeason = "&seasonType=regular";
            string postSeason = "&seasonType=postseason";

            //RegularSeason weeks
            int startingWeek = 1, weeksInSeason = 15;
            for (int i = startingWeek; i <= weeksInSeason; i++)
            {
                AddToGameWeekCollection(regSeason, i);
            }

            //PostSeason week
            int postSeasonWeek = 1;
            AddToGameWeekCollection(postSeason, postSeasonWeek);

            void AddToGameWeekCollection(string seasonType, int week)
            {
                //Build URL strings
                string teamGamesUrl = teamGamesUrlString + seasonType + "&week=" + week;
                string gameInfoUrl = gamesUrlString[0] + "&week=" + week + seasonType + gamesUrlString[1];

                //Make API calls
                string gameStats = GetJSONFromAPI(teamGamesUrl);
                string gameInfos = GetJSONFromAPI(gameInfoUrl);

                //Convert JSONs to DTOs
                List<TeamStatsDTO> teamStatsDTOs = JsonConvert.DeserializeObject<List<TeamStatsDTO>>(gameStats)!;
                List<GameInfoDTO> gameInfoDTOs = JsonConvert.DeserializeObject<List<GameInfoDTO>>(gameInfos)!;

                //Adjust 'week'
                week = seasonType == postSeason ? GameWeeks.Count + (int) 1 : week;

                //Add to data structure
                GameWeeks.Add(new GameWeek(week, teamStatsDTOs, gameInfoDTOs));
            }
        }

        private string GetJSONFromAPI(string url) => APICaller.CallAPI(url, bearer);
    }
}
