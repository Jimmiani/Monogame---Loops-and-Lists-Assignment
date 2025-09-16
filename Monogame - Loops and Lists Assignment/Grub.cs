using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monogame___Loops_and_Lists_Assignment
{

    enum GrubState
    {
        Idle,
        Alert,
        Freed,
        Gone
    }

    internal class Grub
    {
        private GrubState grubState;
        private Random _generator;

        private List<SoundEffect> _alertEffect;
        private List<SoundEffect> _freedEffect;
        private List<SoundEffect> _idleEffect;
        private SoundEffect _breakEffect;
        private SoundEffect _burrowEffect;

        private List<Texture2D> _idleAnim;
        private List<Texture2D> _freedAnim;
        private List<Texture2D> _alertAnim;
        private List<Texture2D> _currentAnim;

        private Rectangle _grubRect;
        private Rectangle _jarRect;
        private Texture2D _jarTexture;

        private int _currentFrame;
        private float _animTimer;
        private float _breakTimer;
        private bool _isBroken;

        public Grub(List<Texture2D> idleAnim, List<SoundEffect> idleEffect, List<Texture2D> alertAnim, List<SoundEffect> alertEffect, List<Texture2D> freedAnim, List<SoundEffect> freedEffect, Texture2D jarTexture, SoundEffect breakEffect, SoundEffect burrowEffect)
        {
            _idleEffect = idleEffect;
            _alertEffect = alertEffect;
            _freedEffect = freedEffect;
            _breakEffect = breakEffect;
            _burrowEffect = burrowEffect;

            _idleAnim = idleAnim;
            _alertAnim = alertAnim;
            _freedAnim = freedAnim;
            _jarTexture = jarTexture;

            _generator = new Random();
            grubState = GrubState.Idle;
            _currentAnim = _idleAnim;
            _currentFrame = 0;
            _animTimer = 0;
            _breakTimer = 0;
            _isBroken = false;
            _jarRect = new Rectangle(_generator.Next(10, 1150), _generator.Next(10, 580), 119, 130);
            _grubRect = new Rectangle(_jarRect.X + 18, _jarRect.Y + 27, 86, 102);
        }

        public void Update(MouseState mouseState, MouseState prevMouseState, GameTime gameTime)
        {
            _animTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (grubState == GrubState.Idle)
            {
                _currentAnim = _idleAnim;

                if (_animTimer >= 1.0 / 12.0)
                {
                    _currentFrame++;
                    _animTimer = 0;
                    if (_currentFrame >= _idleAnim.Count)
                    {
                        _currentFrame = 1;
                    }
                }

                if (_jarRect.Contains(mouseState.Position))
                {
                    _animTimer = 0;
                    _currentFrame = 0;
                    _alertEffect[_generator.Next(0, _alertEffect.Count)].Play();
                    grubState = GrubState.Alert;
                }

            }
            else if (grubState == GrubState.Alert)
            {

                if (!_jarRect.Contains(mouseState.Position) && !_isBroken)
                {
                    _animTimer = 0;
                    _currentFrame = 0;
                    _idleEffect[_generator.Next(0, _idleEffect.Count)].Play();
                    grubState = GrubState.Idle;
                    return;
                }

                _currentAnim = _alertAnim;
                if (_animTimer >= 1.0 / 12.0)
                {
                    _currentFrame++;
                    _animTimer = 0;
                    if (_currentFrame >= _alertAnim.Count)
                    {
                        _currentFrame = 1;
                    }
                }

                if (mouseState.LeftButton == ButtonState.Pressed && prevMouseState.LeftButton == ButtonState.Released)
                {
                    if (!_isBroken)
                        _breakEffect.Play();
                    _isBroken = true;
                }

                if (_isBroken)
                {
                    _breakTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (_breakTimer >= 0.3)
                    {
                        _animTimer = 0;
                        _currentFrame = 0;
                        _freedEffect[_generator.Next(0, _freedEffect.Count)].Play();
                        grubState = GrubState.Freed;
                    }
                }
            }
            else if (grubState == GrubState.Freed)
            {
                _currentAnim = _freedAnim;
                if (_animTimer >= 1.0 / 10.0)
                {
                    _currentFrame++;
                    _animTimer = 0;
                    if (_currentFrame == 15)
                    {
                        _burrowEffect.Play();
                    }
                    if (_currentFrame >= _freedAnim.Count)
                    {
                        grubState = GrubState.Gone;
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (grubState != GrubState.Gone)
            {
                spriteBatch.Draw(_currentAnim[_currentFrame], _grubRect, Color.White);
            }
            
            if (!_isBroken)
            {
                spriteBatch.Draw(_jarTexture, _jarRect, Color.White);
            }
            
        }

        public GrubState CurrentState
        {
            get { return grubState; }
        }

        public Rectangle Hitbox
        {
            get { return _jarRect; }
        }

        public void MoveHitbox()
        {
            _jarRect = new Rectangle(_generator.Next(10, 1150), _generator.Next(10, 580), 119, 130);
            _grubRect = new Rectangle(_jarRect.X + 18, _jarRect.Y + 27, 86, 102);
        }
    }
}
