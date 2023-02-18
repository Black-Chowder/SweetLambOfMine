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

        public List<IWeapon> Weapons = new List<IWeapon>();
        public int WeaponIndex = 0;

        const string name = "WeaponManager";
        public WeaponManager(Entity parent) : base(name, parent)
        {
            flamethrower = new Flamethrower(parent);
            Weapons.Add(flamethrower);
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
