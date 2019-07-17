using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RubiksCube3D.Animation;
using RubiksCube3D.Interfaces;
using RubiksCube3D.IO;
using RubiksCube3D.Managers;
using RubiksCube3D.Models;
using RubiksCube3D.Options;
using RubiksCube3D.Screens;
using System;

namespace RubiksCube3D
{
    public class GameEngine : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        ScreenManager screenManager;

        public static Random Random;
        public static IntPtr WindowPtr;
        public GameEngine()
        {
            WindowPtr = Window.Handle;
            graphics = new GraphicsDeviceManager(this);
            Window.Title = "Rubiks Cube Solver";
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            Settings.Instance.Initialize(graphics);
            IsMouseVisible = true;
            screenManager = new ScreenManager(new MainMenuScreen(), this);
            Random = new Random();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

        }

        protected override void UnloadContent()
        { }

        Input currentInput = new Input();
        Input previousInput = new Input();
        protected override void Update(GameTime gameTime)
        {
            previousInput.Keyboard = currentInput.Keyboard;
            previousInput.Mouse = currentInput.Mouse;
            currentInput.Update();
            screenManager.Update(gameTime, currentInput, previousInput);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            screenManager.Draw(gameTime, spriteBatch);
            base.Draw(gameTime);
        }
    }
}
