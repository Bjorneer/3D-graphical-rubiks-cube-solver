using RubiksCube3D.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.IO;
using RubiksCube3D.Options;
using Microsoft.Xna.Framework.Input;

namespace RubiksCube3D.Screens.SettingsScreenMenus
{
    class RubiksCubeMenu : Menu
    {
        ContentManager content;
        GraphicsDevice graphicsDevice;

        MarkableButtonPanel cameraColorOptions;
        MarkableButtonPanel visualColorOptions;

        Text visual;
        Text camera;
        Text rubikscubeTitle;

        WebColorReader clrReader;
        RGBReader rgbReader;

        RubiksCubeSettings settings;

        public RubiksCubeMenu(RubiksCubeSettings settings)
        {
            this.settings = settings;
        }
        
        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            this.content = content;
            graphicsDevice = graphics;
            SpriteFont titleFont = this.content.Load<SpriteFont>("Fonts/ComicSansMS32");
            SpriteFont textFont = this.content.Load<SpriteFont>("Fonts/ComicSansMS18");
            Texture2D box = this.content.Load<Texture2D>("Sprites/VitBlock");

            //
            //Title
            //
            rubikscubeTitle = new Text("Rubiks Cube", titleFont, new Vector2(0, 0));

            //
            //Color Text
            //
            visual = new Text("Visual Colors", textFont, new Vector2(550, 70));
            camera = new Text("Camera Colors", textFont, new Vector2(100, 70));


            //
            //ClrReader
            //
            clrReader = new WebColorReader();
            clrReader.Initialize(content, graphics);
            clrReader.Show = false;
            clrReader.ExitButtonClick += new EventHandler((sender, e) => { clrReader.Show = false; cameraColorOptions.UnmarkAll(); });


            //
            //Camera Colors
            //
            cameraColorOptions = new MarkableButtonPanel();
            for (int i = 0; i < 6; i++)
            {
                OptionsButton btn = new OptionsButton(new Sprite2D(box, new Rectangle(150, 120 + 65 * i, 50, 50), Settings.Instance.RubiksCube.GetCameraColor(i).ToXnaColor()));
                btn.MarkAnimation = new Animation.ButtonAnimation
                    (null, 
                    new Rectangle(150 - 10, 120-10 + 65*i, 70, 70), 
                    null);
                btn.UnMarkAnimation = new Animation.ButtonAnimation
                    (null,
                    new Rectangle(150, 120 + 65 * i, 50, 50),
                    null);
                cameraColorOptions.Add(btn);
            }

            //
            //RGBReader
            //
            rgbReader = new RGBReader();
            rgbReader.Initialize(content, graphics);
            rgbReader.Show = false;
            rgbReader.ExitButtonClick += new EventHandler((sender, e) => { rgbReader.Show = false; visualColorOptions.UnmarkAll(); });

            //
            //Visual Colors
            //
            visualColorOptions = new MarkableButtonPanel();
            for (int i = 0; i < 6; i++)
            {
                OptionsButton btn = new OptionsButton(new Sprite2D(box, new Rectangle(600, 120 + 65 * i, 50, 50), Settings.Instance.RubiksCube.GetVisualColor(i)));
                btn.MarkAnimation = new Animation.ButtonAnimation
                    (null,
                    new Rectangle(600 - 10, 120-10 + 65 * i, 70, 70),
                    null);
                btn.UnMarkAnimation = new Animation.ButtonAnimation
                    (null,
                    new Rectangle(600, 120 + 65 * i, 50, 50),
                    null);
                visualColorOptions.Add(btn);
            }

            //
            //PanelEvents
            //
            clrReader.ColorChanged += On_ColorReaderChanged;
            rgbReader.ColorChanged += On_RGBColorChange;
            for (int i = 0; i < 6; i++)
            {
                cameraColorOptions[i].Marked += On_CameraColorMark;
            }
            for (int i = 0; i < 6; i++)
            {
                visualColorOptions[i].Marked += On_VisualColorMark;
            }
        }

        private void On_RGBColorChange(object sender, EventArgs e)
        {
            int idx = visualColorOptions.GetMarkedIndex();
            visualColorOptions[idx].Color = rgbReader.MarkedColor;
            settings.SetVisualColor(idx, rgbReader.MarkedColor);
        }

        private void On_ColorReaderChanged(object sender, EventArgs e)
        {
            int idx = cameraColorOptions.GetMarkedIndex();
            cameraColorOptions[idx].Color = clrReader.MarkedColor.ToXnaColor();
            settings.SetCameraColor(idx, clrReader.MarkedColor);
        }

        private void On_CameraColorMark(object sender, EventArgs e)
        {
            visualColorOptions.UnmarkAll();
            clrReader.Show = true;
            rgbReader.Show = false;
        }

        private void On_VisualColorMark(object sender, EventArgs e)
        {
            cameraColorOptions.UnmarkAll();
            clrReader.Show = false;
            rgbReader.Show = true;
            rgbReader.MarkedColor = visualColorOptions.GetMarked().Color;
        }

        public override void Update(GameTime gameTime, Input current, Input previous)
        {
            Vector2 mousePos = TransformToMenu(current.Mouse.Position.ToVector2());
            MouseState virtualMouse = new MouseState
                ((int)mousePos.X, (int)mousePos.Y,
                current.Mouse.ScrollWheelValue,
                current.Mouse.LeftButton, current.Mouse.MiddleButton, current.Mouse.RightButton,
                current.Mouse.XButton1, current.Mouse.XButton2);

            if (!rgbReader.AnyBarMoving)
            {
                visualColorOptions.Update(virtualMouse, previous);
                cameraColorOptions.Update(virtualMouse, previous);
            }

            clrReader.Update(gameTime, current, previous);
            rgbReader.Update(gameTime, current, previous);
            if (rgbReader.Show == true)
            {
                UpdateRGBColor();
            }


        }

        private void UpdateRGBColor()
        {
            rgbReader.MarkedColor = visualColorOptions.GetMarked().Color;
            int i = visualColorOptions.GetMarkedIndex();
            if (i > 2)
            {
                rgbReader.Location = new Vector2(965 - 250,  65 + visualColorOptions[i].Bounds.Bottom - 200);
            }
            else
            {
                rgbReader.Location = new Vector2(965 - 250,  visualColorOptions[i].Bounds.Top + 65);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Show)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, locationMatrix);
                visual.Draw(spriteBatch);
                camera.Draw(spriteBatch);
                rubikscubeTitle.Draw(spriteBatch);
                visualColorOptions.Draw(spriteBatch);
                cameraColorOptions.Draw(spriteBatch);
                spriteBatch.End();

                spriteBatch.Begin();
                clrReader.Draw(spriteBatch);
                rgbReader.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        public override void Dispose()
        {
            if (clrReader != null)
            {
                clrReader.Dispose();
            }
            if (rgbReader != null)
            {
                rgbReader.Dispose();
            }
            base.Dispose();
        }
    }
}
