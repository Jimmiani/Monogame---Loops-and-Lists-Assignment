using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
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
        SoundEffectInstance musicInstance;
        SoundEffect greenpathMusic;

        List<Texture2D> grubIdle;
        List<Texture2D> grubAlert;
        List<Texture2D> grubFreed;

        List<SoundEffect> idleEffect;
        List<SoundEffect> alertEffect;
        List<SoundEffect> freedEffect;
        SoundEffect burrowEffect;
        SoundEffect breakEffect;

        Texture2D grubJarTexture;
        Texture2D background;
        Rectangle window;
        float grubTimer;
        int prevWheelValue;



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

            window = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            grubTimer = 0;
            prevWheelValue = mouseState.ScrollWheelValue;

            grubIdle = new List<Texture2D>();
            grubAlert = new List<Texture2D>();
            grubFreed = new List<Texture2D>();

            idleEffect = new List<SoundEffect>();
            alertEffect = new List<SoundEffect>();
            freedEffect = new List<SoundEffect>();

            base.Initialize();

            musicInstance.IsLooped = true;
            musicInstance.Play();

            grubs = new List<Grub>();
            for (int i = 0; i < 3; i++)
            {
                grubs.Add(new Grub(grubIdle, idleEffect, grubAlert, alertEffect, grubFreed, freedEffect, grubJarTexture, breakEffect, burrowEffect));
                for (int j = 0; j < grubs.Count - 1; j++)
                {

                    if (grubs[i].Hitbox.Intersects(grubs[j].Hitbox))
                    {
                        grubs[i].MoveHitbox();
                        j = -1;
                    }
                }
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
            background = Content.Load<Texture2D>("Grubs/Images/grass_background");

            // Sound Effects

            greenpathMusic = Content.Load<SoundEffect>("Grubs/Sound Effects/Music/Greenpath");
            musicInstance = greenpathMusic.CreateInstance();

            for (int i = 1; i <= 3; i++)
                alertEffect.Add(Content.Load<SoundEffect>("Grubs/Sound Effects/Alert/grub_alert_" + i));
            for (int i = 1; i <= 2; i++)
                freedEffect.Add(Content.Load<SoundEffect>("Grubs/Sound Effects/Freed/grub_free_" + i));
            for (int i = 1; i <= 3; i++)
                idleEffect.Add(Content.Load<SoundEffect>("Grubs/Sound Effects/Sad/grub_sad_" + i));
            burrowEffect = Content.Load<SoundEffect>("Grubs/Sound Effects/Burrow/grub_burrow");
            breakEffect = Content.Load<SoundEffect>("Grubs/Sound Effects/Jar Break/jar_break");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouseState = mouseState;
            prevWheelValue = mouseState.ScrollWheelValue;
            mouseState = Mouse.GetState();
            grubTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            

            if (grubTimer >= 3 || prevWheelValue > mouseState.ScrollWheelValue)
            {
                grubTimer = 0;
                if (grubs.Count <= 25)
                {
                    grubs.Add(new Grub(grubIdle, idleEffect, grubAlert, alertEffect, grubFreed, freedEffect, grubJarTexture, breakEffect, burrowEffect));
                    for (int j = 0; j < grubs.Count - 1; j++)
                    {
                        if (grubs[grubs.Count - 1].Hitbox.Intersects(grubs[j].Hitbox))
                        {
                            grubs[grubs.Count - 1].MoveHitbox();
                            j = -1;
                        }
                    }
                }
            }

            for (int i = 0; i < grubs.Count; i++)
            {
                grubs[i].Update(mouseState, prevMouseState, gameTime);
                if (grubs[i].CurrentState == GrubState.Gone)
                {
                    grubs.RemoveAt(i);
                    i--;
                }
            }
            


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(background, new Vector2(0, 0), Color.White);

            for (int i = 0; i < grubs.Count; i++)
            {
                grubs[i].Draw(_spriteBatch);
            }
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
