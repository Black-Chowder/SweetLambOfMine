using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class Floor : Entity
    {
        TileDrawer tileDrawer;

        public new const string classId = "Floor";
        public Floor(int[,] map, float x, float y, int tileSize, float tileScale) : base(new Vector2(x, y), classId)
        {
            Texture2D tilemap = Globals.content.Load<Texture2D>("tileset");
            Rectangle tileRectSpecs = new Rectangle(16, 16, 8, 14);
            tileDrawer = new TileDrawer(this, tilemap, tileRectSpecs, map, tileSize, tileScale);
            AddTrait(tileDrawer);
        }
    }
}
