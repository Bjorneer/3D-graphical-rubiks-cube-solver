using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RubiksCube3D.IO;
using RubiksCube3D.Animation;
using Microsoft.Xna.Framework.Input;

namespace RubiksCube3D.Models
{
    enum ButtonCondition
    {
        None,
        Hovered,
        Pressed
    }

    class Button
    {
        #region Members
        public ButtonCondition State { get; protected set; }

        protected Sprite2D sprite;
        public Text TextMessege { get; set; }

        public event EventHandler Click;
        public event EventHandler Hover;
        public event EventHandler UnHover;

        public Color Color
        {
            get{ return sprite.Color; }
            set{ sprite.Color = value;}
        }

        public Rectangle Bounds
        {
            get{ return sprite.Bounds;}
            set{ sprite.Bounds = value;}
        }

        public float Angle
        {
            get { return sprite.Angle; }
            set { sprite.Angle = value; }
        }
        public Texture2D Texture
        {
            set
            {
                sprite.Texture = value;
            }
        }


        public ButtonAnimation HoverAnimation { get; set; }
        public ButtonAnimation ClickAnimation { get; set; }
        public ButtonAnimation UnHoverAnimation { get; set; }
        #endregion

        #region Constructor
        public Button(Sprite2D sprite)
        {
            this.sprite = sprite;
        }
        #endregion

        #region Methods

        public virtual void Update(Input currentInput, Input previousInput)
        {
            if (Contains(currentInput.GetVirtualMouseLocation()))
            {
                if (currentInput.Mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                    previousInput.Mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                    State != ButtonCondition.None)
                {
                    if (ClickAnimation != null) Animate(ClickAnimation);
                    State = ButtonCondition.Pressed;
                    OnClick(EventArgs.Empty);
                }
                else if (State != ButtonCondition.Hovered)
                {
                    if (State == ButtonCondition.None)
                    {
                        if (HoverAnimation != null) Animate(HoverAnimation);
                        OnHover(EventArgs.Empty);
                    }
                    State = ButtonCondition.Hovered;
                }

            }
            else if (State != ButtonCondition.None)
            {
                if (UnHoverAnimation != null) Animate(UnHoverAnimation);
                OnUnHover(EventArgs.Empty);
                State = ButtonCondition.None;
            }
        }

        public virtual void Update(MouseState currentInput, Input previousInput)
        {
            if (Contains(currentInput.Position))
            {
                if (currentInput.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Released &&
                    previousInput.Mouse.LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed &&
                    State != ButtonCondition.None)
                {
                    if (ClickAnimation != null) Animate(ClickAnimation);
                    State = ButtonCondition.Pressed;
                    OnClick(EventArgs.Empty);
                }
                else if (State != ButtonCondition.Hovered)
                {
                    if (State == ButtonCondition.None)
                    {
                        if (HoverAnimation != null) Animate(HoverAnimation);
                        OnHover(EventArgs.Empty);
                    }
                    State = ButtonCondition.Hovered;
                }

            }
            else if (State != ButtonCondition.None)
            {
                if (UnHoverAnimation != null) Animate(UnHoverAnimation);
                OnUnHover(EventArgs.Empty);
                State = ButtonCondition.None;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            sprite.Draw(spriteBatch);
            if (TextMessege != null)
            {
                TextMessege.Draw(spriteBatch);
            }
        }

        public void Animate(ButtonAnimation animation)
        {
            if (animation.Angle.HasValue)
                Angle = (float)animation.Angle;
            if (animation.Color.HasValue)
                Color = (Color)animation.Color;
            if (animation.Bounds.HasValue)
                Bounds = (Rectangle)animation.Bounds;
        }

        protected virtual void OnClick(EventArgs e)
        {
            EventHandler handler = Click;
            handler?.Invoke(this, e);
        }

        protected virtual void OnHover(EventArgs e)
        {
            EventHandler handler = Hover;
            handler?.Invoke(this, e);
        }

        protected virtual void OnUnHover(EventArgs e)
        {
            EventHandler handler = UnHover;
            handler?.Invoke(this, e);
        }

        public bool Contains(Vector2 pos)
        {
            return sprite.Contains(pos);
        }

        public bool Contains(Point pos)
        {
            return sprite.Contains(pos);
        }
        #endregion
    }
}
