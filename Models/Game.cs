using static Battleships.Models.PlayerTypes;

namespace Battleships.Models
{
    public class Game
    {
        public PlayerType PlayerTurn { get; set; }
        public bool IsFinished { get; set; }
        public List<Player> Players = new List<Player>();
    }
}
