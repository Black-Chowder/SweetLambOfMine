using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BlackMagic
{

    public class Camera
    {
        public static Random rand = new Random();

        //Position
        public Vector2 Pos { get; private set; } = Vector2.Zero;
        public float X 
        { 
            get => Pos.X; 
            private set => Pos = new Vector2(value, Pos.Y); 
        }
        public float Y 
        { 
            get => Pos.Y; 
            private set => Pos = new Vector2(Pos.X, value); 
        }

        //Size
        public const int defaultWidth = 1920;
        public const int defaultHeight = 1080;
        public int Width { get; private set; } = defaultWidth;
        public int Height { get; private set; } = defaultHeight;

        //Game scaler
        public float GameScale { get; private set; } = 1f;
        public float Scale { get; private set; } = 2f;

        //Zoom
        public float Zoom { get; set; } = 1f;

        //Target position.  Where the camera wants to go
        private float targetX = 0;
        private float targetY = 0;

        //Speed
        public float Speed { get; set; } = 15f;

        //Camera Shake Variables
        private float timer = 0;
        private float intensity = 0;

        //Manual Mouse Control (Mostly To Be Used For Testing)
        private bool mouseControl = true;
        private Vector2 deltaMouse = Vector2.Zero;

        //To run every frame
        public void Update(GameTime gameTime)//TODO: Account for gametime
        {
            camShakeHandler(gameTime);

            //Mouse control stuff
            MouseState mouse = Mouse.GetState();
            if (mouseControl) MouseControlHandler(mouse);
            deltaMouse = mouse.Position.ToVector2();

            //Move camera to requested location based on speed
            X += (targetX - X) / Speed;
            Y += (targetY - Y) / Speed;
        }

        //Update gamescale according to screen dimensions
        private void updateGameScale()
        {
            GameScale = Width / (float)defaultWidth;
            Console.WriteLine("Game Scale Updated.  New Game Scale = " + GameScale);
        }

        //Sets window dimensions to fullscreen (TODO: Figure out if this works if using camera with render target of different size)
        public void SetDimensions(GraphicsDeviceManager graphics, GraphicsDevice GraphicsDevice)
        {
            SetDimensions(graphics, GraphicsDevice.DisplayMode.Width, GraphicsDevice.DisplayMode.Height, true);
        }

        //Sets window dimensions to requested width and height (TODO: Figure out if this works if using camera with render target of different size)
        public void SetDimensions(GraphicsDeviceManager graphics, int Width = defaultWidth, int Height = defaultHeight, bool isFullScreen = true)
        {
            //Actually change window dimensions
            graphics.PreferredBackBufferWidth = Width;
            graphics.PreferredBackBufferHeight = Height;
            graphics.IsFullScreen = isFullScreen;
            graphics.ApplyChanges();

            //Stores new changes
            this.Width = Width;
            this.Height = Height;

            updateGameScale();
        }

        //Force the camera to go to a specific location
        public void SudoGoTo(float x, float y)
        {
            //Sets both camera and target to same location
            this.X = x;
            this.Y = y;
            targetX = x;
            targetY = y;
        }

        //Travel to target location according to speed
        public void GoTo(float x, float y)
        {
            targetX = x;
            targetY = y;
        }

        //Public method to make camera shake
        public void Shake(float intensity, float duration)
        {
            timer = duration;
            this.intensity = intensity;

        }

        //Handle camera shaking (TODO: Implement better camera shaking algorithm)
        private void camShakeHandler(GameTime gameTime)
        {
            //Only run if timer is more than 0
            if (timer <= 0) return;

            //Update timer
            timer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

            //Pick close positions to go to
            //TODO: Adjust so that these are picked in a circle, not a square (using sin and cos)
            float dx = ((float)rand.NextDouble() * 2 - 1) * intensity;
            float dy = ((float)rand.NextDouble() * 2 - 1) * intensity;
            

            X += dx;
            Y += dy;


            Console.WriteLine("Shaking!\nTimer = " + timer + " | Elapsed Game Time = " + gameTime.ElapsedGameTime.TotalMilliseconds);
        }

        private void MouseControlHandler(MouseState mouse)
        {
            if (mouse.MiddleButton == ButtonState.Pressed)
            {
                SudoGoTo(X + (deltaMouse.X - mouse.X) / GameScale, Y + (deltaMouse.Y - mouse.Y) / GameScale);
            }
        }
    }
}
