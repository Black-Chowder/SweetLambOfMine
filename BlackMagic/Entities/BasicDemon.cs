using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    internal class BasicDemon : Entity
    {
        Rigidbody rb;
        TDMovement movement;
        TDFriction friction;

        BasicDemonAI ai;

        public Health health;

        public int stuckNum = 0;

        Texture2D texture1;
        Texture2D texture2;

        float scale;

        const string classId = "BasicDemon";
        public BasicDemon(Vector2 pos) : base(pos, classId)
        {
            Width = 50;
            Height = 100;

            float size = 1448;
            scale = Width / size;

            texture1 = Globals.content.Load<Texture2D>("demon_L.2");
            texture2 = Globals.content.Load<Texture2D>("demon_R2");


            //Set up traits

            //Rigidbody
            HitRect hitbox = new HitRect(this);
            rb = new Rigidbody(this);
            rb.hitboxes.Add(hitbox);
            rb.camera = Globals.Camera;
            AddTrait(rb);

            //Movement
            movement = new TDMovement(this, 3);
            AddTrait(movement);

            //Friction
            friction = new TDFriction(this);
            AddTrait(friction);

            //AI
            ai = new BasicDemonAI(this, movement);
            AddTrait(ai);

            //Health
            health = new Health(this);
            AddTrait(health);
        }

        public override void Update(GameTime gameTime)
        {
            friction.coefficient = 2f + .5f * stuckNum;
            base.Update(gameTime);
        }

        public override void Draw()
        {
            base.Draw();

            float scaleMultiplier = 3f;
            Vector2 offset = new Vector2(Width, 0);
            Vector2 screenPos = Pos - Globals.Camera.Pos - offset;

            Globals.spriteBatch.Draw(texture1, //Texture
                screenPos, //Position
            new Rectangle(0, 0, 5000, 5000), //Source Rectangle
                Color.White, // Color Tint
                0, //Rotation Angle
                new Vector2(.5f, 1), //Origin Of Sprite (where to rotate around)
                scale * scaleMultiplier, //Scale
                SpriteEffects.None, //Sprite Effects
                .5f); //Layer


            //DrawUtils.fillRect(Globals.spriteBatch, (int)(X - Globals.Camera.X), (int)(Y - Globals.Camera.Y), (int)(Width), (int)(Height), Color.Crimson);
            //rb.DrawHitboxBorders();
        }
    }
}
