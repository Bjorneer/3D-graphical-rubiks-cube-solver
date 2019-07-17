using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.IO;
using RubiksCube3D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Screens.ScreenClasses
{
    class SolutionControl
    {
        Texture2D playTexture;
        Texture2D pauseTexture;

        Button mainControlButton;
        Button forwardButton;
        Button backwardButton;
        Button fastForwardButton;
        Button fastBackwardButton;

        private bool _isPlaying = false;
        public bool Play
        {
            get
            {
                return _isPlaying;
            }
            set
            {
                _isPlaying = value;
                if (_isPlaying)
                {
                    Played();
                }
                else
                {
                    Paused();
                }
            }
        }

        public EventHandler OnPlay;
        public EventHandler OnPause;
        public EventHandler OnFastForward;
        public EventHandler OnForward;
        public EventHandler OnBackward;
        public EventHandler OnFastBackward;

        protected virtual void Played()
        {
            mainControlButton.Texture = pauseTexture;
            EventHandler e = OnPlay;
            e?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void Paused()
        {
            mainControlButton.Texture = playTexture;
            EventHandler e = OnPause;
            e?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void FastForward()
        {
            EventHandler e = OnFastForward;
            e?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void FastBackward()
        {
            EventHandler e = OnFastBackward;
            e?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void Forward()
        {
            EventHandler e = OnForward;
            e?.Invoke(this, EventArgs.Empty);
        }
        protected virtual void Backward()
        {
            EventHandler e = OnBackward;
            e?.Invoke(this, EventArgs.Empty);
        }

        public SolutionControl(ContentManager content)
        {
            playTexture = content.Load<Texture2D>("Sprites/PlayBtn");
            pauseTexture = content.Load<Texture2D>("Sprites/PauseBtn");
            mainControlButton = new Button(new Sprite2D(playTexture, new Rectangle(20 + 200 - 30, 20 + 300 + 5, 60, 50)));
            mainControlButton.Click += new EventHandler((s, e) => 
            {
                _isPlaying = !_isPlaying;
                if (_isPlaying)
                {
                    Played();
                }
                else
                {
                    Paused();
                }
            });
            forwardButton = new Button(new Sprite2D(content.Load<Texture2D>("Sprites/ForwardBtn"), new Rectangle(220 + 35, 325, 60, 50)));
            forwardButton.Click += new EventHandler((s, e) =>
            {
                Forward();
            });
            fastForwardButton = new Button(new Sprite2D(content.Load<Texture2D>("Sprites/FastForwardBtn"), new Rectangle(220 + 35 + 65, 325, 60, 50)));
            fastForwardButton.Click += new EventHandler((s, e) =>
            {
                FastForward();
            });
            backwardButton = new Button(new Sprite2D(content.Load<Texture2D>("Sprites/BackwardsBtn"), new Rectangle(220 - 95, 325, 60, 50)));
            backwardButton.Click += new EventHandler((s, e) =>
            {
                Backward();
            });
            fastBackwardButton = new Button(new Sprite2D(content.Load<Texture2D>("Sprites/FastBackwardsBtn"), new Rectangle(220 - 95 - 65, 325, 60, 50)));
            fastBackwardButton.Click += new EventHandler((s, e) =>
            {
                FastBackward();
            });
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            mainControlButton.Draw(spriteBatch);
            forwardButton.Draw(spriteBatch);
            backwardButton.Draw(spriteBatch);
            fastForwardButton.Draw(spriteBatch);
            fastBackwardButton.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime, Input current, Input previous)
        {
            mainControlButton.Update(current, previous);
            forwardButton.Update(current, previous);
            backwardButton.Update(current, previous);
            fastBackwardButton.Update(current, previous);
            fastForwardButton.Update(current, previous);
        }
    }
}
