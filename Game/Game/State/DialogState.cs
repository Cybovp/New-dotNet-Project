using Game.Abtract;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.State
{
    public class DialogState : IEngineState
    {
        private readonly Entity _instigator;
        private readonly IDialog _dialog;
        private IDialogScreen _currentScreen;
        private int _dialogHeight;
        private int _selectedOption;
        private List<Tuple<string, IDialogScreen>> _optionList;
        public DialogState(Entity instigator,IDialog dialog) 
        { 
            _dialog = dialog;
            _instigator = instigator;
            SwitchScreen(_dialog.FirstScreen);
        }
        public void ProcessInput(ConsoleKeyInfo key)
        {
            if (_optionList.Count != 0)
            {
                ColorConsole(false);
                Console.SetCursorPosition(0, _dialogHeight + _selectedOption);
                Console.WriteLine(_optionList[_selectedOption].Item1);
            }

            if (key.Key == ConsoleKey.W)
            {
                if (_selectedOption > 0)
                    _selectedOption--;
            }
            else if (key.Key == ConsoleKey.S)
            {
                if (_selectedOption < _optionList.Count - 1)
                    _selectedOption++;
            }
            else if (key.Key == ConsoleKey.Enter || key.Key == ConsoleKey.Spacebar)
            {
                if (_currentScreen.FinalScreen)
                    Program.Engine.PopState(this);
                else
                {
                    var nextScreen = _optionList[_selectedOption].Item2;
                    SwitchScreen(nextScreen);
                    RenderScreen();
                }
            }
            if (_optionList.Count != 0) 
            {
                ColorConsole(true);
                Console.SetCursorPosition(0, _dialogHeight + _selectedOption);
                Console.WriteLine(_optionList[_selectedOption].Item1);
            }            
        }
        public void Activate()
        {
            RenderScreen();
        }

        public void Deactivate()
        {
        }

        public void Dispose()
        {
        }
        private void SwitchScreen(IDialogScreen screen)
        {
            _currentScreen = screen;
            _optionList = _currentScreen.NextScreens.Select(kv => Tuple.Create(kv.Key, kv.Value)).ToList();
            _selectedOption = 0;
            _currentScreen.EnterScreen(_instigator);
        }
        private void RenderScreen()
        {
            Console.Clear();
            Console.WriteLine("DIALOG");
            Console.WriteLine("------------------------------------------------");
            Console.WriteLine(_currentScreen.Text);
            Console.WriteLine("------------------------------------------------");
            _dialogHeight = Console.CursorTop;

            var index = 0;

            foreach (var kv in _optionList)
            {
                if (index == 0)
                    ColorConsole(true);

                Console.WriteLine(kv.Item1);

                if (index++ == 0)
                    ColorConsole(false);
            }

            if (_currentScreen.FinalScreen)
            {
                ColorConsole(true);
                Console.WriteLine("Exit Dialog");
                ColorConsole(false);
            }
        }
        private void ColorConsole(bool selected)
        {
            if (selected)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor= ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}
