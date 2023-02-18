using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
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

        const string classId = "Lamb";
        public Lamb(Vector2 pos) : base(pos, classId)
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
            movement = new TDMovement(this, directControl: true);
            AddTrait(movement);

            //Friction
            friction = new TDFriction(this);
            AddTrait(friction);

            //Weapon Manager
            weaponManager = new WeaponManager(this);
            AddTrait(weaponManager);
        }

        public override void Draw()
        {
            base.Draw();
            DrawUtils.fillRect(Globals.spriteBatch, (int)(X), (int)(Y), (int)(Width), (int)(Height), Color.Black);
            rb.DrawHitboxBorders();

        }
    }
}
