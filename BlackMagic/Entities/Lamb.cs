using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class Lamb : Entity
    {
        Rigidbody rb;
        TDMovement movement;
        TDFriction friction;

        WeaponManager weaponManager;

        public Health health;

        Texture2D texture;

        float scale;

        const string classId = "Lamb";
        public Lamb(Vector2 pos) : base(pos, classId)
        {
            Width = Height = 50;

            float size = 1448;
            scale = Width / size;

            texture = Globals.content.Load<Texture2D>("sheep2");

            //Set up traits

            //Rigidbody
            HitRect hitbox = new HitRect(this);
            rb = new Rigidbody(this);
            rb.hitboxes.Add(hitbox);
            rb.camera = Globals.Camera;
            AddTrait(rb);

            //Movement
            movement = new TDMovement(this, directControl: true);
            AddTrait(movement);

            //Friction
            friction = new TDFriction(this);
            AddTrait(friction);

            //Weapon Manager
            weaponManager = new WeaponManager(this);
            AddTrait(weaponManager);

            //Health
            health = new Health(this);
            AddTrait(health);
        }

        public override void Update(GameTime gameTime)
        {
            Globals.Camera.GoTo(X - Globals.Camera.Width / 2, Y - Globals.Camera.Height / 2);
            base.Update(gameTime);
        }

        public override void Draw()
        {
            float scaleMultiplier = 3f;
            Vector2 offset = new Vector2(Width, Height * 2);
            Vector2 screenPos = Pos - Globals.Camera.Pos - offset;

            Globals.spriteBatch.Draw(texture, //Texture
                screenPos, //Position
            new Rectangle(0, 0, 5000, 5000), //Source Rectangle
                Color.White, // Color Tint
                0, //Rotation Angle
                new Vector2(.5f, 1), //Origin Of Sprite (where to rotate around)
                scale * scaleMultiplier, //Scale
                SpriteEffects.None, //Sprite Effects
                0f); //Layer

            base.Draw();
            //DrawUtils.fillRect(Globals.spriteBatch, (int)(X - Globals.Camera.X), (int)(Y - Globals.Camera.Y), (int)(Width), (int)(Height), Color.Black);
            //rb.DrawHitboxBorders();

        }
    }
}
