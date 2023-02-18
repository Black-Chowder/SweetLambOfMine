using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class WaveManager : Trait
    {
        public List<Entity> enemies;

        const int minSpawnDist = 1920;
        const int maxSpawnDist = minSpawnDist * 2;

        public int waveNum = 1;

        Random rand;

        const string classId = "WaveManager";
        public WaveManager(Entity parent) : base(classId, parent)
        {
            enemies = new List<Entity>();

            rand = new Random();
        }

        public override void Update(GameTime gameTime)
        {
            bool oneExists = enemies.Count > 0;
            int entityCount = 0;
            foreach (Entity entity in enemies)
            {
                if (!entity.exists)
                    continue;
                oneExists = true;
                entityCount++;
            }

            //Spawn in new wave of entities
            if (entityCount <= 0)
            {
                waveNum++;
                enemies.Clear();
                
                for (int i = 0; i < 2 * waveNum; i++)
                {
                    float dist = rand.Next(minSpawnDist, maxSpawnDist);
                    float angle = (float)(MathF.PI * 2 * rand.NextDouble());

                    Vector2 spawnPos = new Vector2(
                        MathF.Cos(angle) * dist,
                        MathF.Sin(angle) * dist);

                    BasicDemon basic = new BasicDemon(spawnPos);
                    enemies.Add(basic);
                    parent.batch.Add(basic);
                }
            }
        }
    }
}
