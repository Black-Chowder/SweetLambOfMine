using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackMagic
{
    public class Globals
    {
        public static ContentManager content;
        public static SpriteBatch spriteBatch;

        public static SpriteFont defaultFont;

        public static string GameState = "";
        public static Camera Camera;

        public static EntityBatch MainEntityBatch;
    }
}
