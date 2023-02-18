using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms.Design;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BlackMagic
{
    public class TAnimator : Trait, TDraws
    {
        public Animator animator { get; set; }

        const string classId = "Animator";
        public TAnimator(Entity parent, Animator animator) : base(classId, parent)
        {
            this.animator = animator;
        }

        public override void Update(GameTime gameTime)
        {
            animator.Update(gameTime);
        }

        public void Draw() { Draw(Globals.spriteBatch); }
        public void Draw(SpriteBatch spriteBatch, float? x = null, float? y = null)
        {
            if (!parent.isVisible) return;
            x ??= parent.X - Globals.Camera.X;
            y ??= parent.Y - Globals.Camera.Y;
            animator.Draw(spriteBatch, x.Value, y.Value);
        }
    }
}
