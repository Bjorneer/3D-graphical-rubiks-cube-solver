using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Models
{
    abstract class Menu : IDisposable
    {
        bool show = true;
        public bool Show
        {
            get
            {
                return show;
            }
            set
            {
                show = value;
                OnShowChanged();
            }
        }

        public event EventHandler ShowChanged;

        protected virtual void OnShowChanged()
        {
            EventHandler e = ShowChanged;
            e?.Invoke(this, EventArgs.Empty);
        }

        protected Vector2 location;
        public Vector2 Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                ReCalculateLocation();
            }
        }

        protected void ReCalculateLocation()
        {
            locationMatrix = Matrix.CreateTranslation(new Vector3(location, 0));
        }

        protected Matrix locationMatrix;

        public Vector2 TransformToMenu(Vector2 vec)
        {
            return Vector2.Transform(vec, Matrix.Invert(locationMatrix));
        }
        public Vector2 TransformFromMenu(Vector2 vec)
        {
            return Vector2.Transform(vec, locationMatrix);
        }

        public abstract void Initialize(ContentManager content, GraphicsDevice graphics);

        public abstract void Update(GameTime gameTime, Input current, Input previous);

        public abstract void Draw(SpriteBatch spriteBatch);

        public virtual void Dispose()
        { }
    }
}
