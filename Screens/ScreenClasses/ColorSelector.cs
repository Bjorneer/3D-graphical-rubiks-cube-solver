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
    class ColorSelector
    {
        Sprite2D colorSelectorBackground;
        Button[] colorSelection;

        public bool Show { get; set; } = true;

        Color _pressedClr;
        public Color PressedColor
        {
            get
            {
                return _pressedClr;
            }
        }

        public ColorSelector(ContentManager content, Color[] representedColors)
        {
            colorSelectorBackground = new Sprite2D(content.Load<Texture2D>("Sprites/ColorSelector"), new Rectangle(535, 445, 200, 265));
            colorSelection = new Button[6];
            for (int i = 0; i < 6; i++)
            {
                int j = i / 3;
                colorSelection[i] = new Button(new Sprite2D(content.Load<Texture2D>("Sprites/VitBlock"), new Rectangle(535 + 25 + j * 100, 445 + 29 + (i % 3) * 79, 50, 50), representedColors[i]));
                colorSelection[i].Click += new EventHandler
                    ((s, e) =>
                    {
                        Button b = (Button)s;
                        _pressedClr = b.Color;
                        OnColorPressed();
                    });
            }
            _pressedClr = Color.White;
        }

        public event EventHandler ColorPressed;

        protected void OnColorPressed()
        {
            EventHandler e = ColorPressed;
            e?.Invoke(this, EventArgs.Empty);
        }

        public void Update(Input cur, Input prev)
        {
            if (Show)
            {
                for (int i = 0; i < 6; i++)
                {
                    colorSelection[i].Update(cur, prev);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Show)
            {
                colorSelectorBackground.Draw(spriteBatch);
                for (int i = 0; i < colorSelection.Length; i++)
                {
                    colorSelection[i].Draw(spriteBatch);
                }
            }
        }
    }
}
