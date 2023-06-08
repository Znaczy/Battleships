namespace Battleships.Models
{
    public class Ship 
    { 
        public string Type { get; set; }
        public bool IsSunk { get; set; }
        public int Size { get; set; }
        public List<string> Position = new List<string>();
        public int HitsReceived = 0;
    }
}
