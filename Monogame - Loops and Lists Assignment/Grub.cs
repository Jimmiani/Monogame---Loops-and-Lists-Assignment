using Microsoft.Xna.Framework;
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
        private List<Texture2D> _idleAnim;
        private List<Texture2D> _freedAnim;
        private List<Texture2D> _alertAnim;
        private List<Texture2D> _currentAnim;
        private Rectangle _grubRect;
        private Rectangle _jarRect;
        private Texture2D _jarTexture;
        private int _currentFrame;
        private float _animTimer;

        public Grub(List<Texture2D> idleAnim, List<Texture2D> alertAnim, List<Texture2D> freedAnim, Texture2D jarTexture)
        {
            _idleAnim = idleAnim;
            _alertAnim = alertAnim;
            _freedAnim = freedAnim;
            _jarTexture = jarTexture;

            _generator = new Random();
            grubState = GrubState.Idle;
            _currentAnim = _idleAnim;
            _currentFrame = 0;
            _animTimer = 0;
            _grubRect = new Rectangle(_generator.Next(10, 700), _generator.Next(10, 400), 150, 177);
            _jarRect = new Rectangle(_grubRect.X - 25, _grubRect.Y - 49, 208, 227);
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
                    grubState = GrubState.Alert;
                }

            }
            else if (grubState == GrubState.Alert)
            {
                if (!_jarRect.Contains(mouseState.Position))
                {
                    _animTimer = 0;
                    _currentFrame = 0;
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
                    _animTimer = 0;
                    _currentFrame = 0;
                    grubState = GrubState.Freed;
                }
            }
            else if (grubState == GrubState.Freed)
            {
                _currentAnim = _freedAnim;
                if (_animTimer >= 1.0 / 10.0)
                {
                    _currentFrame++;
                    _animTimer = 0;
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
            
            if (grubState != GrubState.Freed && grubState != GrubState.Gone)
            {
                spriteBatch.Draw(_jarTexture, _jarRect, Color.White);
            }
            
        }
    }
}
