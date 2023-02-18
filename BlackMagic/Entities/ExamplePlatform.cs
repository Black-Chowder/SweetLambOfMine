using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class ExamplePlatform : Entity
    {
        public const string classId = "ExamplePlatform";
        public ExamplePlatform(float x, float y, float width, float height) : base(new Vector2(x, y), classId)
        {
            this.Width = width;
            this.Height = height;

            Rigidbody rb = new Rigidbody(this, true);
            HitRect hitRect = new HitRect(this);
            rb.hitboxes.Add(hitRect);
            AddTrait(rb);
        }

        public override void Draw()
        {
            DrawUtils.fillRect(Globals.spriteBatch, (int)(X - Globals.Camera.X), (int)(Y - Globals.Camera.Y), (int)Width, (int)Height, Color.Black);
        }
    }
}
