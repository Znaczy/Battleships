using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleships.Models
{
    internal interface IPlayer
    {
        enum Type
        {
            Human,
            Computer
        }

        enum ShipType
        {

        }

        List<Ship> Ships { get; }

    }
}
