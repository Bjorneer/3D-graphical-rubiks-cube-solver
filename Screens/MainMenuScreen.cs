using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.Interfaces;
using RubiksCube3D.IO;
using RubiksCube3D.Managers;
using RubiksCube3D.Models;
using RubiksCube3D.Rubiks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Screens
{
    class MainMenuScreen : IScreen
    {
        ScreenManager screenManager;
        GraphicsDevice GraphicsDevice;

        private ContentManager _content;

        Sprite2D title;

        Button rubiksSolverButton;
        Camera camera;
        RubiksCube cube;

        Button settingsButton;
        Sprite2D gearSprite;
        Sprite2D smallGearSprite;

        Button exitButton;
        Sprite2D closedDoor;
        Sprite2D openDoor;

        #region Initialize
        public void Initialize(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            this.GraphicsDevice = screenManager.GraphicsDevice;
            _content = new ContentManager(screenManager.ServiceProvider, Constants.CONTENT_DIRECTORY);
            InitializeButtons();
            InitializeTitle();
        }

        private void InitializeButtons()
        {
            Texture2D buttonTexture = _content.Load<Texture2D>("Sprites/MenuButton");
            InitializeRubiksButton(buttonTexture);

            InitializeOptionsButton(buttonTexture);

            InitializeExitButton(buttonTexture);
        }

        private void InitializeExitButton(Texture2D buttonTexture)
        {
            exitButton = new Button(new Sprite2D(buttonTexture, new Rectangle(980, 300, 200, 200)));
            exitButton.Angle = 0.2f;
            exitButton.HoverAnimation = new Animation.ButtonAnimation(null, new Rectangle(980, 300, 220, 220), null);
            exitButton.UnHoverAnimation = new Animation.ButtonAnimation(null, new Rectangle(980, 300, 200, 200), null);
            exitButton.Click += new EventHandler((s, e) => { screenManager.ExitGame(); });

            closedDoor = new Sprite2D(_content.Load<Texture2D>("Sprites/ClosedDoor"), new Rectangle(0, 0, 50, 100));
            closedDoor.Center(exitButton.Bounds);
            closedDoor.Angle = 0.2f;
            openDoor = new Sprite2D(_content.Load<Texture2D>("Sprites/OpenDoor"), new Rectangle(0, 0, 50, 100));
            openDoor.Center((Rectangle)exitButton.HoverAnimation.Bounds);
            openDoor.Show = false;
            openDoor.Angle = 0.2f;
            exitButton.Hover += new EventHandler((s, e) =>
            {
                openDoor.Show = true;
                closedDoor.Show = false;
            });
            exitButton.UnHover += new EventHandler((s, e) =>
            {
                openDoor.Show = false;
                closedDoor.Show = true;
            });
        }

        private void InitializeOptionsButton(Texture2D buttonTexture)
        {
            settingsButton = new Button(new Sprite2D(buttonTexture, new Rectangle(540, 280, 200, 200)));
            settingsButton.HoverAnimation = new Animation.ButtonAnimation(null, new Rectangle(530, 280, 220, 220), null);
            settingsButton.UnHoverAnimation = new Animation.ButtonAnimation(null, new Rectangle(540, 280, 200, 200), null);

            Texture2D gearTexture = _content.Load<Texture2D>("Sprites/Gear");
            gearSprite = new Sprite2D(gearTexture, new Rectangle(0, 0, 100, 100));
            gearSprite.Center(settingsButton.Bounds);
            smallGearSprite = new Sprite2D(gearTexture, new Rectangle(0, 0, 50, 50));
            smallGearSprite.Location = gearSprite.Location + new Vector2(80, -20);
            smallGearSprite.Angle = -0.13f;
            settingsButton.Hover += new EventHandler((s, e) =>
            {
                gearSprite.Center(settingsButton.Bounds);
                smallGearSprite.Location = gearSprite.Location + new Vector2(80, -20);
            });
            settingsButton.UnHover += new EventHandler((s, e) =>
            {
                gearSprite.Center(settingsButton.Bounds);
                gearSprite.Angle = 0;
                smallGearSprite.Location = gearSprite.Location + new Vector2(80, -20);
                smallGearSprite.Angle = -0.13f;
            });
            settingsButton.Click += new EventHandler((s, e) => 
            {
                screenManager.PopScreen();
                screenManager.PushScreen(new SettingsScreen());
            });
        }

        private void InitializeRubiksButton(Texture2D buttonTexture)
        {
            camera = new Camera(GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height, new Vector3(0, 0, 0));
            camera.Zoom = 300;
            camera.Pitch = 0.3f;
            cube = new RubiksCube(25, new Vector3(150, -10, 0), GraphicsDevice);
            cube.SetColor(CubeSide.Front, Color.Crimson, Color.Red, Color.DarkRed, Color.OrangeRed);
            cube.SetColor(CubeSide.Top, Color.Green, Color.GreenYellow, Color.DarkOliveGreen, Color.LightSeaGreen);
            cube.SetColor(CubeSide.Left, Color.BlueViolet, Color.Cyan, Color.LightBlue, Color.DarkBlue);
            cube.SetColor(CubeSide.Bottom, Color.LightPink, Color.Pink, Color.PaleVioletRed, Color.DeepPink);
            cube.SetColor(CubeSide.Right, Color.Lime, Color.LightGoldenrodYellow, Color.YellowGreen, Color.Gold);
            cube.SetColor(CubeSide.Back, Color.Purple, Color.DarkBlue, Color.MediumPurple, Color.HotPink);
            cube.TimePerRotation = 0.5f;

            rubiksSolverButton = new Button(new Sprite2D(buttonTexture, new Rectangle(100, 300, 200, 200)));
            rubiksSolverButton.Angle = -0.2f;
            rubiksSolverButton.HoverAnimation = new Animation.ButtonAnimation(null, new Rectangle(80, 300, 220, 220), null);
            rubiksSolverButton.UnHoverAnimation = new Animation.ButtonAnimation(null, new Rectangle(100, 300, 200, 200), null);
            rubiksSolverButton.Click += new EventHandler((s, e) =>
            {
                screenManager.PopScreen();
                screenManager.PushScreen(new RubiksReaderScreen());
            });
            rubiksSolverButton.Hover += new EventHandler((s, e) =>
            {
                cube.Position = new Vector3(155, -15, 0);
            });
            rubiksSolverButton.UnHover += new EventHandler((s, e) =>
            {
                cube.Position = new Vector3(150, -10, 0);
                gearSprite.Angle = 0;
            });
        }

        private void InitializeTitle()
        {
            title = new Sprite2D(_content.Load<Texture2D>("Sprites/RubiksCubeTitle"), new Rectangle(20, 20, 1240, 200));
        }
        #endregion

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            //2D
            spriteBatch.Begin(SpriteSortMode.Deferred,BlendState.AlphaBlend);
            rubiksSolverButton.Draw(spriteBatch);
            exitButton.Draw(spriteBatch);
            settingsButton.Draw(spriteBatch);
            title.Draw(spriteBatch);
            gearSprite.Draw(spriteBatch);
            smallGearSprite.Draw(spriteBatch);
            openDoor.Draw(spriteBatch);
            closedDoor.Draw(spriteBatch);
            spriteBatch.End();
            //3D
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            cube.Draw(camera);
        }

        public void Update(GameTime gameTime, Input current, Input previous)
        {
            rubiksSolverButton.Update(current, previous);
            settingsButton.Update(current, previous);
            exitButton.Update(current, previous);
            if (rubiksSolverButton.State == ButtonCondition.Hovered && !cube.InRotation )
            {
                cube.SetRotation((CubeSide)GameEngine.Random.Next(0, 6));
            }
            if (settingsButton.State == ButtonCondition.Hovered)
            {
                gearSprite.Angle = gearSprite.Angle + 0.02f;
                smallGearSprite.Angle = smallGearSprite.Angle + -0.04f;
            }
            cube.UpdateOld();
        }

        public void Dispose()
        {
            _content.Dispose();
        }
    }
}
