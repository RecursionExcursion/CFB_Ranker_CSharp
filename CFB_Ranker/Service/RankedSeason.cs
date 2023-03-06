using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFB_Ranker.Service
{
    public class RankedSeason
    {
        public List<List<WeightedTeam>> Weeks { get; set; } = new();
    }
}
