using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace S00141926_ThomasCrudden
{

    class GetGameInputComponent : DrawableGameComponent
    {
        InputEngine input;
        SimpleKeyboard keyboard;
        SpriteFont sfont;
        string name = string.Empty;
        string password = string.Empty;
        bool firstText = false;
        private SpriteBatch spriteBatch;
        public bool Done = false;
        public bool IsMouseVisible { get; private set; }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public string Password
        {
            get
            {
                return password;
            }

            set
            {
                password = value;
            }
        }

        public string Output
        {
            get
            {
                return output;
            }

            set
            {
                output = value;
            }
        }

        public GetGameInputComponent(Game g) : base(g)
        {
            g.Components.Add(this);
            IsMouseVisible = true;
            input = new InputEngine(g);
            keyboard = new SimpleKeyboard(new Vector2(10, 10));
        }

        public void Clear()
        {
            firstText = false;
            Name = string.Empty;
            output = string.Empty;
            input.KeysPressedInLastFrame.Clear();
            keyboard.Output = string.Empty;
            InputEngine.ClearState();
            Done = false;
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            sfont = Game.Content.Load<SpriteFont>("keyboardfont");
            keyboard.LoadContent(Game.Content);
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            keyboard.Update(gameTime);

            if (InputEngine.IsKeyPressed(Keys.Enter) && !firstText && !Done)
            {
                Name = Output; 
                Output = string.Empty;
                firstText = true;
                keyboard.Output = string.Empty;
                InputEngine.ClearState();
            }
            if (InputEngine.IsKeyPressed(Keys.Enter) && firstText && !Done)
            {
                Output = string.Empty;
                keyboard.Output = string.Empty;
                Done = true;
                Enabled = false;
                Visible = false;
            }
            if(InputEngine.IsKeyPressed(Keys.Back))
                if (Output.Length > 0)
                    Output = Output.Remove(Output.Length - 1);
            if (InputEngine.IsKeyPressed(Keys.Space))
                Output += " ";

                base.Update(gameTime);
        }

        string output = "";

        public override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(Color.Black);
            if (!firstText)
                foreach (var s in input.KeysPressedInLastFrame)
                    Output += s;
            else
                foreach (var s in input.KeysPressedInLastFrame)
                {
                    Output += "*";
                    Password += s;
                }

            keyboard.Draw();
            if (Done) return;
            spriteBatch.Begin();
            //spriteBatch.DrawString(sfont, keyboard.Output, new Vector2(10, GraphicsDevice.Viewport.Height - 30), Color.LawnGreen);
            if(!firstText)
                spriteBatch.DrawString(sfont, Output, new Vector2(10, GraphicsDevice.Viewport.Height - 60), Color.Blue);
            else spriteBatch.DrawString(sfont, Output, new Vector2(10, GraphicsDevice.Viewport.Height - 60), Color.Blue);
            //spriteBatch.DrawString(sfont, Name, new Vector2(200, 400), Color.White);
            //spriteBatch.DrawString(sfont, Password, new Vector2(200, 440), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
