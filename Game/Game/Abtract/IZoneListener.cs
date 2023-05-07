using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Abtract
{
    public interface IZoneListener
    {
        void EntityMoved(Entity enity, Vector3 newPosition);
        void EntityAdd(Entity entity);
        void EntityRemove(Entity entity);
    }
}
