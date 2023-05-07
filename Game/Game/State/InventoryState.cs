using Game.Abtract;
using Game.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.State
{
    public class InventoryState : IEngineState
    {
        private readonly Player _player;
        private int _selectedItem;
        public InventoryState(Player player)
        {
            _player = player;
        }
        public void ProcessInput(ConsoleKeyInfo key)
        {
            var equippedCount = _player.EquippedItems.Count();
            var inventoryCount = _player.Inventory.Count();
            var totalItemCount = equippedCount + inventoryCount;

            if (key.Key == ConsoleKey.Escape)
            {
                Program.Engine.PopState(this);
                return;
            }
            else if (key.Key == ConsoleKey.W)
            {
                if (_selectedItem > 0)
                    _selectedItem--;
            }
            else if (key.Key == ConsoleKey.S)
            {
                if (_selectedItem < totalItemCount - 1)
                    _selectedItem++;
            }
            else if (key.Key == ConsoleKey.Spacebar)
            {
                if (_selectedItem < equippedCount)
                {
                    var itemToUnequip = _player.EquippedItems.ElementAt(_selectedItem);
                    _player.UnequipItem(itemToUnequip);
                }
                else
                {
                    var itemToEquip = _player.Inventory.ElementAt(_selectedItem - equippedCount);
                    if (itemToEquip.CanEquip)
                        _player.EquipItem(itemToEquip);
                }
            }

            Render();
        }
        public void Activate()
        {
            Render();
        }

        public void Deactivate()
        {
        }

        public void Dispose()
        {
        }
        private void Render()
        {
            Console.Clear();
            Console.WriteLine("Player Inventory");
            Console.WriteLine("-------------------------------");

            var itemIndex = 0;

            foreach (var item in _player.EquippedItems)
            {
                if (itemIndex == _selectedItem)
                    ColorConsole(true);

                Console.WriteLine("[*] {0}", item.Name);
                ColorConsole(false);

                itemIndex++;
            }
            Console.WriteLine("-------------------------------");

            foreach (var item in _player.Inventory)
            {
                if (itemIndex == _selectedItem)
                    ColorConsole(true);

                Console.WriteLine("[ ] {0}", item.Name);
                ColorConsole(false);

                itemIndex++;
            }
        }
        private void ColorConsole(bool selected)
        {
            if (selected)
            {
                Console.ForegroundColor = ConsoleColor.Black;
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.BackgroundColor = ConsoleColor.Black;
            }
        }
    }
}
