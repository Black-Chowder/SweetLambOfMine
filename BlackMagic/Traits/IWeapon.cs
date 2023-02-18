using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    internal interface IWeapon
    {
        public Texture2D WeaponIconTexture { get; }

        public void PassiveUpdate(GameTime gt);

        public void Update(GameTime gt);

        public void Draw();
    }
}
