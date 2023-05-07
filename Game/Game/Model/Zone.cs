using Game.Abtract;
using Game.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class Zone
    {
        private readonly Entity[,,] _entities;
        public Vector3 Size { get; set; }

        public string Name { get; set; }

        public IEnumerable<Entity> Entities
        {
            get
            {
                for (var x = 0; x < Size.X; x++)
                    for (var y = 0; y < Size.Y; y++)
                        for (var z = 0; z < Size.Z; z++)
                        {
                            var entity = _entities[x, y, z];
                            if (entity == null)
                                continue;

                            yield return entity;
                        }
                            
            }
        
        }

        private readonly HashSet<IZoneListener> _Listeners;

        public Zone(string name, Vector3 size)
        {
            _Listeners = new HashSet<IZoneListener>();
            Size = size;
            Name = name;
            _entities = new Entity[size.X, size.Y, size.Z];
        }
        public void MoveEntity(Entity entity, Vector3 newPosition)
        {
            if (newPosition.X < 0 || newPosition.X >= Size.X ||
                newPosition.Y < 0 || newPosition.Y >= Size.Y ||
                newPosition.Z < 0 || newPosition.Z >= Size.Z)
                return;

            var topMostEntity = GetTopMostEntity(newPosition);
            if (topMostEntity != null)
            {
                var component = topMostEntity.GetComponent<IEntityEntranceComponent>();
                if (component != null )
                {                  
                    if(!component.CanEnter(entity))
                        return;

                    component.Enter(entity);
                }

            }

            _Listeners.MyForEach(L => L.EntityMoved(entity, newPosition));
            _entities[entity.Position.X, entity.Position.Y, entity.Position.Z] = null;
            entity.Position = newPosition;
            _entities[entity.Position.X, entity.Position.Y, entity.Position.Z] = entity;
        }
        public void AddEntity(Entity entity)
        {
            _Listeners.MyForEach(l => l.EntityAdd(entity));
            _entities[entity.Position.X, entity.Position.Y, entity.Position.Z] = entity;
        }
        public void RemoveEntity(Entity entity)
        {
            _Listeners.MyForEach(l => l.EntityRemove(entity));
            var oldEntity = _entities[entity.Position.X, entity.Position.Y, entity.Position.Z];
            if (oldEntity != entity)
                throw new InvalidOperationException("Entity position is out of sync");
            _entities[entity.Position.X, entity.Position.Y, entity.Position.Z] = null;
        }
        public void AddListener(IZoneListener listener)
        {
            if (!_Listeners.Add(listener))
                throw new ArgumentException();
        }
        public void RemoveListener(IZoneListener listener)
        {
            if (_Listeners.Remove(listener))
                throw new ArgumentException();
        }

        private Entity GetTopMostEntity(Vector3 position)
        {
            for (var i = Size.Z - 1; i >=0; i--)
            {
                var entity = _entities[position.X, position.Y, i];
                if (entity != null) return entity;
            }
            return null;
        }
    }
}
