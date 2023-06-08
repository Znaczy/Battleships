using Battleships.Models;
using System.Text.Json;

namespace Battleships
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to the game of battleships");
            PlayGame();
        }

        public static List<Ship> CreateShips()
        {
            string ships = File.ReadAllText(@"..\..\..\ships.json");

            return JsonSerializer.Deserialize<List<Ship>>(ships);
        }

        public static List<Player> CreatePlayers()
        {
            Player human = new Player() { Ships = CreateShips(), PlayerType = PlayerType.human };
            Player computer = new Player() { Ships = CreateShips(), PlayerType = PlayerType.computer };

            PrepareBoard(human);
            PrepareBoard(computer);

            return new List<Player>() { human, computer };
        }

        static void PrepareBoard(Player player)
        {
            string[] letterAxis = new string[8] { "a", "b", "c", "d", "e", "f", "g", "h" };
            string[] numberAxis = new string[8] { "1", "2", "3", "4", "5", "6", "7", "8" };

            player.Board = CreateBoard(letterAxis, numberAxis);

            if(player.PlayerType == PlayerType.computer)
            {
                PopulateWithShips(numberAxis, letterAxis, player);
            }
            else
            {
                PopulateWithShips(player);
            }
        }

        private static void PopulateWithShips(string[] numberAxis, string[] letterAxis, Player computer)
        {
            foreach (Ship ship in computer.Ships)
            {
                int j = 0;
                for (int i = 0; i < ship.Size; i++)
                {
                    ship.Parts.Add(letterAxis[i] + numberAxis[j]);
                }
                j++;
            }
        }

        private static void PopulateWithShips(Player human)
        {
            Console.WriteLine("Please put your ships on board.");
            
            foreach (Ship ship in human.Ships)
            {
                Console.WriteLine("Where would you like to put your {0}?", ship.Type);
                Console.WriteLine(@"Write letter from a to h and number between 1 and 8 (e.g. 'b4'), and press enter.");
                
                for (int i=0; i < ship.Size; i++)
                {
                    string? input = Console.ReadLine();

                    FindNeighbours(input);
                }
            }
        }

        private static List<string> FindNeighbours(string? input)
        {
            string[] letterAxis = new string[8] { "a", "b", "c", "d", "e", "f", "g", "h" };
            string[] numberAxis = new string[8] { "1", "2", "3", "4", "5", "6", "7", "8" };
            

            string letter = input.Substring(0, 1);
            string number = input.Substring(1, 1);


            int letterIndex = 0;

            return new List<string>() { letter + number };
        }

        public static List<string> CreateBoard(string[] letterAxis, string[] numberAxis)
        {
            List<string> board = new List<string>();

            for (int i = 0; i < letterAxis.Length; i++)
            {
                for (int j = 0; j < numberAxis.Length; j++)
                {
                    board.Add(letterAxis[i] + numberAxis[j]);
                }
            }

            return board;
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

        static void PlayGame()
        {
            Game game = new Game() { Players = CreatePlayers(), PlayerTurn = PlayerType.human };

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

                if (!attacker.ShotsTaken.Contains(shot))
                {
                    attacker.ShotsTaken.Add(shot);
                    break;
                }
            }while(attacker.ShotsTaken.Contains(shot));

            foreach(Ship ship in defender.Ships) 
            {
                if (ship.Parts.Contains(shot))
                {
                    ship.ShotsTaken++;
                    if(ship.ShotsTaken == ship.Size)
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
            foreach(Player player in game.Players) 
            {
                if (player.Ships.All(p => p.IsSunk == true))
                {
                    game.IsFinished = true;
                    Console.WriteLine("Congratulations!");
                }
            }
        }
    }
}