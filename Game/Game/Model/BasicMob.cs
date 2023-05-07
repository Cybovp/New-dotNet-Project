using Game.Abtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class BasicMob : ICombatEntity
    {
        public string Name { get { return "Basic Mob"; } }

        public int Hp { get; private set; }

        public BasicMob() { Hp = 100; }   
        public Damage GetDamage(Player player)
        {
            return new Damage("Punch!", 10);
        }

        public void TakeDamage(Damage damage)
        {
            Hp -= damage.Amount;
        }
    }
}
