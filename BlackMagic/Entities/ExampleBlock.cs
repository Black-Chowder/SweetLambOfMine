using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class ExampleBlock : Entity
    {
        public const string classId = "ExampleBlock";
        public ExampleBlock(float x, float y) : base(new Vector2(x, y), classId)
        {
            Width = 32;
            Height = 32;

            Rigidbody rb = new Rigidbody(this);
            HitRect hitbox = new HitRect(this);
            rb.hitboxes.Add(hitbox);
            AddTrait(rb);

            TDFriction friction = new TDFriction(this);
            AddTrait(friction);
        }

        public override void Draw()
        {
            DrawUtils.fillRect(Globals.spriteBatch, (int)(X - Globals.Camera.X), (int)(Y - Globals.Camera.Y), (int)Width, (int)Height, Color.White);
        }
    }
}
