using GameManage.DAL.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace GameManage.DAL.Repositories
{
    public class GameRepository
    {
        private const string FilePath = "C:\\Users\\hypha\\OneDrive\\Documents\\PRN123\\GameManageApp\\game.js";  // Path to JSON file

        // Load all games from JSON file
        public List<Game> LoadGames()
        {
            if (!File.Exists(FilePath))
                return new List<Game>();  // Return an empty list if file doesn't exist

            var jsonData = File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<List<Game>>(jsonData) ?? new List<Game>();
        }

        // Save all games to JSON file
        public void SaveGames(List<Game> games)
        {
            var jsonData = JsonConvert.SerializeObject(games, Newtonsoft.Json.Formatting.Indented);
            File.WriteAllText(FilePath, jsonData);
        }

        // Get all games
        public List<Game> GetAllGames()
        {
            return LoadGames();
        }

        // Get a game by ID
        public Game GetGameById(int id)
        {
            var games = LoadGames();
            return games.FirstOrDefault(g => g.Id == id);
        }

        // Add a new game
        public void AddGame(Game game)
        {
            var games = LoadGames();
            game.Id = games.Any() ? games.Max(g => g.Id) + 1 : 1;  // Auto-increment ID
            games.Add(game);
            SaveGames(games);
        }

        // Update an existing game
        public void UpdateGame(Game updatedGame)
        {
            var games = LoadGames();
            var gameIndex = games.FindIndex(g => g.Id == updatedGame.Id);

            if (gameIndex >= 0)
            {
                games[gameIndex] = updatedGame;
                SaveGames(games);
            }
        }

        // Delete a game by ID
        public void DeleteGame(int id)
        {
            var games = LoadGames();
            var gameToRemove = games.FirstOrDefault(g => g.Id == id);

            if (gameToRemove != null)
            {
                games.Remove(gameToRemove);
                SaveGames(games);
            }
        }
    }
}
