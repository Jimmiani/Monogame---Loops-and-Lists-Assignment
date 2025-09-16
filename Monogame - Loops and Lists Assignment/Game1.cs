using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Monogame___Loops_and_Lists_Assignment
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        MouseState mouseState, prevMouseState;
        List<Grub> grubs;

        List<Texture2D> grubIdle;
        List<Texture2D> grubAlert;
        List<Texture2D> grubFreed;

        Texture2D grubJarTexture;



        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.ApplyChanges();


            grubIdle = new List<Texture2D>();
            grubAlert = new List<Texture2D>();
            grubFreed = new List<Texture2D>();

            base.Initialize();

            grubs = new List<Grub>();
            for (int i = 0; i < 10; i++)
            {
                grubs.Add(new Grub(grubIdle, grubAlert, grubFreed, grubJarTexture));
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i <= 6; i++)
                grubAlert.Add(Content.Load<Texture2D>("Grubs/Images/Alert 12/Cry_00" + i));
            for (int i = 0; i <= 19; i++)
                grubFreed.Add(Content.Load<Texture2D>("Grubs/Images/Freed 10/Freed_" + i.ToString("D3")));
            for (int i = 0; i <= 37; i++)
                grubIdle.Add(Content.Load<Texture2D>("Grubs/Images/Idle 12/Idle_" + i.ToString("D3")));
            grubJarTexture = Content.Load<Texture2D>("Grubs/Images/grub_jar");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouseState = mouseState;
            mouseState = Mouse.GetState();

            for (int i = 0; i < grubs.Count; i++)
            {
                grubs[i].Update(mouseState, prevMouseState, gameTime);
            }
            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            for (int i = 0; i < grubs.Count; i++)
            {
                grubs[i].Draw(_spriteBatch);
            }
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
