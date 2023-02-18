using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public void Draw()
        {
            Weapons[WeaponIndex].Draw();
        }
    }
}
