using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.Devices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlackMagic
{
    //Handles playing requested animations and storing animation sequences
    public class Animator
    {
        private const double twos = 1000 / 12d; //12 fps

        private const string loopPattern = @".+_loop$";
        private static Regex loopRegex = null;

        private class AnimationLayer
        {
            public int Count { get => srcRects.Length; }

            private Rectangle[] srcRects;
            private Vector2[] offsets;

            public AnimationLayer(List<Rectangle> srcRects, List<Vector2> offsets = null)
            {
                this.srcRects = srcRects.ToArray();

                if (offsets is null)
                {
                    this.offsets = new Vector2[Count];
                    for (int i = 0; i < Count; i++)
                        this.offsets[i] = Vector2.Zero;
                }
                else if (offsets.Count != srcRects.Count)
                    throw new Exception("Number of offsets not equal to number of source rectangles");

                else
                    this.offsets = offsets.ToArray();
            }
            
            public Rectangle this[int index] { get => srcRects[index]; }
            public Rectangle GetSrcRect(int index) => srcRects[index];

            public Vector2 GetOffset(int index) => offsets[index];

            //This one shouldn't really be used because it's slow compared to AddFrames, but I'm leaving it in anyway
            public void AddFrame(Rectangle srcRect, Vector2 offset)
            {
                List<Rectangle> toAddRects = new List<Rectangle>();
                List<Vector2> toAddOffsets = new List<Vector2>();
                toAddRects[0] = srcRect;
                toAddOffsets[0] = offset;
                AddFrames(toAddRects, toAddOffsets);
            }

            public void AddFrames(List<Rectangle> srcRects, List<Vector2> offsets = null)
            {
                if (offsets is null)
                {
                    offsets = new List<Vector2>();
                    for (int i = 0; i < srcRects.Count; i++)
                        offsets.Add(Vector2.Zero);
                }
                else if (offsets.Count != srcRects.Count)
                    throw new Exception("Number of offsets not equal to number of source rectangles");

                Rectangle[] newSrcRects = new Rectangle[this.srcRects.Length + srcRects.Count];
                Vector2[] newOffsets = new Vector2[this.offsets.Length + offsets.Count];

                this.srcRects.CopyTo(newSrcRects, 0);
                this.offsets.CopyTo(newOffsets, 0);

                srcRects.CopyTo(newSrcRects, this.srcRects.Length - 1);
                offsets.CopyTo(newOffsets, this.offsets.Length - 1);

                this.srcRects = newSrcRects;
                this.offsets = newOffsets;
            }
        }

        //Stores and handles rectangles that represent locations on the sprite sheet for each frame of animation
        private class Animation
        {
            public bool IsLooping { get; set; } = false;
            public double TotalDuration { get; private set; } = 0d;
            public Point SrcSize { get; private set; }
            public int Count { get => layers[0].Count; } //Returns number of frames in animation
            public int LayersCount { get => layers.Length; }
            
            private double[] durations; //milliseconds

            private AnimationLayer[] layers;

            public Animation(Point srcSize, List<Rectangle> srcRects, bool isLooping = false, List<double> durations = null, List<Vector2> animationOffsets = null)
            {
                this.SrcSize = srcSize;

                AnimationLayer layer = new AnimationLayer(srcRects, animationOffsets);
                layers = new AnimationLayer[1];
                layers[0] = layer;
                this.IsLooping = isLooping;

                setupDurations(durations);
            }

            public Animation(Point srcSize, List<AnimationLayer> layers, bool isLooping = false, List<double> durations = null)
            {
                this.SrcSize = srcSize;
                this.IsLooping = isLooping;
                this.layers = layers.ToArray();

                setupDurations(durations);
            }

            //Some checks and whatnot for setting up duration-related variables.  Used by constructors
            private void setupDurations(List<double> durations = null)
            {
                if (durations is null)
                {
                    this.durations = new double[Count];
                    for (int i = 0; i < Count; i++)
                        this.durations[i] = twos;
                    TotalDuration = twos * Count;
                }
                else if (durations.Count != Count)
                    throw new Exception("Number of durations not equal to number of source rectangles");

                else
                {
                    this.durations = durations.ToArray();
                    for (int i = 0; i < Count; i++)
                        TotalDuration += durations[i];
                }
                    
            }

            public AnimationLayer GetLayer(int index) => layers[index];
            public double GetDuration(int index) => durations[index];

            public int GetAnimationIndex(double duration)
            {
                //I'm going to loop through the durations for now, but it should probably be stored in a dictionary
                //and calculate the key based on the duration to get the index
                double durationCount = 0;
                for (int i = 0; i < Count; i++)
                {
                    durationCount += durations[i];
                    if (duration <= durationCount)
                        return i;
                }
                return Count - 1;
            }
        }
        private Animation animation;
        private Dictionary<string, Animation> animations;

        //Controls which sprite to display in animation
        private int animationIndex;

        //Used to keep track of time since animation started
        private TimeSpan curAnimationTimer;
        private bool isBeginningAnimation = false;

        //Tracks if animation is over
        private bool animationOver = false;
        public bool isAnimationOver() { return animationOver; }

        public bool isFacingRight {
            get => spriteEffects != SpriteEffects.FlipHorizontally;
            set => spriteEffects = value ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
        }

        public float scale { get; set; } = 1f;

        public float gameScale { private get; set; } = 1f;

        public SpriteEffects spriteEffects { get; set; } = SpriteEffects.None;

        public float angle { get; set; } = 0;

        public int width { get; private set; }
        public int height { get; private set; }
        public int columns { get; private set; }
        public int rows { get; private set; }

        public Vector2 origin { get; set; }

        public float layer { get; set; } = 0;

        //TODO: Allow for storage of multiple sprite sheets
        public Texture2D spriteSheet { get; set; }

        public Animator(Texture2D spriteSheet, AsepriteData data)
        {
            loopRegex ??= new Regex(loopPattern);

            this.spriteSheet = spriteSheet;
            this.width = data.meta.size.w;
            this.height = data.meta.size.h;
            curAnimationTimer = new TimeSpan(0);

            animations = new Dictionary<string, Animation>();
            animation = animations.FirstOrDefault().Value;
            isFacingRight = true;

            
            List<Rectangle> srcRects = new List<Rectangle>();
            List<Vector2> offsets = new List<Vector2>();
            List<double> durations = new List<double>();
            for (int i = 0; i < data.frames.Length; i++)
            {
                AseRect frameSrc = data.frames[i].frame;
                srcRects.Add(new Rectangle(frameSrc.x, frameSrc.y, frameSrc.w, frameSrc.h));

                AseRect offset = data.frames[i].spriteSourceSize;
                offsets.Add(new Vector2(offset.x, offset.y));

                double duration = data.frames[i].duration;
                durations.Add(duration);
            }

            //Add animations
            //TODO: allow for layer data that does not separate layers
            int layersCount = data.meta.layers.Length;
            for (int i = 0; i < data.meta.frameTags.Length; i++)
            {
                TagData tagData = data.meta.frameTags[i];

                int count = tagData.to - tagData.from;
                bool isLooping = loopRegex.IsMatch(tagData.name);

                //Get data for all layers for tag
                List<AnimationLayer> layers = new List<AnimationLayer>();
                for (int j = 0; j < layersCount; j++)
                {
                    int layerDataOffset = (int)(((float)j / layersCount) * srcRects.Count);
                    List<Rectangle> curAniRects = srcRects.GetRange(tagData.from + layerDataOffset, count);
                    List<Vector2> curOffsets = offsets.GetRange(tagData.from + layerDataOffset, count);

                    AnimationLayer layer = new AnimationLayer(curAniRects, curOffsets);
                    layers.Add(layer);
                }
                List<double> curDurations = durations.GetRange(tagData.from, count);

                AseSize rawSrcSize = data.frames[0].sourceSize;
                Point srcSize = new Point(rawSrcSize.w, rawSrcSize.h);

                AddAnimation(tagData.name, srcSize, layers, isLooping, curDurations);
            }
        }

        private void AddAnimation(string name, Point srcSize, List<AnimationLayer> layers, 
            bool isLooping = false,
            List<double> durations = null)
        {
            animations.Add(name, new Animation(srcSize, layers, isLooping, durations));
        }

        private void AddAnimation(string name, Point srcSize, List<Rectangle> srcRects, 
            bool isLooping = false, 
            List<double> durations = null, 
            List<Vector2> offsets = null)
        {
            animations.Add(name, new Animation(srcSize, srcRects, isLooping, durations, offsets));
        }
        
        public void SetAnimation(string name, 
            int animationIndex = -1)
        {
            animation = animations[name];

            //Set animation index
            if (animationIndex == 0)
                isBeginningAnimation = true;
            else if (animationIndex < 0)
                animationIndex = this.animationIndex;

            else if (animationIndex >= animation.Count)
                curAnimationTimer = new TimeSpan(0, 0, 0, 0, (int)animation.TotalDuration);

            else
                curAnimationTimer = new TimeSpan(0, 0, 0, 0, (int)animation.GetDuration(animationIndex));

            this.animationIndex = animation.GetAnimationIndex(curAnimationTimer.TotalMilliseconds);
        }

        //Returns name of animation currently playing
        public string GetAnimation()
        {
            foreach (KeyValuePair<string, Animation> ani in animations)
                if (ani.Value == animation)
                    return ani.Key;
            return "";
        }

        //To be run by every frame by entity
        public void Update(GameTime gt)
        {
            curAnimationTimer += gt.ElapsedGameTime;
            if (isBeginningAnimation)
            {
                isBeginningAnimation = false;
                curAnimationTimer = TimeSpan.Zero;
            }

            animationOver = false;

            //If animation isn't set, still try to run
            if (animation == null)
            {
                if (animations.ContainsKey("neutral"))
                    animation = animations["neutral"];

                else
                    animation = animations.FirstOrDefault().Value;

                animationIndex = 0;
            }

            animationIndex = animation.GetAnimationIndex(curAnimationTimer.TotalMilliseconds);
            
            if (curAnimationTimer.TotalMilliseconds >= animation.TotalDuration)
            {
                if (animation.IsLooping)
                {
                    //Reset animation to beginning
                    animationIndex = 0;
                    curAnimationTimer = new TimeSpan(0);
                }
                else
                {
                    //Freeze animation on last frame
                    animationIndex = animation.Count - 1;
                    animationOver = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, float x, float y)
        {
            animation ??= animations.FirstOrDefault().Value;
            
            for (int i = 0; i < animation.LayersCount; i++)
            {
                AnimationLayer layer = animation.GetLayer(i);

                Rectangle srcRect = layer[animationIndex];

                Vector2 offset = layer.GetOffset(animationIndex);
                if (spriteEffects == SpriteEffects.FlipHorizontally)
                    offset = new Vector2(animation.SrcSize.X - (srcRect.Width + offset.X), offset.Y);

                Vector2 screenPos = new Vector2(x, y) + offset * scale;

                spriteBatch.Draw(spriteSheet, //Texture
                    screenPos * gameScale, //Position
                    srcRect, //Source Rectangle
                    Color.White, // Color Tint
                    angle, //Rotation Angle
                    origin, //Origin Of Sprite (where to rotate around)
                    scale * gameScale, //Scale
                    spriteEffects, //Sprite Effects
                    this.layer); //Layer
            }
        }
    }
}
