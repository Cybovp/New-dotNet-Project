using Game.Abtract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class Ability : IAbility
    {
        private readonly int _damage;
        public string Name { get; private set; }
        public Ability(string name,int damage)
        {
            Name = name;
            _damage = damage;
        }

        public Damage GetDamage(ICombatEntity entity)
        {
            return new Damage(Name, _damage);
        }
    }
}
