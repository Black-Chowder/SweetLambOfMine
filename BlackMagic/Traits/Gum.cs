using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    internal class Gum : Trait, IWeapon
    {
        public Entity[] attached = new Entity[2];

        LinkedList<Entity[]> pairings = new LinkedList<Entity[]>();

        Projectile projectile;
        class Projectile : Entity
        {
            Entity parentEntity;
            Gum parentTrait;
            const float speed = 2;

            const string classId = "GumProjectile";
            public Projectile(Entity parentEntity, Gum parentTrait, Vector2 pos, float angle) : base(pos, classId)
            {
                this.parentEntity = parentEntity;
                this.parentTrait = parentTrait;
                Width = Height = 15;

                dx = MathF.Cos(angle) * speed;
                dy = MathF.Sin(angle) * speed;
            }

            public override void Update(GameTime gameTime)
            {
                foreach (Entity entity in parentEntity.batch.entities)
                {
                    if (entity == parentEntity)
                        continue;

                    Rectangle selfRect = new Rectangle(Pos.ToPoint(), new Point((int)Width, (int)Height));
                    Rectangle enemyRect = new Rectangle(entity.Pos.ToPoint(), new Point((int)entity.Width, (int)entity.Height));

                    if (selfRect.Intersects(enemyRect))
                    {
                        if (parentTrait.attached[0] == null)
                        {
                            parentTrait.attached[0] = entity;
                        }
                        else if (parentTrait.attached[0] != entity)
                        {
                            parentTrait.attached[1] = entity;
                        }
                    }
                }

                base.Update(gameTime);
            }

            public override void Draw()
            {
                DrawUtils.fillRect(Globals.spriteBatch, (int)(X), (int)(Y), (int)(Width), (int)(Height), Color.Pink);

                base.Draw();
            }
        }


        MouseState mouse;

        public Texture2D WeaponIconTexture => throw new NotImplementedException();

        const string name = "Gum";
        public Gum(Entity parent) : base(name, parent)
        {
            for (int i = 0; i < attached.Length; i++)
                attached[i] = null;

            pairings = new LinkedList<Entity[]>();
        }


        public override void Update(GameTime gameTime)
        {
            mouse = Mouse.GetState();

            if (projectile != null)
                projectile.Update(gameTime);

            if (attached[0] != null && attached[1] != null)
            {
                pairings.AddLast(attached);
                attached = new Entity[2];
            }

            for (LinkedListNode<Entity[]> node = pairings.First; node != null; node = node.Next)
            {
                Entity e1 = node.Value[0];
                Entity e2 = node.Value[1];

                float dist = DistanceUtils.getDistance(e1.Pos, e2.Pos);
                float angle = MathF.Atan2(e1.Y - e2.Y, e1.X - e2.X);

                //WARNING: This code REALLY doesn't seem like it should work but it does for some reason and I don't want to mess with it anymore
                e1.DeltaPos = new Vector2(-MathF.Cos(-angle), MathF.Sin(-angle));
                e2.DeltaPos = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            }

            if (ClickHandler.leftClicked)
            {
                float angle = MathF.PI + MathF.Atan2(parent.Y - Globals.Camera.Y - mouse.Y, parent.X - Globals.Camera.X - mouse.X);
                projectile = new Projectile(parent, this, parent.Pos, angle);
            }
        }

        public void Draw()
        {
            if (projectile != null) projectile.Draw();

            //TODO: Draw connections between enemeies
        }

        public void PassiveUpdate(GameTime gt)
        {
            throw new NotImplementedException();
        }
    }
}
