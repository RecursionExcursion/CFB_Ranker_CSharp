using CFB_Ranker.AbstractModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFB_Ranker.API.DTO
{
    /*
     * DTO objects used to transfer raw data from JSON
     */
    public class TeamStatsDTO : AbstractTeamStats
    {
        public TeamStatsDTO(string id, TeamDTO[] teams) : base(id, teams) { }
    }

    public class TeamDTO : AbstractTeam
    {
        public TeamDTO(string school, int points, StatDTO[] stats) : base(school, points, stats) { }
    }

    public class StatDTO : AbstractStat
    {
        public StatDTO(string category, string stat) : base(category, stat) { }
    }
}
