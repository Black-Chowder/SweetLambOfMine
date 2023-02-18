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
    internal class Soda : Trait, IWeapon
    {
        MouseState mouse;
        public bool canShoot = true;

        const float cooldownSet = 200;
        float cooldown = 0;

        public Texture2D WeaponIconTexture => throw new NotImplementedException();

        public Soda(Entity parent) : base(parent)
        {

        }


        public override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();


            if (canShoot && ClickHandler.leftClicked)
            {
                parent.batch.Add(new SodaProjectile(parent.Pos, mouse.Position.ToVector2() + Globals.Camera.Pos));
                cooldown = cooldownSet;
            }
        }

        public void Draw()
        {

        }

        public void PassiveUpdate(GameTime gt)
        {
            cooldown--;
            if (cooldown <= 0)
                canShoot = true;
            else
                canShoot = false;
        }

    }
}
