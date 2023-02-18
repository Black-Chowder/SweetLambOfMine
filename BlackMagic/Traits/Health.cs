using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class Health : Trait
    {
        public int health = 100;

        const string name = "Health";
        public Health(Entity parent) : base(name, parent)
        {
        }

        public override void Update(GameTime gameTime)
        {
            if (health <= 0)
                parent.exists = false;
        }
    }
}
