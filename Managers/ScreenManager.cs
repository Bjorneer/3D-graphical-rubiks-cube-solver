using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.IO;
using RubiksCube3D.Interfaces;
using Microsoft.Xna.Framework;

namespace RubiksCube3D.Managers
{
    class ScreenManager
    {
        #region Fields

        private Stack<IScreen> _listOfScreens = new Stack<IScreen>();
        private IScreen _currentScreen;

        private GameEngine _game;

        public GraphicsDevice GraphicsDevice;
        public IServiceProvider ServiceProvider;

        #endregion

        #region Constructors

        public ScreenManager(IScreen startScreen, GameEngine game)
        {
            _game = game;
            GraphicsDevice = game.GraphicsDevice;
            ServiceProvider = game.Services;

            _currentScreen = startScreen;
            PushScreen(_currentScreen);

            _currentScreen.Initialize(this);
        }

        #endregion

        #region Methods

        public void PushScreen(IScreen screen)
        {
            _listOfScreens.Push(screen);
        }

        public void PopScreen()
        {
            IScreen removed = _listOfScreens.Peek();
            removed.Dispose();
            _listOfScreens.Pop();
            if (_listOfScreens.Count != 0)
            {
                _currentScreen = _listOfScreens.Peek();
            }
        }

        public void RemoveAllScreens()
        {
            while (_listOfScreens.Count > 0)
            {
                PopScreen();
            }
        }

        public void UpdateScreen()
        {
            if (_listOfScreens.Peek() != _currentScreen)
            {
                _currentScreen = _listOfScreens.Peek();

                _currentScreen.Initialize(this);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            _currentScreen.Draw(gameTime, spriteBatch);
        }

        public void ExitGame()
        {
            RemoveAllScreens();
            _game.Exit();
        }

        public void Update(GameTime gameTime, Input curInput, Input prevInput)
        {
            UpdateScreen();
            _currentScreen.Update(gameTime, curInput, prevInput);
        }

        #endregion
    }
}
