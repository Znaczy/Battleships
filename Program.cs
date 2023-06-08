using Battleships.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Battleships
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var services = new ServiceCollection();

            var builder = Host.CreateDefaultBuilder(args);

            builder.ConfigureServices(
            services => services
            .AddScoped<IGameService, GameService>()
            .AddScoped<IPlayerServices, PlayerService>()
            .AddScoped<IShipService, ShipService>()
            );

            using var host = builder.Build();
            var srv = host.Services.GetService<IGameService>();
            srv.PlayGame();
        }
    }
}