using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RubiksCube3D.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Models
{
    class SlideBar
    {
        public Color SlideColor { get; set; } = Color.White;

        public Sprite2D Bar;
        public Sprite2D Marker;

        int offset;
        public bool IsBarMoving { get; private set; }

        private float _value;
        private int _decimals;

        private int _upperRange;
        private int _lowerRange;

        public event EventHandler ValueChange;

        public Rectangle Bounds
        {
            get
            {
                return Bar.Bounds;
            }
            set
            {
                Bar.Bounds = value;
                Marker.Bounds = new Rectangle(
            (int)Bar.Location.X + Bar.Width / 2 + -Marker.Bounds.Width / 2,
            (int)Bar.Location.Y + Bar.Height / 2 - Marker.Bounds.Height / 2,
            Marker.Bounds.Width,
            Marker.Bounds.Height);
                Marker.Location = CalculateLocation();
            }
        }

        public float Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = (float)Math.Round(value, _decimals);
                Marker.Location = CalculateLocation();
            }
        }

        public int UpperRange
        {
            get => _upperRange;
            set
            {
                _upperRange = value;

                Value = CalculateValue();
            }
        }

        public int LowerRange
        {
            get => _lowerRange;
            set
            {
                _lowerRange = value;

                Value = CalculateValue();
            }
        }

        private float CalculateValue()
        {
            return ((Marker.Location.X - Bar.Location.X - offset + Marker.Width / 2) / (Bar.Width - 2 * offset) * (_upperRange - _lowerRange)) + _lowerRange;
        }

        private Vector2 CalculateLocation()
        {
            return new Vector2(
                Bar.Location.X + offset + (Bar.Width - 2 * offset) * (_value - _lowerRange) / (_upperRange - _lowerRange) - Marker.Width / 2
                , Marker.Location.Y);
        }

        public SlideBar(
            Texture2D barTexture,
            Texture2D markerTexture,
            Rectangle barRectangle,
            Rectangle markerRectangle,
            int barOffset,
            int decimals = 2)
        {
            offset = barOffset;
            _upperRange = 100;
            _lowerRange = 0;
            _decimals = decimals;

            Bar = new Sprite2D(barTexture)
            {
                Bounds = barRectangle
            };


            Marker = new Sprite2D(markerTexture)
            {
                Bounds = new Rectangle(
                        (int)Bar.Location.X + Bar.Width / 2 + -markerRectangle.Width / 2,
                        (int)Bar.Location.Y + Bar.Height / 2 - markerRectangle.Height / 2,
                        markerRectangle.Width,
                        markerRectangle.Height),
            };

            Value = CalculateValue();
        }

        protected void OnValueChange(EventArgs e)
        {
            EventHandler handler = ValueChange;
            handler?.Invoke(this, e);
        }

        public void Update(Input curInput, Input prevInput)
        {
            Vector2 mouseLocation = curInput.GetVirtualMouseLocation();

            if (curInput.Mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                (IsBarMoving || (Marker.Bounds.Contains(mouseLocation) && prevInput.Mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)))
            {
                IsBarMoving = true;
                Marker.Location = new Vector2(
                    Math.Min(Math.Max(mouseLocation.X - Marker.Width / 2, Bar.Location.X + offset - Marker.Width / 2), Bar.Bounds.Right - offset - Marker.Width / 2),
                    Marker.Location.Y);

                Value = CalculateValue();
                OnValueChange(EventArgs.Empty);
            }
            else
            {
                IsBarMoving = false;
            }
        }

        public void Update(MouseState mouseState, Input prevInput)
        {

            if (mouseState.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                (IsBarMoving || (Marker.Bounds.Contains(mouseState.Position) && prevInput.Mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released)))
            {
                IsBarMoving = true;
                Marker.Location = new Vector2(
                    Math.Min(Math.Max(mouseState.Position.X - Marker.Width / 2, Bar.Location.X + offset - Marker.Width / 2), Bar.Bounds.Right - offset - Marker.Width / 2),
                    Marker.Location.Y);

                Value = CalculateValue();
                OnValueChange(EventArgs.Empty);
            }
            else
            {
                IsBarMoving = false;
            }
        }

        public void Draw(SpriteBatch sb)
        {
            if(SlideColor == Color.White)
            {
                Bar.Draw(sb);
                Marker.Draw(sb);
            }
            else
            {
                float percentToDraw = (Value - LowerRange) / (UpperRange -LowerRange);
                Point size = new Point((int)(Bar.Width * percentToDraw), Bar.Height);
                Point textSize = new Point((int)(Bar.Texture.Width * percentToDraw), Bar.Texture.Height);
                sb.Draw(Bar.Texture, new Rectangle(Bar.Location.ToPoint(), size),new Rectangle(0,0, textSize.X, textSize.Y), SlideColor);
                sb.Draw(Bar.Texture, new Rectangle((int)Bar.Location.X + size.X, (int)Bar.Location.Y, Bar.Width - size.X, Bar.Height), new Rectangle(size.X, 0, Bar.Texture.Width - textSize.X, Bar.Texture.Height), Color.White);
                Marker.Draw(sb);
            }
        }
    }
}
