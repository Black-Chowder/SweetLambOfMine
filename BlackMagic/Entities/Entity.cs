using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class EntityBatch
    {
        public List<Entity> entities { get; private set; }

        public Lamb player;

        public EntityBatch()
        {
            entities = new List<Entity>();

            Globals.MainEntityBatch = this;

            //Add(new AnimatedEntity(200, 200));
            player = new Lamb(new Vector2(100, 100));
            Add(player);
        }

        public EntityBatch(byte[] byteMap)
        {
            entities = new List<Entity>();

            Globals.MainEntityBatch = this;

            MapLoader mapLoader = new MapLoader(this);
            mapLoader.Load(byteMap);

            //Add(new ExamplePlayer());
            //Add(new ExamplePlatform(100, 300, 500, 100));
            //Add(new ExampleBlock(200, 200));
        }

        public void Add(Entity e)
        {
            entities.Add(e);
            e.batch = this;
        }

        public void Remove(Entity e)
        {
            entities.Remove(e);
            e.batch = null;
        }

        public void Update(GameTime gameTime)
        {
            Globals.Camera.Update(gameTime);
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
                if (!entities[i].exists)
                    entities.RemoveAt(i);
            }
        }

        public void Draw()
        {
            Globals.spriteBatch.Begin(SpriteSortMode.FrontToBack, samplerState: SamplerState.PointClamp);
            for (int i = 0; i < entities.Count; i++)
                entities[i].Draw();
            Globals.spriteBatch.End();
        }
    }

    public abstract class Entity
    {
        private List<Trait> traits;
        private Dictionary<string, Trait> traitSet;
        private List<TDraws> tDraws;
        
        public string ClassId { get; protected set; }

        public Vector2 Pos { get; set; }
        public float X
        {
            get => Pos.X;
            set => Pos = new Vector2(value, Pos.Y);
        }
        public float Y
        {
            get => Pos.Y;
            set => Pos = new Vector2(Pos.X, value);
        }


        public Vector2 DeltaPos { get; set; } = Vector2.Zero;
        public float dx
        {
            get => DeltaPos.X;
            set => DeltaPos = new Vector2(value, DeltaPos.Y);
        }
        public float dy
        {
            get => DeltaPos.Y;
            set => DeltaPos = new Vector2(DeltaPos.X, value);
        }

        public float Width;
        public float Height;

        public bool isVisible = true;

        public bool exists { get; set; } = true;

        public EntityBatch batch { get; set; }

        public Entity(Vector2 pos, string classId = "unnamed")
        {
            traits = new List<Trait>();
            traitSet = new Dictionary<string, Trait>();
            tDraws = new List<TDraws>();
            ClassId = classId;
            Pos = pos;
        }

        public void AddTrait<T>(T t) where T : Trait
        {
            if (HasTrait<T>())
                return;
            traitSet.Add(typeof(T).FullName, t);
            traits.Add(t);
            traits.Sort((a, b) => { return a.priority.CompareTo(b.priority); });
            if (t is TDraws td)
                tDraws.Add(td);
        }

        public void RemoveTrait<T>(T t) where T : Trait
        {
            traits.Remove(t);
            traitSet.Remove(typeof(T).FullName);
            if (t is TDraws td)
                tDraws.Remove(td);
        }

        public T GetTrait<T>() where T : Trait
        {
            traitSet.TryGetValue(typeof(T).FullName, out Trait toReturn);
            return (T)toReturn;
        }

        public List<Trait> GetTraits(params Type[] types)
        {
            List<Trait> toReturn = new List<Trait>();
            foreach (Type T in types)
            {
                Trait toAdd;
                if (traitSet.TryGetValue(T.FullName, out toAdd))
                    toReturn.Add(toAdd);
            }
            return toReturn;
        }

        public bool HasTrait<T>() where T : Trait
        {
            return traitSet.ContainsKey(typeof(T).FullName);
        }

        //Legacy get type function
        public Trait GetTrait(string name)
        {
            for (int i = 0; i < traits.Count; i++)
                if (traits[i].name == name)
                    return traits[i];
            return null;
        }

        //Legacy hasTrait function
        public bool HasTrait(string name)
        {
            for (int i = 0; i < traits.Count; i++)
                if (traits[i].name == name)
                    return true;
            return false;
        }

        public virtual void Update(GameTime gameTime)
        {
            TraitUpdate(gameTime);
        }

        protected void TraitUpdate(GameTime gameTime)
        {
            for (int i = 0; i < traits.Count; i++)
                if (traits[i].isActive) traits[i].Update(gameTime);

            if (MathF.Abs(dx) < .01f) dx = 0;
            if (MathF.Abs(dy) < .01f) dy = 0;
            Pos += DeltaPos;
        }

        public virtual void Draw() 
        {
            if (!isVisible) return;
            for (int i = 0; i < tDraws.Count; i++)
                tDraws[i].Draw();
        }
    }
}
