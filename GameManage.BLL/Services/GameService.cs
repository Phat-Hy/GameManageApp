using GameManage.DAL.Entities;
using GameManage.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameManage.BLL.Services
{
    public class GameService
    {
        private readonly GameRepository _gameRepository;

        // Constructor that initializes GameRepository
        public GameService()
        {
            _gameRepository = new GameRepository();
        }

        // Get all games
        public List<Game> GetAllGames()
        {
            return _gameRepository.GetAllGames();
        }

        // Get a game by ID
        public Game GetGameById(int id)
        {
            return _gameRepository.GetGameById(id);
        }

        // Add a new game
        public void AddGame(Game game)
        {
            // Additional business logic can be added here, if needed
            _gameRepository.AddGame(game);
        }

        // Update an existing game
        public void UpdateGame(Game updatedGame)
        {
            var existingGame = _gameRepository.GetGameById(updatedGame.Id);
            if (existingGame == null)
                throw new ArgumentException("Game not found");

            _gameRepository.UpdateGame(updatedGame);
        }

        // Delete a game by ID
        public void DeleteGame(int id)
        {
            var existingGame = _gameRepository.GetGameById(id);
            if (existingGame == null)
                throw new ArgumentException("Game not found");

            _gameRepository.DeleteGame(id);
        }

        public List<Game> SearchByNameOrPublisher(string name, string publisher)
        {
            return _gameRepository.Search(name, publisher);
        }

        public List<Game> SortGames(string sort) => _gameRepository.SortGames(sort);
    }
}
