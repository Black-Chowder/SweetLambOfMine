using BlackMagic.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BlackMagic
{
    public class MapLoader
    {
        private EntityBatch parentBatch;
        public MapLoader(EntityBatch parent)
        {
            this.parentBatch = parent;
        }

        public void Load(byte[] byteMap)
        {
            TiledMapData mapData = JsonSerializer.Deserialize<TiledMapData>(byteMap);

            foreach (LayerData layer in mapData.layers)
            {
                switch (layer.name)
                {
                    case "Floors":
                        foreach (ChunkData chunk in layer.chunks)
                        {
                            Floor floor = new Floor(
                                chunk.data.Cast<int>().ToArray().ToMatrix(16).Transpose(), 
                                chunk.x * mapData.tilewidth, 
                                chunk.y * mapData.tileheight, 
                                mapData.tilewidth, 
                                2f);

                            parentBatch.Add(floor);
                        }
                        break;

                    case "Walls":
                        foreach (ChunkData chunk in layer.chunks)
                        {
                            Wall wall = new Wall(
                                chunk.data.Cast<int>().ToArray().ToMatrix(16).Transpose(),
                                chunk.x * mapData.tilewidth,
                                chunk.y * mapData.tileheight,
                                mapData.tilewidth,
                                2f);

                            parentBatch.Add(wall);
                        }

                        break;

                    case "PlayerSpawner":
                        foreach (ChunkData chunk in layer.chunks)
                        {
                            for (int r = 0; r < chunk.height; r++)
                            {
                                for (int c = 0; c < chunk.width; c++)
                                {
                                    int chunkData = (int)chunk.data[c + r * chunk.width];
                                    if (chunkData == 0)
                                        continue;

                                    Lamb lamb = new Lamb(new Vector2(
                                        (c + chunk.x) * 16 * 2f,
                                        (r + chunk.y) * 16 * 2f));
                                    parentBatch.Add(lamb);
                                    parentBatch.player = lamb;
                                }
                            }
                        }

                        break;

                    case "EnemySpawner":
                        foreach (ChunkData chunk in layer.chunks)
                        {
                            for (int r = 0; r < chunk.height; r++)
                            {
                                for (int c = 0; c < chunk.width; c++)
                                {
                                    int chunkData = (int)chunk.data[c + r * chunk.width];
                                    if (chunkData == 0)
                                        continue;

                                    BasicDemon demon = new BasicDemon(new Vector2(
                                        (c + chunk.x) * 16 * 2f,
                                        (r + chunk.y) * 16 * 2f));
                                    parentBatch.Add(demon);

                                }
                            }
                        }

                        break;
                }
            }
        }
    }
}
