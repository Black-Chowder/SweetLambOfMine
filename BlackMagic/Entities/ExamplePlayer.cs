using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BlackMagic
{
    public class ExamplePlayer : Entity
    {
        public ExamplePlayer(float x = 0, float y = 0) : base(new Vector2(x, y))
        {
            Width = Height = 32;
            Rigidbody rb = new Rigidbody(this);
            HitRect hitRect = new HitRect(this);
            rb.hitboxes.Add(hitRect);
            AddTrait(rb);

            TDMovement mm = new TDMovement(this, 5, true);
            AddTrait(mm);

            TDFriction friction = new TDFriction(this);
            AddTrait(friction);
        }

        public override void Update(GameTime gameTime)
        {
            Globals.Camera.GoTo(X - Globals.Camera.Width / 2, Y - Globals.Camera.Height / 2);

            base.Update(gameTime);
        }

        public override void Draw()
        {
            DrawUtils.fillRect(Globals.spriteBatch, (int)(X - Globals.Camera.X), (int)(Y - Globals.Camera.Y), (int)Width, (int)Height, Color.White);
        }
    }
}
