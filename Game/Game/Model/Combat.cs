using Game.Abtract;
using Game.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game.Model
{
    public class Combat
    {
        private readonly List<ICombatListener> _listeners;
        public Player Player { get; private set; }
        public ICombatEntity Entity { get; private set; }
        public Combat(Player player, ICombatEntity entity)
        {
            _listeners = new List<ICombatListener>();
            Player = player;
            Entity = entity;
        }
        public void UseItem(IItem item)
        {
            if (!item.CanUse)
            {
                _listeners.MyForEach(f => f.DisplayMessage("Can't use: " + item.Name));
                return;
            }
            PerformAction(item.GetDamage(Entity));
        }
        public void UseAbility(IAbility ability)
        {
            PerformAction(ability.GetDamage(Entity));
        }
        private void PerformAction(Damage damage)
        {
            _listeners.ForEach(f => f.DisplayMessage(Entity.Name + " Took " + damage.Amount + " damage from " + damage.Text));

            Entity.TakeDamage(damage);
            if (Entity.Hp <= 0)
            {
                _listeners.MyForEach(f => f.DisplayMessage(Entity.Name + " died"));
                _listeners.MyForEach(f => f.EndCombat());
                return;
            }

            damage = Entity.GetDamage(Player);
            _listeners.MyForEach(f => f.DisplayMessage("Player Took " + damage.Amount + " damage from " + damage.Text));
            Player.TakeDamage(damage);

            if (Player.Hp <= 0)
                _listeners.MyForEach(f => f.PlayerDied());
        }
        public void AddListener(ICombatListener listener)
        {
            _listeners.Add(listener);
        }
        public void RemoveListener(ICombatListener listener)
        {
            _listeners.Remove(listener);
        }
    }
}
