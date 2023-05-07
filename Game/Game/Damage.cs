using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class Damage
    {
        public string Text { get; private set; }
        public int Amount { get; private set; }

        public Damage(string text, int amount)
        {
            Text = text;
            Amount = amount;
        }
        public Damage ModifyAmount(int delta)
        {
            return new Damage(Text, Amount + delta);
        }
    }
}
