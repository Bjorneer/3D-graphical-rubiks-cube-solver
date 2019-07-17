using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RubiksCube3D.IO;
using RubiksCube3D.Models;
using RubiksCube3D.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Screens.SettingsScreenMenus
{
    class AudioMenu : Menu
    {
        GraphicsDevice graphics;

        Text title;

        Text musicVolumeText;
        SlideBar musicVolumeBar;
        Text musicVolumeValue;
        

        Text soundFXVolumeText;
        SlideBar soundFXVolumeBar;
        Text soundFXVolumeValue;

        AudioSettings settings;

        public AudioMenu(AudioSettings settings)
        {
            this.settings = settings;
        }

        public override void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            this.graphics = graphics;
            SpriteFont textFont = content.Load<SpriteFont>("Fonts/ComicSansMS18");
            SpriteFont titleFont = content.Load<SpriteFont>("Fonts/ComicSansMS32");
            Texture2D barTexture = content.Load<Texture2D>("Sprites/VolumeBar");
            Texture2D markerTexture = content.Load<Texture2D>("Sprites/VolumeMark");


            title = new Text("Audio", titleFont, new Vector2(0, 0));

            musicVolumeText = new Text("Music Volume:", textFont);
            musicVolumeBar = new SlideBar(barTexture, markerTexture, new Rectangle(300, 100, 400, 50), new Rectangle(0, 0, 30, 30), 30);
            musicVolumeText.CenterVertically(musicVolumeBar.Bar.Bounds, 80);
            musicVolumeValue = new Text("", textFont);
            musicVolumeValue.TextMessage = musicVolumeBar.Value.ToString();
            musicVolumeValue.Center(new Rectangle(725, 100, 50, 50));
            musicVolumeBar.ValueChange += On_MusicVolumeChange;

            soundFXVolumeText = new Text("SoundFX Volume:", textFont);
            soundFXVolumeBar = new SlideBar(barTexture, markerTexture, new Rectangle(300, 200, 400, 50), new Rectangle(0, 0, 30, 30), 30);
            soundFXVolumeText.CenterVertically(soundFXVolumeBar.Bar.Bounds, 100);
            soundFXVolumeValue = new Text("", textFont);
            soundFXVolumeValue.TextMessage = soundFXVolumeBar.Value.ToString();
            soundFXVolumeValue.Center(new Rectangle(725, 200, 50, 50));
            soundFXVolumeBar.ValueChange += On_SoundFXVolumeChange;
        }

        private void On_MusicVolumeChange(object sender, EventArgs e)
        {
            musicVolumeValue.TextMessage = musicVolumeBar.Value.ToString();
            musicVolumeValue.Center(new Rectangle(725, 100, 50, 50));
        }

        private void On_SoundFXVolumeChange(object sender, EventArgs e)
        {
            soundFXVolumeValue.TextMessage = soundFXVolumeBar.Value.ToString();
            soundFXVolumeValue.Center(new Rectangle(725, 200, 50, 50));
        }

        public override void Update(GameTime gameTime, Input current, Input previous)
        {
            Vector2 mousePos = TransformToMenu(current.Mouse.Position.ToVector2());
            MouseState virtualMouse = new MouseState
                ((int)mousePos.X, (int)mousePos.Y, 
                current.Mouse.ScrollWheelValue, 
                current.Mouse.LeftButton, current.Mouse.MiddleButton, current.Mouse.RightButton,
                current.Mouse.XButton1, current.Mouse.XButton2);


            musicVolumeBar.Update(virtualMouse, previous);

            soundFXVolumeBar.Update(virtualMouse, previous);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Show == true)
            {
                spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, locationMatrix);
                title.Draw(spriteBatch);
                musicVolumeBar.Draw(spriteBatch);
                soundFXVolumeBar.Draw(spriteBatch);
                musicVolumeText.Draw(spriteBatch);
                soundFXVolumeText.Draw(spriteBatch);
                musicVolumeValue.Draw(spriteBatch);
                soundFXVolumeValue.Draw(spriteBatch);
                spriteBatch.End();
            }
        }
    }
}
