using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace BlackMagic
{
    public class Flamethrower : Trait, TDraws, IWeapon
    {
        const float range = 150f;

        const float spread = MathF.PI / 6;
        const int nRays = 3;

        bool isShooting = false;
        bool canShoot = true;

        public const float maxAmmo = 100;
        public float ammo = maxAmmo;

        float aimAngle = 0;
        float scale;

        List<Ray> rays;

        MouseState mouse;

        Texture2D texture;

        public Texture2D WeaponIconTexture => throw new NotImplementedException();

        public Flamethrower(Entity parent, byte priority = 100) : base(parent, priority)
        {
            rays = new List<Ray>();
            for (int i = 0; i < nRays; i++)
            {
                rays.Add(new Ray(0, 0, 0));
            }

            texture ??= Globals.content.Load<Texture2D>("sheep_fire");

            float size = 1448;
            scale = 150 / size;
        }

        public override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();


            isShooting = mouse.LeftButton == ButtonState.Pressed && ammo > 0;
            //Debug.WriteLine("ammo = " + ammo);
            if (ammo <= 0)
                canShoot = false;
            if (isShooting && canShoot)
            {
                ammo--;
                shootLogic();
            }
        }


        private void shootLogic()
        {
            aimAngle = MathF.PI + MathF.Atan2(parent.Y - Globals.Camera.Y - mouse.Y, parent.X - Globals.Camera.X - mouse.X);

            for (int i = 0; i < rays.Count; i++)
            {

                float angle = aimAngle + i * spread / nRays - (nRays / 2) * spread / nRays;
                rays[i] = new Ray(parent.X + parent.Width / 2, parent.Y, angle);

                Vector2? hitPoint = rays[i].cast(parent, range);
                Entity hitEntity = rays[i].getEntity();
                if (hitEntity == null || !hitPoint.HasValue || hitEntity is not BasicDemon)
                    continue;
                
                BasicDemon demon = (BasicDemon)hitEntity;
                demon.health.health -= 3;
            }
        }

        public void Draw()
        {
            if (isShooting && canShoot)
            {
                float scaleMultiplier = 2f;
                //Vector2 offset = new Vector2(Width, 0);
                Vector2 screenPos = parent.Pos + new Vector2(parent.Width / 2, 0) - Globals.Camera.Pos;// - offset;

                Globals.spriteBatch.Draw(texture, //Texture
                    screenPos, //Position
                new Rectangle(0, 0, 5000, 5000), //Source Rectangle
                    Color.White, // Color Tint
                    aimAngle - MathF.PI / 3.5f, //Rotation Angle
                    new Vector2(0, .5f), //Origin Of Sprite (where to rotate around)
                    scale * scaleMultiplier, //Scale
                    SpriteEffects.None, //Sprite Effects
                    0f); //Layer
            }
        }

        public void PassiveUpdate(GameTime gt)
        {
            if (!isShooting)
            {
                ammo += .5f;
                if (ammo > maxAmmo)
                {
                    canShoot = true;
                    ammo = maxAmmo;
                }
            }
        }
    }
}
