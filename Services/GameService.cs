using Battleships.Models;

namespace Battleships.Services
{
    public class GameService : IGameService
    {
        private readonly IPlayerServices _playerService;

        public GameService (IPlayerServices playerService) 
        {
            _playerService = playerService;
        }

        public void PlayGame()
        {
            Console.WriteLine("Welcome to the game of battleships");
            Game game = new Game(){ Players = _playerService.CreatePlayers(), PlayerTurn = PlayerType.human };

            while (!game.IsFinished)
            {
                _playerService.Shoot(game);

                ChangeTurn(game);

                CheckIfGameIsFinished(game);
            }

            Console.WriteLine("Game over!");
        }



        private static void CheckIfGameIsFinished(Game game)
        {
            foreach (Player player in game.Players)
            {
                if (player.Ships.All(p => p.IsSunk == true))
                {
                    game.IsFinished = true;
                    Console.WriteLine("Congratulations!");
                }
            }
        }

        static void ChangeTurn(Game game)
        {
            if (game.PlayerTurn == PlayerType.human)
            {
                game.PlayerTurn = PlayerType.computer;
            }
            else
            {
                game.PlayerTurn = PlayerType.human;
            }
        }
    }
}
