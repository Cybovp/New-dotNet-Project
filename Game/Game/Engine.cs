using Game.Abtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Engine
    {
        private readonly Stack<IEngineState> _state;

        public bool IsRunning { get; private set; }

        public Engine() 
        {
            _state = new Stack<IEngineState>();
            IsRunning = true;
        }
        public void Quit()
        {
            IsRunning = false;
        }

        public void PushState (IEngineState state) 
        {
            if(_state.Count > 0)
                _state.Peek().Deactivate();

            _state.Push(state);
            state.Activate();
        }

        public void PopState (IEngineState state) 
        {
            if (_state.Count == 0 || state != _state.Peek())
                throw new InvalidOperationException("No state left on stack, or you are not poping a state you are not.");

            _state.Pop();
            state.Deactivate();
            state.Dispose();

            if(_state.Count > 0 )
                _state.Peek().Activate();
        }

        public void SwitchState (IEngineState state) 
        {
            if(_state.Count > 0)
            {
                var oldstate = _state.Pop();
                oldstate.Deactivate();
                oldstate.Dispose();
            }

            _state.Push(state);
            state.Activate();
        }

        public void ProcessInput(ConsoleKeyInfo key) 
        { 
            if(_state.Count > 0)
                _state.Peek().ProcessInput(key);
        }
    }
}
