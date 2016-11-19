using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using S00141926_ThomasCrudden;

namespace S00141926_ThomasCrudden
{
   
    public class SimpleKeyboard
    {
        Vector2 _position;

        SpriteBatch batch;
        SpriteFont font;
        Texture2D keyBackground;

        List<SpriteKey> keys = new List<SpriteKey>();

        public string Output { get; set; }
        
        public SimpleKeyboard(Vector2 position)
        {
            _position = position;
            Output = "";
        }

        public void LoadContent(ContentManager _content)
        {
            font = _content.Load<SpriteFont>("keyboardfont");
            batch = new SpriteBatch(Helpers.GraphicsDevice);
            keyBackground = _content.Load<Texture2D>("key");

            CreateKeys();
        }

        private void CreateKeys()
        {
            keys.Add(new SpriteKey(50, "A", new Vector2(0, 0)));
            keys.Add(new SpriteKey(50, "B", new Vector2(50, 0)));
            keys.Add(new SpriteKey(50, "C", new Vector2(100, 0)));
            keys.Add(new SpriteKey(50, "D", new Vector2(150, 0)));
            keys.Add(new SpriteKey(50, "E", new Vector2(200, 0)));
            keys.Add(new SpriteKey(50, "F", new Vector2(250, 0)));
            keys.Add(new SpriteKey(50, "G", new Vector2(300, 0)));
            keys.Add(new SpriteKey(50, "H", new Vector2(350, 0)));
            keys.Add(new SpriteKey(50, "I", new Vector2(400, 0)));
            keys.Add(new SpriteKey(50, "J", new Vector2(450, 0)));
            keys.Add(new SpriteKey(50, "K", new Vector2(500, 0)));
            keys.Add(new SpriteKey(50, "L", new Vector2(550, 0)));
            keys.Add(new SpriteKey(50, "M", new Vector2(600, 0)));

            keys.Add(new SpriteKey(50, "N", new Vector2(0, 50)));
            keys.Add(new SpriteKey(50, "O", new Vector2(50, 50)));
            keys.Add(new SpriteKey(50, "P", new Vector2(100, 50)));
            keys.Add(new SpriteKey(50, "Q", new Vector2(150, 50)));
            keys.Add(new SpriteKey(50, "R", new Vector2(200, 50)));
            keys.Add(new SpriteKey(50, "S", new Vector2(250, 50)));
            keys.Add(new SpriteKey(50, "T", new Vector2(300, 50)));
            keys.Add(new SpriteKey(50, "U", new Vector2(350, 50)));
            keys.Add(new SpriteKey(50, "V", new Vector2(400, 50)));
            keys.Add(new SpriteKey(50, "W", new Vector2(450, 50)));
            keys.Add(new SpriteKey(50, "X", new Vector2(500, 50)));
            keys.Add(new SpriteKey(50, "Y", new Vector2(550, 50)));
            keys.Add(new SpriteKey(50, "Z", new Vector2(600, 50)));

            keys.Add(new SpriteKey(650, 50, "Space", new Vector2(0, 100)));
            keys.Add(new SpriteKey(100, 150, "Delete", new Vector2(600, 0)));

            foreach (var k in keys)
                k.Position += _position;
        }

        public  void Update(GameTime gametime)
        {
            foreach (var key in keys)
            {
                var rect = new Rectangle((int)key.Position.X, (int)key.Position.Y, key.Width, key.Height);

                if (rect.Contains((int)InputEngine.MousePosition.X, (int)InputEngine.MousePosition.Y))
                {
                    key.IsMouseOver = true;

                    if (InputEngine.IsMouseLeftClick())
                    {
                        if (key.Text == "Space")
                        {
                            Output += " ";
                        }
                        else if (key.Text == "Delete")
                        {
                            if (Output.Length > 0)
                                Output = Output.Remove(Output.Length - 1);
                        }
                        else
                        {
                            Output += key.Text;
                        }
                    }
                }
                else
                {
                    key.IsMouseOver = false;
                }
                
            }

        }

        public  void Draw()
        {
            batch.Begin();
            
            foreach (var key in keys)
            {
                if (!key.IsMouseOver)
                    batch.Draw(keyBackground, new Rectangle((int)key.Position.X, (int)key.Position.Y, key.Width, key.Height), Color.White);
                else
                    batch.Draw(keyBackground, new Rectangle((int)key.Position.X, (int)key.Position.Y, key.Width, key.Height), Color.Black);

                batch.DrawString(font, key.Text, key.Position, Color.White);
            }
            
            batch.End();

        }
    }

    public class SpriteKey
    {
        public string Text;
        public Vector2 Position;
        public int Width;
        public int Height;

        public bool IsMouseOver;

        public SpriteKey(int size, string text, Vector2 position)
        {
            Width = size;
            Height = size;
            Text = text;
            Position = position;
        }

        public SpriteKey(int width, int height, string text, Vector2 position)
        {
            Width = width;
            Height = height;
            Text = text;
            Position = position;
        }
    }
}
