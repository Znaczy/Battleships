using Battleships.Models;
using System.Text.RegularExpressions;
using static Battleships.Models.PlayerTypes;

namespace Battleships.Services
{
    public class PlayerService : IPlayerServices
    {
        private readonly IShipServices _shipServices;

        public PlayerService(IShipServices shipServices) 
        {
            _shipServices = shipServices;
        }

        public List<Player> CreatePlayers()
        {
            Player human = new Player() { Ships = _shipServices.CreateShips(), PlayerType = PlayerType.human };
            Player computer = new Player() { Ships = _shipServices.CreateShips(), PlayerType = PlayerType.computer };

            PopulateWithShipsManualy(human);
            PopulateWithShipsAutomaticly(computer);

            return new List<Player>() { human, computer };
        }

        private static void PopulateWithShipsAutomaticly(Player computer)
        {
            string[] letterAxis = new string[8] { "a", "b", "c", "d", "e", "f", "g", "h" };
            string[] numberAxis = new string[8] { "1", "2", "3", "4", "5", "6", "7", "8" };

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

        private static void PopulateWithShipsManualy(Player human)
        {
            Console.WriteLine("Please put your ships on board.");

            foreach (Ship ship in human.Ships)
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
    }
}
