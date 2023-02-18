using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BlackMagic
{
    public class TileDrawer : Trait, TDraws
    {
        private int[,] map;

        //Stores the size of each tile on the tilemap
        public int tileSize { get; private set; }

        //Multiplier for tileSize when drawing
        public float tileScale { get; private set; } = 1f;

        private Texture2D tilemap = null;
        private Rectangle[] tileRects;

        public Camera camera { get; set; }

        const string classId = "TileDrawer";
        public TileDrawer(Entity parent, Texture2D texture, Rectangle tileRectSpecs, int[,] map, int tileSize, float tileScale) : base(classId, parent)
        {
            this.map = map;
            this.tileSize = tileSize;
            this.camera = Globals.Camera;

            this.tileSize = tileSize;
            this.tileScale = tileScale;

            this.tilemap = texture;
            this.tileRects = DrawUtils.spriteSheetLoader(tileRectSpecs.X, tileRectSpecs.Y, tileRectSpecs.Width, tileRectSpecs.Height);
        }

        public override void Update(GameTime gameTime) { }

        public void Draw()
        {
            //Draw Tiles
            /*
             * The following code draws tiles to the screen but only the tiles that are
             * visible to the camera.  It does this -[using voodoo magic]- no.  It does
             * this by calculating which tile in the tileset to start drawing from and
             * which tile to end at and only draws those tiles.
             */

            float x = parent.X;
            float y = parent.Y;

            float maxR = ((camera.X + camera.Width) / tileScale - x) / tileSize;
            float maxC = ((camera.Y + camera.Height) / tileScale - y) / tileSize;

            for (int r = (int)MathF.Max(MathF.Ceiling((camera.X / tileScale - x) / tileSize) - 1, 0);
                r < map.GetLength(0) && r < maxR;
                r++)
            {

                for (int c = (int)MathF.Max(MathF.Ceiling((camera.Y / tileScale - y) / tileSize) - 1, 0);
                    c < map.GetLength(1) && c < maxC;
                    c++)
                {

                    if (map[r, c] == 0)
                        continue;

                    Globals.spriteBatch.Draw(tilemap,
                        new Vector2(
                            (x + r * tileSize) * tileScale - camera.X,
                            (y + c * tileSize) * tileScale - camera.Y),
                        tileRects[map[r, c] - 1],
                        Color.White,
                        0,
                        new Vector2(0, 0),
                        tileScale,
                        SpriteEffects.None,
                        0f);
                }
            }
        }
    }
}
