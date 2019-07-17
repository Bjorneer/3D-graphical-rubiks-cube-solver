using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RubiksCube3D.IO;
using RubiksCube3D.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Interfaces
{
    interface IScreen : IDisposable
    {
        void Initialize(ScreenManager screenManager);

        void Draw(GameTime gameTime, SpriteBatch spriteBatch);

        void Update(GameTime gameTime, Input current, Input previous);
    }
}
