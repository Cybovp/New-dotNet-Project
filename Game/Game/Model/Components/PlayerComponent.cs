using Game.Abtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model.Components
{
    public class PlayerComponent : Component
    {
        public Player Player { get; private set; }
        public PlayerComponent(Player player) 
        {
            Player = player;
        }
    }
}
