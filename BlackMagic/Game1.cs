using BlackMagic.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct2D1.Effects;
using SharpDX.Direct3D9;
using System.Diagnostics;

namespace BlackMagic
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;

        Texture2D title;
        Texture2D tutorial;
        Texture2D start;
        bool first = true;

        Song song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            IsMouseVisible = true;

            //graphics.IsFullScreen = true;
            //graphics.PreferredBackBufferWidth = 1920;
            //graphics.PreferredBackBufferHeight = 1080;
            //graphics.ApplyChanges();

            Globals.Camera = new Camera();
            Globals.Camera.SetDimensions(graphics, 1920, 1080, false);

            //Add Penumbra Component
            //penumbra = new PenumbraComponent(this);
            //Components.Add(penumbra);
            //penumbra.AmbientColor = Color.Black;
        }

        protected override void Initialize()
        {
            Globals.GameState = "Title";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.content = Content;
            Globals.defaultFont = Content.Load<SpriteFont>("DefaultFont");

            title = Globals.content.Load<Texture2D>("title");
            tutorial = Globals.content.Load<Texture2D>("tutorial");
            start = Globals.content.Load<Texture2D>("generatedtext_2");

            song = Globals.content.Load<Song>("Eggy_Toast_-_Ghost.mp3");


            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ClickHandler.Update();

            switch (Globals.GameState)
            {
                case "Title":
                    if (ClickHandler.IsClicked(Keys.Space))
                    {
                        Globals.GameState = "Instructions";
                    }
                    break;

                case "Instructions":
                    if (ClickHandler.IsClicked(Keys.Space))
                    {
                        Globals.GameState = "Start";
                    }
                    break;

                case "Start":
                    Globals.MainEntityBatch = new EntityBatch();

                    Globals.GameState = "MainLoop";
                    break;

                case "MainLoop":
                    Globals.MainEntityBatch.Update(gameTime);
                    break;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //penumbra.BeginDraw();
            GraphicsDevice.Clear(Color.Purple);

            MediaPlayer.IsRepeating = true;
            if (first) MediaPlayer.Play(song);
            first = false;

            switch (Globals.GameState)
            {
                case "Title":
                    Globals.spriteBatch.Begin();

                    Globals.spriteBatch.Draw(title, //Texture
                        new Vector2(0, 0), //Position
                    new Rectangle(0, 0, 1920, 1080), //Source Rectangle
                        Color.White, // Color Tint
                        0, //Rotation Angle
                        new Vector2(.5f, 1), //Origin Of Sprite (where to rotate around)
                        1, //Scale
                        SpriteEffects.None, //Sprite Effects
                        0f); //Layer

                    Globals.spriteBatch.Draw(start, //Texture
                        new Vector2(Globals.Camera.Width / 2 - 296 / 2, 300), //Position
                    new Rectangle(0, 0, 1920, 1080), //Source Rectangle
                        Color.White, // Color Tint
                        0, //Rotation Angle
                        new Vector2(.5f, 1), //Origin Of Sprite (where to rotate around)
                        1, //Scale
                        SpriteEffects.None, //Sprite Effects
                        0f); //Layer
                    Globals.spriteBatch.End();
                    break;
                case "Instructions":
                    Globals.spriteBatch.Begin();
                    Globals.spriteBatch.Draw(tutorial, //Texture
                        new Vector2(0, 0), //Position
                    new Rectangle(0, 0, 1920, 1080), //Source Rectangle
                        Color.White, // Color Tint
                        0, //Rotation Angle
                        new Vector2(.5f, 1), //Origin Of Sprite (where to rotate around)
                        1, //Scale
                        SpriteEffects.None, //Sprite Effects
                        0f); //Layer
                    Globals.spriteBatch.End();
                    break;

                case "Start":

                    break;
                case "MainLoop":
                    Globals.MainEntityBatch.Draw();
                    break;
            }

            base.Draw(gameTime);
        }
    }
}