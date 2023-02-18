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
        bool canShoot = true;

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
            }
        }

        public void Draw()
        {

        }

        public void PassiveUpdate(GameTime gt)
        {

        }

    }
}
