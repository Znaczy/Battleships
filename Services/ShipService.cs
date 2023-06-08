using Battleships.Models;
using System.Text.Json;

namespace Battleships.Services
{
    public class ShipService : IShipService
    {
        public List<Ship> CreateShips()
        {
            string ships = File.ReadAllText(@"..\..\..\ships.json");

            return JsonSerializer.Deserialize<List<Ship>>(ships);
        }
    }
}
