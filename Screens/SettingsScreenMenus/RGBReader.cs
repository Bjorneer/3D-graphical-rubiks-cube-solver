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

namespace RubiksCube3D.Screens.SettingsScreenMenus
{
    class RGBReader
    {
        Sprite2D background;
        SlideBar red;
        SlideBar green;
        SlideBar blue;

        private bool show = true;
        public bool Show
        {
            get
            {
                return show;
            }
            set
            {
                show = value;
            }
        }

        public event EventHandler ExitButtonClick;

        protected virtual void OnExitButtonClick()
        {
            EventHandler e = ExitButtonClick;
            e?.Invoke(this, EventArgs.Empty);
        }
        Button exitBtn;

        Color currentColor;
        public Color MarkedColor
        {
            get
            {
                return currentColor;
            }
            set
            {
                currentColor = value;
                red.Value = currentColor.R;
                blue.Value = currentColor.B;
                green.Value = currentColor.G;
            }
        }

        public bool AnyBarMoving
        {
            get
            {
                return red.IsBarMoving || blue.IsBarMoving || green.IsBarMoving;
            }
        }
        public event EventHandler ColorChanged;

        protected virtual void OnColorChanged()
        {
            EventHandler e = ColorChanged;
            e?.Invoke(this, EventArgs.Empty);
        }

        public Vector2 Location
        {
            set
            {
                background.Bounds = new Rectangle((int)value.X, (int)value.Y, 250, 200);
                red.Bounds = new Rectangle(background.Bounds.X + 25, background.Bounds.Y + 28, 200, 30);
                green.Bounds = new Rectangle(background.Bounds.X + 25, background.Bounds.Y + 28 + 28 + 30, 200, 30);
                blue.Bounds = new Rectangle(background.Bounds.X + 25, background.Bounds.Y + 28 + 28 + 28 + 60, 200, 30);
                exitBtn.Bounds = new Rectangle(background.Bounds.X, background.Bounds.Y, 20, 20);
            }
        }

        Texture2D slideTexture;
        Texture2D markTexture;

        public void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            //Background
            background = new Sprite2D(content.Load<Texture2D>("Sprites/ColorChooserBackground"), new Rectangle(585, 100, 250, 200));

            //Extra
            exitBtn = new Button(new Sprite2D(content.Load<Texture2D>("Sprites/XMark"), new Rectangle(background.Bounds.X, background.Bounds.Y, 20, 20)));
            exitBtn.Click += new EventHandler((sender, e) => { OnExitButtonClick(); });

            //RGB
            slideTexture = content.Load<Texture2D>("Sprites/Colorbar");
            markTexture = content.Load<Texture2D>("Sprites/ColorMarker");
            red = new SlideBar(slideTexture, markTexture, new Rectangle(background.Bounds.X + 25, background.Bounds.Y + 28,200, 30), new Rectangle(0, 0, 10, 30),0, 0);
            green = new SlideBar(slideTexture, markTexture, new Rectangle(background.Bounds.X + 25, background.Bounds.Y + 28 + 28 + 30, 200, 30), new Rectangle(0, 0, 10, 30), 0, 0);
            blue = new SlideBar(slideTexture, markTexture, new Rectangle(background.Bounds.X + 25, background.Bounds.Y + 28 + 28 + 28 + 60, 200, 30), new Rectangle(0, 0, 10, 30), 0, 0);
            red.UpperRange = 255;
            green.UpperRange = 255;
            blue.UpperRange = 255;
            red.ValueChange += On_ValueChange;
            blue.ValueChange += On_ValueChange;
            green.ValueChange += On_ValueChange;
            green.SlideColor = Color.Green;
            red.SlideColor = Color.Red;
            blue.SlideColor = Color.Blue;
        }

        private void On_ValueChange(object sender, EventArgs e)
        {
            currentColor.R = (byte)red.Value;
            currentColor.G = (byte)green.Value;
            currentColor.B = (byte)blue.Value;
            OnColorChanged();
        }

        public void Update(GameTime gameTime, Input current, Input previous)
        {
            if (show)
            {
                exitBtn.Update(current, previous);
                red.Update(current, previous);
                green.Update(current, previous);
                blue.Update(current, previous);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (show)
            {
                background.Draw(spriteBatch);
                exitBtn.Draw(spriteBatch);
                red.Draw(spriteBatch);
                green.Draw(spriteBatch);
                blue.Draw(spriteBatch);
            }
        }

        public void Dispose()
        {

        }
    }
}
