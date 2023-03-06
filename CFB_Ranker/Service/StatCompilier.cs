using CFB_Ranker.AbstractModels;
using CFB_Ranker.Persistence.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFB_Ranker.Service
{
    public class StatCompilier
    {
        public static void ComplileStatsForTeam(SerGame game, WeightedTeam team)
        {
            AbstractTeam thisTeam = game.TeamStats.Teams.Where(t => t.School == team.SchoolName).First();
            AbstractTeam oppTeam = game.TeamStats.Teams.Where(t => t.School != team.SchoolName).First();

            string totalYards = "totalYards";

            //Add game to schedule
            team.Schedule.Add(game);

            //Allocate Points
            team.TotalPointsFor += thisTeam.Points;
            team.TotalPointsAllowed += oppTeam.Points;

            if (thisTeam.Points > oppTeam.Points)
            {
                team.Record.Wins++;
            } else
            {
                team.Record.Losses++;
            }

            //Allocate Yards
            string teamYards;
            string oppYards;

            try
            {
                teamYards = thisTeam.Stats.Where(s => s.Category == totalYards).Select(s => s.Stat).First();
                oppYards = oppTeam.Stats.Where(s => s.Category == totalYards).Select(s => s.Stat).First();

            } catch (InvalidOperationException e)
            {
                Console.WriteLine($"Data is missing from a data structure, {game.Id}- week {game.Week}");

                string teamHomeOrAway = team.School.Id == game.Home_Id ? "H" : "A";
                string oppHomeOrAway = teamHomeOrAway == "H" ? "A" : "H";

                teamYards = GameNotComplete(game.Id, teamHomeOrAway);
                oppYards = GameNotComplete(game.Id, oppHomeOrAway);
            }
            team.TotalOffense += Convert.ToInt32(teamYards);
            team.TotalDefense += Convert.ToInt32(oppYards);
        }
        private static string GameNotComplete(string gameId, string homeOrAway)
        {
            //Replace with WebScraper?, currently manually handling errors
            string[] buffAkrWk14 = new string[]
           {
                //Game ID
                "401506450",
                //Home Yards
                "291",
                //Away Yards
                "301"
           };

            List<string[]> gamesWithoutStats = new() { buffAkrWk14 };

            foreach (var game in gamesWithoutStats)
            {
                if (game[0] == gameId)
                {
                    return homeOrAway == "H" ? game[1] : game[2];
                }
            }
            throw new Exception("Game was not found");
        }

    }
}

