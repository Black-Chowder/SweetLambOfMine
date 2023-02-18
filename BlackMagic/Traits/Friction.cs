﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace BlackMagic
{
    public class Friction : Trait
    {
        public float coefficient;
        public float airCoefficient;
        public float frictionForce = 0;

        //REMINDER TO PROGRAM LATER: 
        //Friction force, air friction and whatnot need to be treated differently if game is top-down!

        private const String traitName = "friction";
        public Friction(Entity parent, float coefficient) : base(traitName, parent)
        {
            this.coefficient = coefficient;
            airCoefficient = 0;
        }

        public Friction(Entity parent, float coefficient, float airCoefficient) : base(traitName, parent)
        {
            this.coefficient = coefficient;
            this.airCoefficient = airCoefficient;
        }


        public override void Update(GameTime gameTime)
        {
            //This doesn't properly apply friction while in the air to dy.
            if (parent.HasTrait("gravity") && ((Gravity)parent.GetTrait("gravity")).grounded && coefficient != 0) 
                frictionForce = parent.dx - (parent.dx) / coefficient;

            else if (airCoefficient != 0) 
                frictionForce = parent.dx - (parent.dx) / airCoefficient;

            else 
                frictionForce = 0;

            parent.dx -= frictionForce;
        }
    }
}
