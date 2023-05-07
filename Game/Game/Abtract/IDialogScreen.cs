using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Abtract
{
    public interface IDialogScreen
    {
        bool FinalScreen { get; }

        string Text { get; }

        Dictionary<string, IDialogScreen> NextScreens { get; }

        void EnterScreen(Entity entity);
    }
}
