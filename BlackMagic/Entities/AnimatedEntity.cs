using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlackMagic.Properties;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BlackMagic
{
    public class AnimatedEntity : Entity
    {
        TAnimator tAnimator;
        Animator animator;

        Texture2D texture;
        public AnimatedEntity(float x = 0, float y = 0) : base(new Vector2(x, y))
        {
            AsepriteData aniJson = JsonSerializer.Deserialize<AsepriteData>(Resources.Astrid);

            texture = Globals.content.Load<Texture2D>("Astrid");

            animator = new Animator(texture, aniJson);
            animator.SetAnimation("attack");
            animator.scale = 3f;
            animator.isFacingRight = true;
            tAnimator = new TAnimator(this, animator);
            AddTrait(tAnimator);
        }
    }
}
