using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace BlackMagic
{
    internal class WeaponManager : Trait, TDraws
    {
        public Flamethrower flamethrower;
        public Gum gum;

        public List<IWeapon> Weapons = new List<IWeapon>();
        public int WeaponIndex = 1;

        const string name = "WeaponManager";
        public WeaponManager(Entity parent) : base(name, parent)
        {
            flamethrower = new Flamethrower(parent);
            Weapons.Add(flamethrower);

            gum = new Gum(parent);
            Weapons.Add(gum);
        }

        public override void Update(GameTime gameTime)
        {
            Weapons[WeaponIndex].Update(gameTime);

            if (ClickHandler.IsClicked(Keys.D1))
            {
                WeaponIndex = 0;
            }
            else if (ClickHandler.IsClicked(Keys.D2))
            {
                WeaponIndex = 1;
            }
        }

        public void Draw()
        {
            Weapons[WeaponIndex].Draw();
        }
    }
}
