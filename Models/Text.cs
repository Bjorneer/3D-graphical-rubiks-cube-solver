using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Models
{
    class Text
    {
        #region Members
        public float Scale = 1f;
        public Vector2 Location;
        public string TextMessage;
        public SpriteFont Font;
        public Color Color;
        public float Angle = 0;
        #endregion

        #region Constructors
        public Text(string text, SpriteFont font) :this(text, font, Vector2.Zero)
        { }

        public Text(string text, SpriteFont font, Vector2 location) : this(text, font, location, Color.Black)
        { }

        public Text(string text, SpriteFont font, Vector2 location, Color color)
        {
            this.TextMessage = text;
            this.Font = font;
            this.Location = location;
            this.Color = color;
        }
        #endregion

        #region Methods
        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = Font.MeasureString(TextMessage) / 2;
            spriteBatch.DrawString(Font, TextMessage, Location + origin, Color, Angle, origin, Scale, SpriteEffects.None, 1f);
        }

        public void CenterVertically(Rectangle bounds, float xLocation)
        {
            Vector2 size = Font.MeasureString(TextMessage);
            size *= Scale;
            Location.X = xLocation - size.X / 2;
            Location.Y = bounds.Top + bounds.Height / 2 - size.Y / 2;
        }

        public void CenterHorizontally(Rectangle bounds, float yLocation)
        {
            Vector2 size = Font.MeasureString(TextMessage);
            size *= Scale;
            Location.Y = yLocation - size.Y / 2;
            Location.X = bounds.Left + bounds.Width / 2 - size.X / 2;
        }

        public void Center(Rectangle bounds)
        {
            Vector2 size = Font.MeasureString(TextMessage);
            size *= Scale;
            Location.X = bounds.Left + bounds.Width / 2 - size.X / 2;
            Location.Y = bounds.Top + bounds.Height / 2 - size.Y / 2;
        }

        /// <summary>
        /// Scales the Text to be within a certain area
        /// </summary>
        /// <param name="bounds">The bounds to scale by</param>
        /// <param name="allowGreater">Decides if the text can become greater than the font</param>
        public void ScaleText(Rectangle bounds, bool allowGreater)
        {
            Vector2 size = Font.MeasureString(TextMessage);
            if (allowGreater)
            {
                Scale = Math.Min(bounds.Width / size.X, bounds.Height / size.Y);
            }
            else
            {
                Scale = Math.Min(Math.Min(bounds.Width / size.X, bounds.Height / size.Y), 1);
            }
        }
        #endregion
    }
}
