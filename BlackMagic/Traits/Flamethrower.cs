using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class Flamethrower : Trait, TDraws
    {
        const float range = 150f;

        bool isShooting = false;

        Ray ray;

        MouseState mouse;

        public Flamethrower(Entity parent, byte priority = 100) : base(parent, priority)
        {

        }

        public override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();


            isShooting = mouse.LeftButton == ButtonState.Pressed;

            if (isShooting)
            {
                shootLogic();
            }
        }


        private void shootLogic()
        {
            float angle = MathF.PI + MathF.Atan2(parent.Y - Globals.Camera.Y - mouse.Y, parent.X - Globals.Camera.X - mouse.X);

            ray = new Ray(parent.X, parent.Y, angle);
            Vector2? hitPoint = ray.cast(parent, range);
            Entity hitEntity = ray.getEntity();
            if (hitEntity == null || !hitPoint.HasValue)
                return;
            
        }

        public void Draw()
        {
            if (isShooting)
            {
                ray.drawRay(Globals.spriteBatch);
            }
        }
    }
}
