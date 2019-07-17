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
    class SolutionViewer
    {
        private int _currentMove;
        private string[] _solution;
        public void Increment()
        {
            if (_currentMove < _solution.Length - 1)
            {
                _currentMove++;
                RecalibrateText();
                SetMoveMarker();
            }
        }

        public void Decrement()
        {
            if (_currentMove > 0)
            {
                _currentMove--;
                RecalibrateText();
                SetMoveMarker();
            }
        }

        public int CurrentMove
        {
            get
            {
                return _currentMove;
            }
            set
            {
                if (value < 0 || value >= _solution.Length)
                {
                    throw new ArgumentOutOfRangeException();
                }
                _currentMove = value;
                RecalibrateText();
                SetMoveMarker();
            }
        }

        Sprite2D background;
        SpriteFont _font;

        Text ofTotalMoves;
        private void RecalibrateText()
        {
            ofTotalMoves.TextMessage = (_currentMove + 1) +  " of " + (_solution.Length - 1);
            ofTotalMoves.CenterHorizontally(background.Bounds, background.Bounds.Bottom - 10);
        }

        Sprite2D marker;
        private void SetMoveMarker()
        {
            Vector2 sizeOfMarked = _font.MeasureString(_solution[_currentMove]);
            marker.Bounds = new Rectangle(positions[_currentMove].ToPoint(), sizeOfMarked.ToPoint());
        }

        Vector2[] positions = new Vector2[400];

        public SolutionViewer(string[] solution, ContentManager content)
        {
            this._solution = new string[solution.Length + 1];
            for (int i = 0; i < solution.Length; i++)
            {
                _solution[i] = solution[i];
            }
            _solution[_solution.Length - 1] = "Finish";
            this._currentMove = 0;
            _font = content.Load<SpriteFont>("Fonts/Arial10");
            CalibrateLocations();
            background = new Sprite2D(content.Load<Texture2D>("Sprites/SolutionBack"), new Rectangle(20, 20, 400, 300));
            ofTotalMoves = new Text("", _font);
            RecalibrateText();

            marker = new Sprite2D(content.Load<Texture2D>("Sprites/VitBlock"));
            marker.Color = Color.Yellow;
            SetMoveMarker();
        }

        private void CalibrateLocations()
        {
            int k = 0;
            for (int i = 0; i < 16; i++)
            {
                for (int j = 0; j < 19; j++, k++)
                {
                    positions[k] = new Vector2(30 + j * 20, 30 + i * 20);
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            background.Draw(spriteBatch);
            marker.Draw(spriteBatch);
            for (int i = 0; i < _solution.Length; i++)
            {
                spriteBatch.DrawString(_font, _solution[i], positions[i], Color.Black);
            }
            ofTotalMoves.Draw(spriteBatch);

        }

        public void Update(GameTime gameTime, Input current, Input previous)
        {

        }
    }
}
