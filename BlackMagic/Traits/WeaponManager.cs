using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;

namespace BlackMagic
{
    internal class WeaponManager : Trait, TDraws
    {
        public Flamethrower flamethrower;
        public Gum gum;
        public Soda soda;

        public List<IWeapon> Weapons = new List<IWeapon>();
        public int WeaponIndex = 1;

        float scale;
        float Width = 100;
        float Height = 100;

        Texture2D flamethrowerIcon;
        Texture2D gumIcon;

        const string name = "WeaponManager";
        public WeaponManager(Entity parent) : base(name, parent)
        {
            flamethrower = new Flamethrower(parent);
            Weapons.Add(flamethrower);

            gum = new Gum(parent);
            Weapons.Add(gum);

            soda = new Soda(parent);
            Weapons.Add(soda);

            float size = 1448;
            scale = Width / size;

            flamethrowerIcon = Globals.content.Load<Texture2D>("sheep_flame");
            gumIcon = Globals.content.Load<Texture2D>("sheep_gum");
        }

        public override void Update(GameTime gameTime)
        {
            Weapons[WeaponIndex].Update(gameTime);

            foreach (IWeapon weapon in Weapons)
            {
                weapon.PassiveUpdate(gameTime);
            }

            if (ClickHandler.IsClicked(Keys.D1))
            {
                WeaponIndex = 0;
            }
            else if (ClickHandler.IsClicked(Keys.D2))
            {
                WeaponIndex = 1;
            }
            else if (ClickHandler.IsClicked(Keys.D3))
            {
                WeaponIndex = 2;
            }
        }

        public void Draw()
        {
            foreach (IWeapon w in Weapons)
            {
                w.Draw();
            }

            float scaleMultiplier = 1.5f;
            Vector2 offset = new Vector2(Width, Height * 2);
            Vector2 screenPos = new Vector2(50, 0);

            Globals.spriteBatch.Draw(flamethrowerIcon, //Texture
                screenPos, //Position
            new Rectangle(0, 0, 5000, 5000), //Source Rectangle
                Color.White * (flamethrower.ammo / Flamethrower.maxAmmo), // Color Tint
                0, //Rotation Angle
                new Vector2(.5f, 1), //Origin Of Sprite (where to rotate around)
                scale * scaleMultiplier, //Scale
                SpriteEffects.None, //Sprite Effects
                0f); //Layer


            scaleMultiplier = 1.5f;
            offset = new Vector2(Width, Height * 2);
            screenPos = new Vector2(50, 125);

            Globals.spriteBatch.Draw(gumIcon, //Texture
                screenPos, //Position
            new Rectangle(0, 0, 5000, 5000), //Source Rectangle
                Color.White * (WeaponIndex == 1 ? 1 : 0.5f), // Color Tint
                0, //Rotation Angle
                new Vector2(.5f, 1), //Origin Of Sprite (where to rotate around)
                scale * scaleMultiplier, //Scale
                SpriteEffects.None, //Sprite Effects
                0f); //Layer
        }
    }
}
