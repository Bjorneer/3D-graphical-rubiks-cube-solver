using RubiksCube3D.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.IO;
using RubiksCube3D.Managers;
using RubiksCube3D.Models;
using Microsoft.Xna.Framework.Content;
using RubiksCube3D.Animation;
using RubiksCube3D.Screens.SettingsScreenMenus;
using RubiksCube3D.Options;

namespace RubiksCube3D.Screens
{
    class SettingsScreen : IScreen
    {
        Sprite2D settingsMenu;
        ScreenManager screenManager;
        GraphicsDevice GraphicsDevice;

        private ContentManager _content;

        MarkableButtonPanel categories;
        string[] categoriesName = { "Controls", "Audio", "Window", "Rubiks Cube" };
        OptionsButton controlsBtn;
        OptionsButton audioBtn;
        OptionsButton windowBtn;
        OptionsButton rubiksBtn;

        Button applyBtn;
        Button backBtn;
        Button resetBtn;

        AudioMenu audioMenu;
        RubiksCubeMenu rubiksMenu;
        ControlsMenu controlsMenu;
        GraphicsMenu graphicsMenu;

        AudioSettings audioSettings;
        ControlsSettings controlsSettings;
        GraphicsSettings graphicsSettings;
        RubiksCubeSettings rubiksSettings;


        public void Initialize(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            GraphicsDevice = screenManager.GraphicsDevice;
            _content = new ContentManager(screenManager.ServiceProvider, "Content");

            //Background
            settingsMenu = new Sprite2D(_content.Load<Texture2D>("Sprites/SettingsMenu"), new Rectangle(100,50,1080,620));

            audioSettings = Settings.Instance.Audio.Clone() as AudioSettings;
            controlsSettings = Settings.Instance.Controls.Clone() as ControlsSettings;
            graphicsSettings = Settings.Instance.Graphics.Clone() as GraphicsSettings;
            rubiksSettings = Settings.Instance.RubiksCube.Clone() as RubiksCubeSettings;


            SpriteFont font = _content.Load<SpriteFont>("Fonts/Arial24");
            Texture2D boxTexture = _content.Load<Texture2D>("Sprites/Transparent");

            ButtonAnimation hover = new ButtonAnimation(null, null, new Color(0, 255, 255, 150));
            ButtonAnimation unHoverUnMark = new ButtonAnimation(null, null, Color.Transparent);
            ButtonAnimation mark = new ButtonAnimation(null, null, new Color(0, 180, 255, 250));


            //
            //Buttons
            //

            //
            //Controls
            //
            controlsBtn = new OptionsButton(new Sprite2D(boxTexture, new Rectangle(115, 120, 225, 75)));
            controlsBtn.Color = Color.Transparent;
            controlsBtn.HoverAnimation = hover;
            controlsBtn.UnHoverAnimation = unHoverUnMark;
            controlsBtn.UnMarkAnimation = unHoverUnMark;
            controlsBtn.MarkAnimation = mark;
            controlsBtn.TextMessege = new Text(categoriesName[0], font);
            controlsBtn.TextMessege.Center(controlsBtn.Bounds);


            //
            //Audio
            //
            audioBtn = new OptionsButton(new Sprite2D(boxTexture, new Rectangle(115, 195, 225, 75)));
            audioBtn.Color = Color.Transparent;
            audioBtn.HoverAnimation = hover;
            audioBtn.UnHoverAnimation = unHoverUnMark;
            audioBtn.UnMarkAnimation = unHoverUnMark;
            audioBtn.MarkAnimation = mark;
            audioBtn.TextMessege = new Text(categoriesName[1], font);
            audioBtn.TextMessege.Center(audioBtn.Bounds);



            //
            //Window
            //
            windowBtn = new OptionsButton(new Sprite2D(boxTexture, new Rectangle(115, 270, 225, 75)));
            windowBtn.Color = Color.Transparent;
            windowBtn.HoverAnimation = hover;
            windowBtn.UnHoverAnimation = unHoverUnMark;
            windowBtn.UnMarkAnimation = unHoverUnMark;
            windowBtn.MarkAnimation = mark;
            windowBtn.TextMessege = new Text(categoriesName[2], font);
            windowBtn.TextMessege.Center(windowBtn.Bounds);


            //
            //Rubiks
            //
            rubiksBtn = new OptionsButton(new Sprite2D(boxTexture, new Rectangle(115, 345, 225, 75)));
            rubiksBtn.Color = Color.Transparent;
            rubiksBtn.HoverAnimation = hover;
            rubiksBtn.UnHoverAnimation = unHoverUnMark;
            rubiksBtn.UnMarkAnimation = unHoverUnMark;
            rubiksBtn.MarkAnimation = mark;
            rubiksBtn.TextMessege = new Text(categoriesName[3], font);
            rubiksBtn.TextMessege.Center(rubiksBtn.Bounds);


            //
            //Menus
            //
            audioMenu = new AudioMenu(audioSettings);
            audioMenu.Initialize(_content, GraphicsDevice);
            audioMenu.Location = new Vector2(375, 65);
            audioMenu.Show = false;

            rubiksMenu = new RubiksCubeMenu(rubiksSettings);
            rubiksMenu.Initialize(_content, GraphicsDevice);
            rubiksMenu.Location = new Vector2(375, 65);
            rubiksMenu.Show = false;

            controlsMenu= new ControlsMenu(controlsSettings);
            controlsMenu.Initialize(_content, GraphicsDevice);
            controlsMenu.Location = new Vector2(375, 65);
            controlsMenu.Show = false;

            graphicsMenu = new GraphicsMenu(graphicsSettings);
            graphicsMenu.Initialize(_content, GraphicsDevice);
            graphicsMenu.Location = new Vector2(375, 65);
            graphicsMenu.Show = false;


            //
            //Panel
            //
            categories = new MarkableButtonPanel();
            categories.Add(controlsBtn);
            categories.Add(audioBtn);
            categories.Add(windowBtn);
            categories.Add(rubiksBtn);
            categories.SetMarked(0);

            audioBtn.Click += new EventHandler((s, e) => { audioMenu.Show = true; rubiksMenu.Show = false; controlsMenu.Show = false; graphicsMenu.Show = false; });
            windowBtn.Click += new EventHandler((s, e) => { audioMenu.Show = false; rubiksMenu.Show = false; controlsMenu.Show = false; graphicsMenu.Show = true; });
            rubiksBtn.Click += new EventHandler((s, e) => { audioMenu.Show = false; rubiksMenu.Show = true; controlsMenu.Show = false; graphicsMenu.Show = false; });
            controlsBtn.Click += new EventHandler((s, e) => { audioMenu.Show = false; rubiksMenu.Show = false; controlsMenu.Show = true; graphicsMenu.Show = false; });

            //
            //Btns
            //
            backBtn = new Button(new Sprite2D(_content.Load<Texture2D>("Sprites/BackBtn"), new Rectangle(85, 630, 100,75)));
            backBtn.Click += On_ExitButtonClick;

            applyBtn = new Button(new Sprite2D(_content.Load<Texture2D>("Sprites/ApplyBtn"), new Rectangle(1050, 630, 150, 75)));
            applyBtn.Click += On_ApplySettings;

            resetBtn = new Button(new Sprite2D(_content.Load<Texture2D>("Sprites/ResetBtn"), new Rectangle(825, 630, 150, 75)));
            resetBtn.Click += On_ResetSettings;
        }

        private void On_ResetSettings(object sender, EventArgs e)
        {
            Settings.Instance.ResetOptions();
            Dispose();
            Initialize(screenManager);
        }

        private void On_ApplySettings(object sender, EventArgs e)
        {
            Settings.Instance.Audio = audioSettings;
            Settings.Instance.Graphics = graphicsSettings;
            Settings.Instance.Controls = controlsSettings;
            Settings.Instance.RubiksCube = rubiksSettings;
            Settings.Instance.ApplyChanges();
        }

        private void On_ExitButtonClick(object sender, EventArgs e)
        {
            screenManager.PopScreen();
            screenManager.PushScreen(new MainMenuScreen());
        }

        public void Update(GameTime gameTime, Input current, Input previous)
        {
            categories.Update(current,previous);
            audioMenu.Update(gameTime, current, previous);
            rubiksMenu.Update(gameTime, current, previous);
            backBtn.Update(current, previous);
            resetBtn.Update(current, previous);
            applyBtn.Update(current, previous);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            settingsMenu.Draw(spriteBatch);
            categories.Draw(spriteBatch);
            backBtn.Draw(spriteBatch);
            resetBtn.Draw(spriteBatch);
            applyBtn.Draw(spriteBatch);
            spriteBatch.End();
            audioMenu.Draw(spriteBatch);
            rubiksMenu.Draw(spriteBatch);
            graphicsMenu.Draw(spriteBatch);
            controlsMenu.Draw(spriteBatch);
        }


        public void Dispose()
        {
            _content.Dispose();
            if (rubiksMenu != null)
            {
                rubiksMenu.Dispose();
            }
        }
    }
}
