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
                Shoot(game);

                ChangeTurn(game);

                CheckIfGameIsFinished(game);
            }

            Console.WriteLine("Game over!");
        }

        private static void Shoot(Game game)
        {
            string shot = string.Empty;

            Player attacker = game.Players.Where(p => p.PlayerType == game.PlayerTurn).FirstOrDefault();
            Player defender = game.Players.Where(p => p.PlayerType != game.PlayerTurn).FirstOrDefault();

            do
            {
                shot = Shoot();
                Console.WriteLine("Player {0}, shoots: {1}!", attacker.PlayerType, shot);

                if (!attacker.HitsReceived.Contains(shot))
                {
                    attacker.HitsReceived.Add(shot);
                    break;
                }
            } while (attacker.HitsReceived.Contains(shot));

            foreach (Ship ship in defender.Ships)
            {
                if (ship.Position.Contains(shot))
                {
                    ship.HitsReceived++;
                    if (ship.HitsReceived == ship.Size)
                    {
                        ship.IsSunk = true;
                        Console.WriteLine("Congratulations, {0}! You have sunk the {1}!", attacker.PlayerType, ship.Type);
                    }
                }
            }
        }

        private static string Shoot()
        {

            string[] letterAxis = new string[8] { "a", "b", "c", "d", "e", "f", "g", "h" };
            string[] numberAxis = new string[8] { "1", "2", "3", "4", "5", "6", "7", "8" };
            var rand = new Random();

            int rndLetterIndex = rand.Next(letterAxis.Length);
            int rndNumberIndex = rand.Next(numberAxis.Length);

            string letter = letterAxis[rndLetterIndex];
            string number = numberAxis[rndNumberIndex];

            return letter + number;
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
