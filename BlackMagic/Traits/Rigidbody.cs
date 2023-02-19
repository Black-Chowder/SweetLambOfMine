using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BlackMagic
{
    //Trait to handle collision of hitboxes
    public class Rigidbody : Trait
    {
        public List<Type> ignoreTypes = new List<Type>();

        //Stores hitboxes
        public List<Hitbox> hitboxes = new List<Hitbox>();

        //isOverride means the entity cannot be pushed by other entities
        public bool isOverride = false;

        //isTotal means all rigidbody entities will collide with this entity
        public bool isTotal = false;  //<<== May Remove.  Not implemented Yet

        //The number of rays the rigibody samples from 
        public int raysPerSide = 4;

        //Thickness from how far inside the hitbox the collision detection rays will be cast
        public float skinWidth = 5;

        // <Temporary Testing Variables>
        public Camera camera;
        // </Temporary Testing Variables>

        public Color BorderColor { get; set; } = Color.Red;

        //Constructor(s)
        const string traitName = "rigidbody";
        public Rigidbody(Entity parent, bool isOverride = false) : base(traitName, parent)
        {
            base.priority = byte.MaxValue;
            this.isOverride = isOverride;
        }

        public Rigidbody(Entity parent, List<Hitbox> hitboxes, bool isOverride = false) : base(traitName, parent)
        {
            base.priority = byte.MaxValue;
            this.isOverride = isOverride;
            this.hitboxes = hitboxes;
        }

        public Rigidbody(Entity parent, Hitbox hitbox, bool isOverride = false) : base(traitName, parent)
        {
            base.priority = byte.MaxValue;
            this.isOverride = isOverride;
            hitboxes = new List<Hitbox>();
            hitboxes.Add(hitbox);
        }



        //Gets the distance from the edge of the shape and a point given to it.
        public float GetDist(Vector2 pos)
        {
            float shortestDist = float.PositiveInfinity;
            foreach (Hitbox hitbox in hitboxes)
            {
                float dist = hitbox.GetDist(pos);
                if (dist < shortestDist)
                    shortestDist = dist;
            }
            return shortestDist;
        }

        //Intended to be used for debugging purposes exclusively
        public void DrawHitboxBorders()
        {
            int hitboxIndex = -1;
            foreach (Hitbox hitbox in hitboxes)
            {
                hitboxIndex++;
                switch(hitbox)
                {
                    case HitRect:
                        HitRect hitRect = (HitRect)hitbox;
                        Rectangle drawRect = new Rectangle(
                            (int)(hitRect.Rect.X + parent.X - camera.X),
                            (int)(hitRect.Rect.Y + parent.Y - camera.Y), 
                            hitRect.Rect.Width, 
                            hitRect.Rect.Height);
                        DrawUtils.DrawRectBorder(Globals.spriteBatch, drawRect, BorderColor);
                        Globals.spriteBatch.DrawString(Globals.defaultFont, $"{hitboxIndex}", drawRect.Location.ToVector2(), BorderColor);
                        break;

                    case HitCirc:
                        HitCirc hitCirc = (HitCirc)hitbox;
                        //TODO
                        break;

                    case HitPoly:
                        HitPoly hitPoly = (HitPoly)hitbox;

                        //Draw a line between every point in the hitpoly
                        for (int i = 0; i < hitPoly.Points.Length - 1; i++)
                        {
                            Vector2 curPoint = hitPoly.Points[i];
                            Vector2 nextPoint = hitPoly.Points[i + 1];

                            DrawUtils.DrawLine(Globals.spriteBatch, 
                                curPoint + new Vector2(parent.X, parent.Y) - new Vector2(camera.X, camera.Y), 
                                nextPoint + new Vector2(parent.X, parent.Y) - new Vector2(camera.X, camera.Y),
                                BorderColor);
                        }

                        //Connect start point to end point
                        Vector2 firstPoint = hitPoly.Points[0];
                        Vector2 lastPoint = hitPoly.Points[hitPoly.Points.Length - 1];

                        DrawUtils.DrawLine(Globals.spriteBatch, 
                            firstPoint + new Vector2(parent.X, parent.Y) - new Vector2(camera.X, camera.Y), 
                            lastPoint + new Vector2(parent.X, parent.Y) - new Vector2(camera.X, camera.Y), 
                            BorderColor);

                        break;
                }
            }
        }

        //TODO: Don't create new rays every update.  Use rays from pool of already created rays.  Should improve efficiency
        public override void Update(GameTime gameTime)
        {
            return;
            if (isOverride) return;

            Ray ray;
            Vector2? rayData;

            bool hasGravity = parent.HasTrait<Gravity>();
            Gravity gravity = parent.GetTrait<Gravity>();
            if (hasGravity) gravity.grounded = false;

            //TODO: if !isOverride, then change self variables and others to properly apply forces

            foreach (Hitbox raw in hitboxes)
            {
                if (!(raw is HitRect)) continue;
                HitRect hitbox = (HitRect)raw;

                hitbox.resetCollisionData();
                for (int i = 0; i < raysPerSide; i++)
                {
                    //Calculate Ray Casting Points
                    //TODO: Get rid of division to speed up calculation
                    float raycastY = hitbox.AbsoluteY + skinWidth + (hitbox.Height - skinWidth * 2) * i / (raysPerSide - 1);
                    float raycastX = hitbox.AbsoluteX + skinWidth + (hitbox.Width - skinWidth * 2) * i / (raysPerSide - 1);

                    //Local Variable Used For Storing Entity Currently Colliding With:
                    Entity entity;
                    Rigidbody entityRigidbody;

                    //TODO: Turn following repeats of code into separate functions!!

                    //Top
                    ray = new Ray(raycastX, hitbox.AbsoluteY + skinWidth, (float)(Math.PI * 3 / 2));
                    rayData = ray.cast(parent);
                    if (rayData.HasValue && rayData.Value.Y > hitbox.AbsoluteY + parent.dy && parent.dy < 0)
                    {
                        entity = ray.getEntity();
                        entityRigidbody = (Rigidbody)entity.GetTrait(traitName);
                        if (!entityRigidbody.isOverride) entity.dy = parent.dy;

                        hitbox.Top = entity;

                        parent.dy = 0;
                        parent.Y = rayData.Value.Y - hitbox.Y;
                    }

                    //Bottom
                    ray = new Ray(raycastX, hitbox.AbsoluteY + hitbox.Height - skinWidth, (float)(Math.PI / 2));
                    rayData = ray.cast(parent);
                    if (rayData.HasValue && rayData.Value.Y < hitbox.AbsoluteY + hitbox.Height + parent.dy && parent.dy > 0)
                    {
                        entity = ray.getEntity();
                        entityRigidbody = (Rigidbody)entity.GetTrait(traitName);
                        if (!entityRigidbody.isOverride) entity.dy = parent.dy;

                        hitbox.Bottom = entity;
                        if (hasGravity) gravity.grounded = true;

                        parent.dy = 0;
                        parent.Y = rayData.Value.Y - hitbox.Height - hitbox.Y;
                    }

                    //Right
                    ray = new Ray(hitbox.AbsoluteX + hitbox.Width - skinWidth, raycastY, 0);
                    rayData = ray.cast(parent);
                    if (rayData.HasValue && rayData.Value.X < hitbox.AbsoluteX + hitbox.Width + parent.dx && parent.dx > 0)
                    {
                        entity = ray.getEntity();
                        entityRigidbody = (Rigidbody)entity.GetTrait(traitName);
                        if (!entityRigidbody.isOverride) entity.dx = parent.dx;

                        hitbox.Left = entity;

                        parent.dx = 0;
                        parent.X = rayData.Value.X - hitbox.Width - hitbox.X;
                    }

                    //Left
                    ray = new Ray(hitbox.AbsoluteX + skinWidth, raycastY, (float)(Math.PI));
                    rayData = ray.cast(parent);
                    if (rayData.HasValue && rayData.Value.X > hitbox.AbsoluteX + parent.dx && parent.dx < 0)
                    {
                        entity = ray.getEntity();
                        entityRigidbody = (Rigidbody)entity.GetTrait(traitName);
                        if (!entityRigidbody.isOverride) entity.dx = parent.dx;

                        hitbox.Right = entity;

                        parent.dx = 0;
                        parent.X = rayData.Value.X - hitbox.X;
                    }
                }
            }
        }
    }
}
