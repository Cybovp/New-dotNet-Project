using Game.Abtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model.Components
{
    public class ConstantEntranceComponent : Component, IEntityEntranceComponent
    {
        private readonly bool _canEnter;
        public ConstantEntranceComponent(bool canEnter) 
        { 
            _canEnter = canEnter;
        }
        public bool CanEnter(Entity entity)
        {
            return _canEnter;
        }

        public void Enter(Entity entity)
        {

        }
    }
}
