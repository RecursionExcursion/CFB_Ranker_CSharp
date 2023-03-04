// See https://aka.ms/new-console-template for more information
using CFB_Ranker.API;
using CFB_Ranker.API.DTO;
using CFB_Ranker.Persistence;
using CFB_Ranker.Persistence.Serializable_Models;
using CFB_Ranker.Persistence.Serialization;
Console.WriteLine("Hello, World!");




PersistenceManager manager = new();
Season season = manager.LoadData();



Console.WriteLine();

