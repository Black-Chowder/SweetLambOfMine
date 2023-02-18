using BlackMagic.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
                }
            }
        }
    }
}
