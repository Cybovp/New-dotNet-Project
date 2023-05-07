using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Abtract
{
    public abstract class Component : IComponent
    {
        public Entity Parent { get ; set ; }

    }
}
