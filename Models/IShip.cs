namespace Battleships.Models
{
    internal interface IShip
    {
        abstract string GetType();
        string Type { get; }
        List<string> Parts { get; set; }
        List<string> Neighbours { get; set; }
    }

}
