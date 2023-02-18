using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace BlackMagic
{
    public class SodaProjectile : Entity
    {
        bool isTraveling;
        float speed = 7f;

        float aoe = 300;

        Vector2 target;
        static Texture2D texture;
        static Texture2D circText;
        float scale;

        float angle = 0;

        float timer = 300;
        public SodaProjectile(Vector2 pos, Vector2 target) : base(pos)
        {
            isTraveling = true;

            float size = 1448;
            scale = 150 / size;

            this.target = target;

            texture ??= Globals.content.Load<Texture2D>("sheep_soda");

            circText ??= DrawUtils.createCircleTexture(Globals.spriteBatch.GraphicsDevice, 300);

            float angle = MathF.Atan2(target.Y - pos.Y, target.X - pos.X);
            DeltaPos = new Vector2(MathF.Cos(angle) * speed, MathF.Sin(angle) * speed);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (isTraveling)
            {
                if (DistanceUtils.getDistance(Pos, target) < 10)
                {
                    isTraveling = false;
                    DeltaPos = Vector2.Zero;
                }

                angle++;

                return;
            }

            timer--;
            if (timer < 0)
                exists = false;

            foreach (Entity entity in batch.entities)
            {
                if (entity is not BasicDemon)
                    continue;

                float dist = DistanceUtils.getDistance(Pos, entity.Pos);
                if (dist < aoe)
                {
                    ((BasicDemon)entity).health.health--;
                }
            }
        }

        public override void Draw()
        {
            if (isTraveling)
            {
                float scaleMultiplier = 2f;
                //Vector2 offset = new Vector2(Width, 0);
                Vector2 screenPos = Pos - Vector2.Zero * 100 - Globals.Camera.Pos;// - offset;

                Globals.spriteBatch.Draw(texture, //Texture
                    screenPos, //Position
                new Rectangle(0, 0, 5000, 5000), //Source Rectangle
                    Color.White, // Color Tint
                angle / 16f, //Rotation Angle
                    new Vector2(0, .5f), //Origin Of Sprite (where to rotate around)
                    .05f, //Scale
                    SpriteEffects.None, //Sprite Effects
                    0f); //Layer
            }
            else
            {
                //Vector2 offset = new Vector2(Width, 0);
                Vector2 screenPos = Pos - (Vector2.One * aoe / 2f) - Globals.Camera.Pos;// - offset;

                Globals.spriteBatch.Draw(circText, //Texture
                    screenPos, //Position
                new Rectangle(0, 0, (int)aoe, (int)aoe), //Source Rectangle
                    Color.DarkRed, // Color Tint
                0, //Rotation Angle
                    new Vector2(0, .5f), //Origin Of Sprite (where to rotate around)
                    1, //Scale
                    SpriteEffects.None, //Sprite Effects
                    0f); //Layer
            }

            base.Draw();
        }
    }
}
