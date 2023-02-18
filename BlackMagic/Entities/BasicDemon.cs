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
            movement = new TDMovement(this);
            AddTrait(movement);
        }

        public override void Draw()
        {
            base.Draw();

            rb.DrawHitboxBorders();
        }
    }
}
