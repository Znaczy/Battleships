using Battleships.Models;

namespace Battleships.Services
{
    public interface IPlayerServices
    {
        public List<Player> CreatePlayers();
        public void Shoot(Game game);
    }
}