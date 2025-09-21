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


        enum Screen
        {
            Intro,
            Game,
            End
        }

        MouseState mouseState, prevMouseState;
        KeyboardState keyboardState;
        List<Grub> grubs;
        Grub introGrub1;
        Grub introGrub2;
        Grub introGrub3;
        SoundEffectInstance musicInstance;
        SoundEffect greenpathMusic;
        SpriteFont gameFont;

        List<Texture2D> grubIdle;
        List<Texture2D> grubAlert;
        List<Texture2D> grubFreed;
        List<Texture2D> grubBounce;
        List<Texture2D> grubWave;

        List<SoundEffect> idleEffect;
        List<SoundEffect> alertEffect;
        List<SoundEffect> freedEffect;
        SoundEffect burrowEffect;
        SoundEffect breakEffect;

        Texture2D grubJarTexture;
        Texture2D gameBackground;
        Texture2D introBackground;
        Rectangle window;
        float grubTimer;
        int prevWheelValue;
        int grubAmount;
        int grubsOnScreen;
        Screen screen;



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
            screen = Screen.Intro;
            grubAmount = 0;

            grubIdle = new List<Texture2D>();
            grubAlert = new List<Texture2D>();
            grubFreed = new List<Texture2D>();
            grubBounce = new List<Texture2D>();
            grubWave = new List<Texture2D>();

            idleEffect = new List<SoundEffect>();
            alertEffect = new List<SoundEffect>();
            freedEffect = new List<SoundEffect>();

            base.Initialize();

            musicInstance.IsLooped = true;
            musicInstance.Play();

            introGrub1 = new Grub(grubBounce, grubWave, 985, 350, 300);
            introGrub2 = new Grub(grubBounce, grubWave, 60, 430, 100);
            introGrub3 = new Grub(grubBounce, grubWave, 730, 470, 50);

            grubs = new List<Grub>();
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
            for (int i = 0; i <= 3; i++)
                grubBounce.Add(Content.Load<Texture2D>("Grubs/Images/Home Idle 12/Home Bounce_" + i.ToString("D3")));
            for (int i = 0; i <= 22; i++)
                grubWave.Add(Content.Load<Texture2D>("Grubs/Images/Home Wave 12/Home Wave_" + i.ToString("D3")));
            grubJarTexture = Content.Load<Texture2D>("Grubs/Images/grub_jar");
            gameBackground = Content.Load<Texture2D>("Grubs/Images/grass_background");
            introBackground = Content.Load<Texture2D>("Grubs/Images/intro_background");

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

            // Fonts
            gameFont = Content.Load<SpriteFont>("Grubs/Fonts/gameFont");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            prevMouseState = mouseState;
            prevWheelValue = mouseState.ScrollWheelValue;
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();

            if (screen == Screen.Intro)
            {
                introGrub1.Update(gameTime);
                introGrub2.Update(gameTime);
                introGrub3.Update(gameTime);
                if (keyboardState.IsKeyDown(Keys.Enter))
                {
                    screen = Screen.Game;
                }
            }

            else if (screen == Screen.Game)
            {
                grubTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                if (grubTimer >= 2 || prevWheelValue > mouseState.ScrollWheelValue)
                {
                    grubTimer = 0;
                    if (grubs.Count <= 25 && grubsOnScreen + grubAmount < 50)
                    {
                        grubs.Add(new Grub(grubIdle, idleEffect, grubAlert, alertEffect, grubFreed, freedEffect, grubJarTexture, breakEffect, burrowEffect));
                        grubsOnScreen++;
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
                        grubAmount++;
                        grubsOnScreen--;
                    }
                }

                if (grubAmount >= 50 && keyboardState.IsKeyDown(Keys.R))
                {
                    screen = Screen.Intro;
                    grubAmount = 0;
                    grubsOnScreen = 0;
                }
            }


                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();


            if (screen == Screen.Intro)
            {
                _spriteBatch.Draw(introBackground, Vector2.Zero, Color.White);
                _spriteBatch.DrawString(gameFont, new string("Grub Savior!"), new Vector2(350, 30), Color.Black, 0, Vector2.Zero, 1.2f, SpriteEffects.None, 0);
                _spriteBatch.DrawString(gameFont, new string("Press 'ENTER' to play"), new Vector2(450, 300), Color.Black, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                introGrub1.Draw(_spriteBatch, false);
                introGrub2.Draw(_spriteBatch, true);
                introGrub3.Draw(_spriteBatch, false);
            }

            else if (screen == Screen.Game)
            {
                _spriteBatch.Draw(gameBackground, Vector2.Zero, Color.White);
                _spriteBatch.DrawString(gameFont, new string($"{grubAmount}/50"), new Vector2(1070, 30), Color.Black);

                for (int i = 0; i < grubs.Count; i++)
                {
                    grubs[i].Draw(_spriteBatch);
                }
                if (grubAmount >= 50)
                {
                    _spriteBatch.DrawString(gameFont, new string("Congratulations!"), new Vector2(270, 200), Color.Black);
                    _spriteBatch.DrawString(gameFont, new string("You saved all the grubs!"), new Vector2(300, 300), Color.Black, 0, Vector2.Zero, 0.7f, SpriteEffects.None, 0);
                    _spriteBatch.DrawString(gameFont, new string("Press 'R' to return to the home screen."), new Vector2(327, 450), Color.Black, 0, Vector2.Zero, 0.4f, SpriteEffects.None, 0);
                }
            }
            

                _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
