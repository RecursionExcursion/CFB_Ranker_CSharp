using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFB_Ranker.Service
{
    public class WeightDistributor
    {
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int PointsFor { get; set; }
        public int PointsAllowed { get; set; }
        public int TotalOffense { get; set; }
        public int TotalDefense { get; set; }
        public int ScheduleStrength { get; set; }
        public int PollInertia { get; set; }

        public WeightDistributor(int wins, int loss, int pointsFor, int pointsAllowed, int totalOffense, int totalDefense, int scheduleStrength, int pollInertia)
        {
            Wins = wins;
            Losses = loss;
            PointsFor = pointsFor;
            PointsAllowed = pointsAllowed;
            TotalOffense = totalOffense;
            TotalDefense = totalDefense;
            ScheduleStrength = scheduleStrength;
            PollInertia = pollInertia;
        }
    }
}
