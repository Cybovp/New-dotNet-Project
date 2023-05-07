using Game.Model;
using Game.Model.Components;
using Game.State;
using System;
using System.Security.AccessControl;
using System.Security.Cryptography.X509Certificates;

namespace Game
{
    public static class Program
    {
        public static Engine Engine { get; private set; }
        public static Player playerModel = new Player();
        private static void Main()
        {
            var random = new Random();

            const int ZoneWidth = 50;
            const int ZoneHeight = 30;

            Console.BufferWidth = Console.WindowWidth = ZoneWidth;
            Console.BufferHeight = Console.WindowHeight = ZoneHeight + 1;

            //Add player
            
            playerModel.AddAbility(new Ability("Fire Ball!", 10));
            playerModel.AddAbility(new Ability("Fire Storm!", 100));
            playerModel.AddItem(new Item("Sword", false, true, totalDamage: 30));
            playerModel.AddItem(new Item("Bow", false, true, totalDamage: 10));

            var player = new Entity();
            player.AddComponent(new SpriteComponent { Sprite = 'P' });
            player.AddComponent(new PlayerComponent(playerModel ));
            player.Position = new Vector3(2, 2, 1);

            //Add dialog npc
            var npc1 = new Entity();
            npc1.AddComponent(new DialogComponent(new Dialog
            (
                new DialogScreen("Hey there!", nextScreens: new Dictionary<string, Abtract.IDialogScreen>
                {
                    {"option 1", new DialogScreen("WOOT!", e => Console.WriteLine("ACTION 1")) },
                    {"option 2", new DialogScreen("STUFF AND THING!", e => Console.WriteLine("ACTION 2"), new Dictionary<string, Abtract.IDialogScreen>
                    {
                        {"More Stuff", new DialogScreen("FINAL SCREEN") }
                    })
                    }
                })
            )));
            npc1.AddComponent(new SpriteComponent { Sprite = '!'});
            npc1.Position = new Vector3(35, 5, 0);

            //Add item npc
            var npc2 = new Entity();
            npc2.AddComponent(new DialogComponent(new Dialog
            (
                new DialogScreen("Have this item!",
                    e => 
                    {
                        var defence = random.Next(-10, -1);
                        e.GetComponent<PlayerComponent>().Player.AddItem(new Item("Armor - " + Math.Abs(defence), true, false, defence));
                    })
            )));
            npc2.AddComponent(new SpriteComponent { Sprite = '?' });
            npc2.Position = new Vector3(10, 2, 0);

            //Add enemy
            var enemy = new Entity();
            enemy.AddComponent(new SpriteComponent { Sprite = 'E' });
            enemy.AddComponent(new CombatComponent(() => new Combat(playerModel, new BasicMob())));
            enemy.Position = new Vector3(13, 2, 0);

            //var wall = new Entity();
            //wall.AddComponent(new ConstantEntranceComponent(false));
            //wall.Position = new Vector3(5, 5, 0);

            //var tallGrass = new Entity();adadd
            //tallGrass.AddComponent(new SpriteComponent { Sprite = '#' });
            //tallGrass.Position = new Vector3(3, 3, 0);

            var zone1 = new Zone("Zone 1", new Vector3(ZoneWidth, ZoneHeight, 3));
            zone1.AddEntity(player);
            zone1.AddEntity(npc1);
            zone1.AddEntity(npc2);
            zone1.AddEntity(enemy);
            //zone1.AddEntity(wall);
            //zone1.AddEntity(tallGrass);
            //Add tall grass
            AddSpriteArea(3, 3, 10, 10, 0, true,'#',true,zone1 );
            //Add ceiling
            AddSpriteArea(15, 3, 10, 5, 2, true, '@',false, zone1);
            //Add vertical wall
            AddSpriteArea(30, 1, 1, 10, 0, false,'*',false,zone1);
            //Add horizontal wall
            AddSpriteArea(30, 11, 10, 1, 0, false, '*',false, zone1);

            Engine = new Engine();
            Engine.PushState(new ZoneState(player,zone1));

            while (Engine.IsRunning)
                Engine.ProcessInput(Console.ReadKey(true));
        }
        public static void AddSpriteArea(int initialX, int initialY, int length, int width,int level,bool canEnter, char sprite, bool isEnemy, Zone zone)
        {
            for (var i = initialX; i < initialX + length; i++)
                for (var j = initialY; j < initialY + width; j++)
                {
                    var entity = new Entity();
                    entity.AddComponent(new SpriteComponent { Sprite = sprite });
                    if (!canEnter )
                        entity.AddComponent(new ConstantEntranceComponent(false));
                    if (isEnemy)
                        entity.AddComponent(new CombatComponent(() => new Combat(playerModel, new BasicMob())));
                    entity.Position = new Vector3(i, j, level);

                    zone.AddEntity(entity);
                }
        }
    }
}