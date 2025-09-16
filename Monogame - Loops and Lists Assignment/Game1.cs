using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
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

        List<SoundEffect> idleEffect;
        List<SoundEffect> alertEffect;
        List<SoundEffect> freedEffect;

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

            idleEffect = new List<SoundEffect>();
            alertEffect = new List<SoundEffect>();
            freedEffect = new List<SoundEffect>();

            base.Initialize();

            grubs = new List<Grub>();
            for (int i = 0; i < 10; i++)
            {
                grubs.Add(new Grub(grubIdle, idleEffect, grubAlert, alertEffect, grubFreed, freedEffect, grubJarTexture));
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Images

            for (int i = 0; i <= 6; i++)
                grubAlert.Add(Content.Load<Texture2D>("Grubs/Images/Alert 12/Cry_00" + i));
            for (int i = 0; i <= 19; i++)
                grubFreed.Add(Content.Load<Texture2D>("Grubs/Images/Freed 10/Freed_" + i.ToString("D3")));
            for (int i = 0; i <= 37; i++)
                grubIdle.Add(Content.Load<Texture2D>("Grubs/Images/Idle 12/Idle_" + i.ToString("D3")));
            grubJarTexture = Content.Load<Texture2D>("Grubs/Images/grub_jar");

            // Sound Effects

            for (int i = 1; i <= 3; i++)
                alertEffect.Add(Content.Load<SoundEffect>("Grubs/Sound Effects/Alert/grub_alert_" + i));
            for (int i = 1; i <= 2; i++)
                freedEffect.Add(Content.Load<SoundEffect>("Grubs/Sound Effects/Freed/grub_free_" + i));
            for (int i = 1; i <= 3; i++)
                idleEffect.Add(Content.Load<SoundEffect>("Grubs/Sound Effects/Sad/grub_sad_" + i));
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
