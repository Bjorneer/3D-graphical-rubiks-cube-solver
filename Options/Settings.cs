
using Microsoft.Xna.Framework;
using System.IO;

namespace RubiksCube3D.Options
{
    class Settings
    {
        public AudioSettings Audio { get; set; }
        public GraphicsSettings Graphics { get; set; }
        public ControlsSettings Controls { get; set; }
        public RubiksCubeSettings RubiksCube { get; set; }

        private GraphicsDeviceManager graphicsDevice;

        private static Settings instance;

        private Settings()
        {

        }

        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                }
                return instance;
            }
        }

        public void Initialize(GraphicsDeviceManager graphics)
        {
            graphicsDevice = graphics;
            string filePath = Constants.SETTINGS_FILE;

            Audio = new AudioSettings();
            Graphics = new GraphicsSettings();
            Controls = new ControlsSettings();
            RubiksCube = new RubiksCubeSettings();


            if (File.Exists(filePath))
            {
                StreamReader stream = new StreamReader(filePath, true);
                try
                {
                    LoadOptions(stream);
                }
                catch
                {
                    System.Windows.Forms.MessageBox.Show(
                        $"Error occured while loading from file '{filePath}', reseting file.",
                        "Error",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error);
                    ResetOptions();
                }
                finally
                {
                    stream.Close();
                    stream.Dispose();
                }
            }
            Save();
            ApplyChanges();
        }

        private void LoadOptions(StreamReader stream)
        {
            Graphics.Load(stream);
            Audio.Load(stream);
            Controls.Load(stream);
            RubiksCube.Load(stream);
        }

        public void ResetOptions()
        {
            Audio = new AudioSettings();
            Graphics = new GraphicsSettings();
            Controls = new ControlsSettings();
            RubiksCube = new RubiksCubeSettings();
        }

        public void Save()
        {
            string filePath = Constants.SETTINGS_FILE;

            if (File.Exists(filePath))
            {
                FileStream clearedFile = File.Open(filePath, FileMode.Open);
                clearedFile.SetLength(0);
                clearedFile.Close();
            }

            StreamWriter stream = new StreamWriter(filePath, true);

            Graphics.Save(stream);
            Audio.Save(stream);
            Controls.Save(stream);
            RubiksCube.Save(stream);

            stream.Close();
            stream.Dispose();
        }

        public void ApplyChanges()
        {
            Save();
            graphicsDevice.PreferredBackBufferWidth = Graphics.Resolution.Width;
            graphicsDevice.PreferredBackBufferHeight = Graphics.Resolution.Height;
            graphicsDevice.ApplyChanges();
        }
    }
}
