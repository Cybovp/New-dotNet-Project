using Game.Abtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class DialogScreen : IDialogScreen
    {
        private Action<Entity> _enterAction;
        public bool FinalScreen { get { return NextScreens.Count == 0; } }
        public string Text { get; private set; }
        public Dictionary<string, IDialogScreen> NextScreens { get; private set; }

        public DialogScreen(string text, Action<Entity> enterAction = null, Dictionary<string, IDialogScreen> nextScreens = null)
        {
            Text = text;    
            _enterAction = enterAction;

            NextScreens = nextScreens ?? new Dictionary<string, IDialogScreen>();
        }
        public void EnterScreen(Entity entity)
        {
            if (_enterAction != null)
                _enterAction(entity);
        }
    }
}
