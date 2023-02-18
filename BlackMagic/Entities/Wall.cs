using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class Wall : Entity
    {
        public Rigidbody rb;
        TileDrawer tileDrawer;
        private List<HitPoly> hitPolys = new List<HitPoly>();
        private List<Rectangle> rects;


        //Stores the size of each tile on the tilemap texture
        public int tileSize { get; private set; }

        //Multiplier for tileSize when drawing
        public float tileScale { get; private set; } = 1f;

        private static Texture2D tilemap = null;


        public new const string classId = "Wall";
        public Wall(int[,] map, float x, float y, int tileSize, float tileScale) : base(new Vector2(x, y), classId)
        {
            Width = tileSize * tileScale * map.GetLength(0);
            Height = tileSize * tileScale * map.GetLength(1);

            //TODO: Make this a public static method to set
            tilemap ??= Globals.content.Load<Texture2D>("tilemap_packed");
            Rectangle tileRectSpecs = new Rectangle(16, 16, 12, 11);
            tileDrawer = new TileDrawer(this, tilemap, tileRectSpecs, map, tileSize, tileScale);
            AddTrait(tileDrawer);

            rb = new Rigidbody(this, true);

            this.rects = new List<Rectangle>();

            int[,] map2 = map.Clone<int>();
            List<Rectangle> rects = map2.ToRectangles();
            for (int i = 0; i < rects.Count; i++)
            {
                rects[i] = new Rectangle(
                    (int)(rects[i].X * tileSize * tileScale + x), 
                    (int)(rects[i].Y * tileSize * tileScale + y), 
                    (int)(rects[i].Width * tileSize * tileScale), 
                    (int)(rects[i].Height * tileSize * tileScale));
                HitRect hitRect = new HitRect(this, rects[i]);
                rb.hitboxes.Add(hitRect);
                this.rects.Add(rects[i]);
            }

            rb.camera = Globals.Camera;
            AddTrait(rb);
        }

        public override void Draw()
        {
            base.Draw();
            //rb.DrawHitboxBorders();
        }
    }
}
