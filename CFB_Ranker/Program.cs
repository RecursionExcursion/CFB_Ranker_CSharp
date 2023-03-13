// See https://aka.ms/new-console-template for more information
using CFB_Ranker.API;
using CFB_Ranker.API.DTO;
using CFB_Ranker.Persistence;
using CFB_Ranker.Persistence.Serializable_Models;
using CFB_Ranker.Persistence.Serialization;
using CFB_Ranker.Service;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        Season season = new PersistenceManager().LoadData();

        int Wins = 6;
        int Losses = 0;
        int PointsFor = 4;
        int PointsAllowed = 4;
        int TotalOffense = 1;
        int TotalDefense = 1;
        int ScheduleStrength = 2;
        int PollInertia = 5;

        WeightDistributor weightDistributor = new(Wins, Losses, PointsFor, PointsAllowed, TotalOffense, TotalDefense, ScheduleStrength, PollInertia);

        RankingAlgorithm rankingAlgorithm = new(season,weightDistributor);
        rankingAlgorithm.RankTeams();


        Console.WriteLine();
    }
}