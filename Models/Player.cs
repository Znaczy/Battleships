namespace Battleships.Models
{
    public class Player
    {
        public PlayerType PlayerType { get; set; }
        public List<Ship> Ships = new List<Ship>();
        public List<string> ShotsTaken = new List<string>();
        public List<string> Board = new List<string>();
    }
}
