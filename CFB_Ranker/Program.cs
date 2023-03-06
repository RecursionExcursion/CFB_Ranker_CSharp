// See https://aka.ms/new-console-template for more information
using CFB_Ranker.API;
using CFB_Ranker.API.DTO;
using CFB_Ranker.Persistence;
using CFB_Ranker.Persistence.Serializable_Models;
using CFB_Ranker.Persistence.Serialization;
using CFB_Ranker.Service;

Console.WriteLine("Hello, World!");



Season season = new PersistenceManager().LoadData();


RankingAlgorithm rankingAlgorithm = new RankingAlgorithm(season);
rankingAlgorithm.RankTeams();


Console.WriteLine();

