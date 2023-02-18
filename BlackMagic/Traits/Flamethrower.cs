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

        const float spread = MathF.PI / 6;
        const int nRays = 3;

        bool isShooting = false;

        Ray ray;

        List<Ray> rays;

        MouseState mouse;

        public Flamethrower(Entity parent, byte priority = 100) : base(parent, priority)
        {
            rays = new List<Ray>();
            for (int i = 0; i < nRays; i++)
            {
                rays.Add(new Ray(0, 0, 0));
            }
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
            float aimAngle = MathF.PI + MathF.Atan2(parent.Y - Globals.Camera.Y - mouse.Y, parent.X - Globals.Camera.X - mouse.X);

            for (int i = 0; i < rays.Count; i++)
            {

                float angle = aimAngle + i * spread / nRays - (nRays / 2) * spread / nRays;
                rays[i] = new Ray(parent.X, parent.Y, angle);

                Vector2? hitPoint = rays[i].cast(parent, range);
                Entity hitEntity = rays[i].getEntity();
                if (hitEntity == null || !hitPoint.HasValue || hitEntity is not BasicDemon)
                    continue;
                

            }
        }

        public void Draw()
        {
            if (isShooting)
            {
                for (int i = 0; i < rays.Count; i++)
                    rays[i].drawRay(Globals.spriteBatch);
            }
        }
    }
}
