using Game.Abtract;
using Game.Model;
using Game.Model.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.State
{
    public class ZoneState : IEngineState
    {
        private readonly Zone _zone;
        private readonly ZoneRenderer _zoneRenderer;
        private readonly Entity _player;

        public ZoneState(Entity player, Zone zone)
        {
            _player = player;
            _zone = zone;
            _zoneRenderer = new ZoneRenderer(zone);

            _zone.AddListener(_zoneRenderer);
        }
        public void ProcessInput(ConsoleKeyInfo key)
        {
            var pos = _player.Position;

            if (key.Key == ConsoleKey.Escape)
                Program.Engine.PushState(new MainMenuState(_player.GetComponent<PlayerComponent>().Player));

            if (key.Key == ConsoleKey.W)
                _zone.MoveEntity(_player, new Vector3(pos.X, pos.Y - 1, pos.Z));

            if (key.Key == ConsoleKey.A)
                _zone.MoveEntity(_player, new Vector3(pos.X - 1, pos.Y, pos.Z));

            if (key.Key == ConsoleKey.S)
                _zone.MoveEntity(_player, new Vector3(pos.X, pos.Y + 1, pos.Z));

            if (key.Key == ConsoleKey.D)
                _zone.MoveEntity(_player, new Vector3(pos.X + 1, pos.Y, pos.Z));

        }
        public void Activate()
        {
            _zoneRenderer.IsActive = true;
            _zoneRenderer.RenderAll();
        }

        public void Deactivate()
        {
            _zoneRenderer.IsActive = false;
        }

        public void Dispose()
        {
            _zone.RemoveListener(_zoneRenderer);
        }
    }
}
