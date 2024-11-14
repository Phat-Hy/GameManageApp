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
        private const string FilePath = "C:\\Users\\Mlxg\\source\\repos\\GameManageApp\\game.js";  // Path to JSON file

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
        public List<Game> Search(string name, string publisher)
        {
            var games = LoadGames();

            if (string.IsNullOrWhiteSpace(name) && string.IsNullOrWhiteSpace(publisher))
                return games;

            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(publisher))
                return games.Where(g => g.Name.ToLower().Contains(name.ToLower()) || g.Publisher.ToLower().Contains(publisher.ToLower())).ToList();

            if (!string.IsNullOrWhiteSpace(name))
                return games.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList();

            return games.Where(g => g.Publisher.ToLower().Contains(publisher.ToLower())).ToList();
        }

        public List<Game> SortGames(string sortOrder)
        {
            var games = LoadGames();

            return sortOrder switch
            {
                "NameAsc" => games.OrderBy(g => g.Name).ToList(),
                "NameDesc" => games.OrderByDescending(g => g.Name).ToList(),
                "DateAsc" => games.OrderBy(g => g.ReleaseDate).ToList(),
                "DateDesc" => games.OrderByDescending(g => g.ReleaseDate).ToList(),
                "PublisherAsc" => games.OrderBy(g => g.Publisher).ToList(),
                "PublisherDesc" => games.OrderByDescending(g => g.Publisher).ToList(),
                _ => games
            };
        }
    }
}
