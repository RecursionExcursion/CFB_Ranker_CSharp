// See https://aka.ms/new-console-template for more information
using CFB_Ranker.API;
using CFB_Ranker.API.DTO;
using CFB_Ranker.Persistence;
using CFB_Ranker.Persistence.Serializable_Models;
using CFB_Ranker.Persistence.Serialization;
Console.WriteLine("Hello, World!");

string dir = @"C:\Users\rloup\Main Folder\Programming\Workspaces\VS-workspace\CFB_Ranker\CFB_Ranker\Persistence\Team_Data\";
string filename = "file.json";

Season? season = null;

if (File.Exists(dir + filename))
{
    season = SerializationManager.ReadJsonFromFile<Season>(dir + filename);
} else
{
    SerializationManager.LoadDateFromAPI();
    SerializationManager.WriteJsonToFile<Season>(dir + filename);
}


Console.WriteLine();

