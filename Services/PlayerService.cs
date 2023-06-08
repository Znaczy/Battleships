using Battleships.Models;
using System.Text.RegularExpressions;

namespace Battleships.Services
{
    public class PlayerService : IPlayerServices
    {
        private readonly IShipService _shipServices;

        static string[] letterAxis = new string[8] { "a", "b", "c", "d", "e", "f", "g", "h" };
        static string[] numberAxis = new string[8] { "1", "2", "3", "4", "5", "6", "7", "8" };

        public PlayerService(IShipService shipServices) 
        {
            _shipServices = shipServices;
        }

        public List<Player> CreatePlayers()
        {
            Player human = new Player() { Ships = _shipServices.CreateShips(), PlayerType = PlayerType.human };
            Player computer = new Player() { Ships = _shipServices.CreateShips(), PlayerType = PlayerType.computer };

            //PopulateWithShipsManually(human);
            PopulateWithShipsAutomatically(human);

            PopulateWithShipsAutomatically(computer);

            return new List<Player>() { human, computer };
        }

        private static void PopulateWithShipsAutomatically(Player computer)
        {
            foreach (Ship ship in computer.Ships)
            {
                int j = 0;
                for (int i = 0; i < ship.Size; i++)
                {
                    ship.Position.Add(letterAxis[i] + numberAxis[j]);
                }
                j++;
            }
        }

        public void Shoot(Game game)
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
            var rand = new Random();

            int rndLetterIndex = rand.Next(letterAxis.Length);
            int rndNumberIndex = rand.Next(numberAxis.Length);

            string letter = letterAxis[rndLetterIndex];
            string number = numberAxis[rndNumberIndex];

            return letter + number;
        }


        private static void PopulateWithShipsManually(Player human)
        {
            Console.WriteLine("Please put your ships on board.");

            foreach (Ship ship in human.Ships)
            {
                BuildShip(ship);
            }
        }

        private static void BuildShip(Ship ship)
        {
            Console.WriteLine("Where would you like to put your {0}?", ship.Type);
            Console.WriteLine(@"Write letter from a to h and number between 1 and 8 (e.g. 'b4'), and press enter.");

            for (int i = 0; i < ship.Size; i++)
            {
                string? input = Console.ReadLine();

                while (string.IsNullOrEmpty(input) || !Regex.IsMatch(input, @"[a-hA-H][1-8]"))
                {
                    Console.WriteLine("Input was not valid. Please try again.");
                    Console.WriteLine(@"Write letter from a to h and number between 1 and 8 (e.g. 'b4'), and press enter.");
                    input = Console.ReadLine();
                }

                ship.Position.Add(input);

                var neighbours = FindNeighbours(input);
                Console.WriteLine(@"Which one do you want to be next?");

                foreach (string neighbour in neighbours)
                {
                    Console.WriteLine(neighbour + " ");
                }
            }
        }

        private static List<string> FindNeighbours(string? input)
        {
            string letter = input.Substring(0, 1);
            string number = input.Substring(1, 1);

            int letterIndex = Array.IndexOf(letterAxis, letter);
            int numberIndex = Array.IndexOf(numberAxis, number);

            List<string> neighbours = new List<string>();

            if (letterIndex != 7)
            {
                neighbours.Add(letterAxis[letterIndex + 1] + numberAxis[numberIndex]);
            }

            if (letterIndex != 0)
            {
                neighbours.Add(letterAxis[letterIndex - 1] + numberAxis[numberIndex]);
            }

            if (numberIndex != 0)
            {
                neighbours.Add(letterAxis[letterIndex] + numberAxis[numberIndex - 1]);
            }

            if (numberIndex != 7)
            {
                neighbours.Add(letterAxis[letterIndex] + numberAxis[numberIndex + 1]);
            }

            return neighbours;
        }
    }
}

