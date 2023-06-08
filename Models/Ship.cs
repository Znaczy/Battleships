using System.Text.Json;

namespace Battleships.Models
{
    public class Ship
    { 
        public string Type { get; set; }
        public bool IsSunk { get; set; }
        public int Size { get; set; }
        public List<string> Parts = new List<string>();
        public int ShotsTaken = 0;

        public List<Ship> CreateShips()
        {
            string ships = File.ReadAllText(@"..\..\..\ships.json");

            return JsonSerializer.Deserialize<List<Ship>>(ships);
        }
    }

}
