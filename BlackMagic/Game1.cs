using BlackMagic.Properties;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace BlackMagic
{
    public class Game1 : Game
    {
        public static GraphicsDeviceManager graphics;

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
            Globals.GameState = "Start";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Globals.spriteBatch = new SpriteBatch(GraphicsDevice);
            Globals.content = Content;
            Globals.defaultFont = Content.Load<SpriteFont>("DefaultFont");

            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ClickHandler.Update();

            switch (Globals.GameState)
            {
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

            switch (Globals.GameState)
            {
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