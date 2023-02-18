using Microsoft.Xna.Framework;
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

        const string classId = "BasicDemon";
        public BasicDemon(Vector2 pos) : base(pos, classId)
        {
            Width = Height = 50;

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
        }

        public override void Draw()
        {
            base.Draw(); 
            DrawUtils.fillRect(Globals.spriteBatch, (int)(X), (int)(Y), (int)(Width), (int)(Height), Color.Crimson);
            rb.DrawHitboxBorders();
        }
    }
}
