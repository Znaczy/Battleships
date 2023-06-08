using static Battleships.Models.PlayerTypes;

namespace Battleships.Models
{
    public class Player
    {
        public PlayerType PlayerType { get; set; }
        public List<Ship> Ships = new List<Ship>();
        public List<string> HitsReceived = new List<string>();
    }
}
