using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RubiksCube3D.IO;
using RubiksCube3D.Managers;
using RubiksCube3D.Rubiks;
using RubiksCube3D.Models;
using System;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using RubiksCube3D.Interfaces;
using DirectShowLib;
using System.Linq;
using RubiksCube3D.Options;
using RubiksCube3D.Screens.ScreenClasses;

namespace RubiksCube3D.Screens
{
    class RubiksReaderScreen : IScreen
    {
        bool readFinished;
        Button solveButton;

        ScreenManager screenManager;
        GraphicsDevice GraphicsDevice;

        private ContentManager _content;

        Camera cubeCamera;
        RubiksCube cube;
        RubiksColorReader rubiksColorReader;

        MarkableButtonPanel rubiksReadedColors;
        Button confirmReadedColors;
        Color[,] readedColors;

        CubeSide currentSideToSet;
        float targetAngleX = 0;
        float targetAngleY = 0;
        float targetAngleZ = 0;
        float currentAngleX = 0;
        float currentAngleY = 0;
        float currentAngleZ = 0;
        bool setSolvePosition = false;

        ColorSelector colorSelector;

        System.Windows.Forms.ComboBox webCamDevices;
        DsDevice[] cameraDevices;
        Sprite2D webCamBackground;
        WebCamera webCam;
        Button takeImageButton;
        Sprite2D cameraIcon;
        System.Windows.Forms.PictureBox cameraFeedBox;
        System.Windows.Forms.PictureBox stillImageBox;
        System.Windows.Forms.PictureBox cameraBoxes;
        IntPtr ip = IntPtr.Zero;

        Button scrambleButton;
        Color[] representedColors;
        bool scrambleMode = false;

        const int VIDEOWIDTH = 320;
        const int VIDEOHEIGHT = 240;
        const int VIDEOBITSPERPIXEL = 24;

        public void Initialize(ScreenManager screenManager)
        {
            this.screenManager = screenManager;
            this.GraphicsDevice = screenManager.GraphicsDevice;
            _content = new ContentManager(screenManager.ServiceProvider, Constants.CONTENT_DIRECTORY);

            //
            //Cube
            //
            cubeCamera = new Camera(GraphicsDevice.Viewport.Width / (float)GraphicsDevice.Viewport.Height, new Vector3(30, 0, 0));
            cubeCamera.Zoom = 100;
            cube = new RubiksCube(30, new Vector3(0, 0, 0), GraphicsDevice);
            cube.SetDefaultColor();
            Settings.Instance.RubiksCube.GetCameraColor(0);
            System.Drawing.Color[] readerTargetColors = new System.Drawing.Color[6];
            representedColors = new Color[6];
            for (int i = 0; i < 6; i++)
            {
                readerTargetColors[i] = Settings.Instance.RubiksCube.GetCameraColor(i);
                representedColors[i] = Settings.Instance.RubiksCube.GetVisualColor(i);
            }
            rubiksColorReader = new RubiksColorReader(readerTargetColors, representedColors);

            //ColorSelector
            colorSelector = new ColorSelector(_content, representedColors);
            colorSelector.Show = false;
            colorSelector.ColorPressed += On_ColorSelected;

            //Confirm selection of colors
            Texture2D blockTexture = _content.Load<Texture2D>("Sprites/VitBlock");
            rubiksReadedColors = new MarkableButtonPanel();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    OptionsButton b = new OptionsButton(new Sprite2D(blockTexture, new Rectangle(350 + 1 + j * 50, 450 + 1 + i * 50, 48, 48)));
                    b.MarkAnimation = new Animation.ButtonAnimation(null, new Rectangle(350 + j * 50, 450 + i * 50, 50, 50), null);
                    b.UnMarkAnimation = new Animation.ButtonAnimation(null, new Rectangle(350 + 1 + j * 50, 450 + 1 + i * 50, 48, 48), null);
                    b.Click += new EventHandler((s, e) => { colorSelector.Show = true; });
                    rubiksReadedColors.Add(b);
                }
            }
            readedColors = new Color[3,3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    readedColors[i, j] = Color.White;
                }
            }

            confirmReadedColors = new Button(new Sprite2D(_content.Load<Texture2D>("Sprites/Pil"), new Rectangle(350, 610, 150, 85)));
            confirmReadedColors.Click += On_ConfirmColors;

            solveButton = new Button(new Sprite2D(_content.Load<Texture2D>("Sprites/SolveButton"), new Rectangle(970, 480, 260, 200)));
            solveButton.Click += new EventHandler((s, e) => 
            {
                setSolvePosition = true;
                cube.AngleX = cube.AngleX % (float)(Math.PI * 2);
                cube.AngleY = cube.AngleY % (float)(Math.PI * 2);
                cube.AngleZ = cube.AngleZ % (float)(Math.PI * 2);
            });


            //WebCamDevices
            webCamDevices = new System.Windows.Forms.ComboBox();
            webCamDevices.Location = new System.Drawing.Point(80, 280);
            webCamDevices.Size = new System.Drawing.Size(200, 40);
            webCamDevices.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            webCamDevices.Name = "WebCamDevices";
            webCamDevices.AllowDrop = false;
            webCamDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cameraDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            //
            //WebCamera
            //
            cameraFeedBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(cameraFeedBox)).BeginInit();
            stillImageBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(stillImageBox)).BeginInit();
            //CameraFeed
            cameraFeedBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            cameraFeedBox.Location = new System.Drawing.Point(20, 20);
            cameraFeedBox.Name = "CameraFeed";
            cameraFeedBox.Size = new System.Drawing.Size(VIDEOWIDTH, VIDEOHEIGHT);
            cameraFeedBox.TabIndex = 1;
            cameraFeedBox.TabStop = false;
            //StillImage
            stillImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            stillImageBox.Location = new System.Drawing.Point(20, 460);
            stillImageBox.Name = "StillImage";
            stillImageBox.Size = new System.Drawing.Size(320, 240);
            stillImageBox.TabIndex = 2;
            stillImageBox.TabStop = false;
            //Cameraboxes
            cameraBoxes = new System.Windows.Forms.PictureBox();
            cameraBoxes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            cameraBoxes.Location = new System.Drawing.Point(0, 0);
            cameraBoxes.Name = "CameraBox";
            cameraBoxes.Size = new System.Drawing.Size(VIDEOWIDTH, VIDEOHEIGHT);
            cameraBoxes.TabIndex = 0;
            cameraBoxes.BackColor = System.Drawing.Color.Transparent;

            System.Drawing.Bitmap pic = new System.Drawing.Bitmap(VIDEOWIDTH, VIDEOHEIGHT);
            using (System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(pic))
            {
                System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Purple, 1);
                System.Drawing.Brush b = System.Drawing.Brushes.Transparent;
                graph.FillRectangle(b, 0, 0, 320, 240);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        graph.DrawRectangle(pen, 40 + 30 + 80 * j, 30 + 80 * i, 20, 20);
                    }
                }
                pen.Dispose();
            }
            cameraBoxes.Image = pic;

            cameraFeedBox.Controls.Add(cameraBoxes);
            System.Windows.Forms.Control.FromHandle(GameEngine.WindowPtr).Controls.Add(stillImageBox);
            System.Windows.Forms.Control.FromHandle(GameEngine.WindowPtr).Controls.Add(cameraFeedBox);
            System.Windows.Forms.Control.FromHandle(GameEngine.WindowPtr).Controls.Add(webCamDevices);


            webCamDevices.SelectedIndexChanged += On_WebDeviceChanged;

            //WebCameraBackground
            webCamBackground = new Sprite2D(_content.Load<Texture2D>("Sprites/Menu"), new Rectangle(10, 10, 525, 700));

            //CameraButton
            takeImageButton = new Button(new Sprite2D(_content.Load<Texture2D>("Sprites/Button"), new Rectangle(75, 330 ,200,100)));
            cameraIcon = new Sprite2D(_content.Load<Texture2D>("Sprites/Camera"), new Rectangle(0,0,100,100));
            cameraIcon.Center(takeImageButton.Bounds);
            takeImageButton.Click += On_TakeImageButtonClick;

            //Scramble
            scrambleButton = new Button(new Sprite2D(_content.Load<Texture2D>("Sprites/ScrambleButton"), new Rectangle(1060,550,200, 150)));
            scrambleButton.Click += On_ScramblePress;


            UpdateCameraDevices();
        }

        private void On_ScramblePress(object sender, EventArgs e)
        {
            readFinished = true;
            scrambleMode = true;
            System.Windows.Forms.Control.FromHandle(GameEngine.WindowPtr).Controls.Clear();
        }

        private void On_ColorSelected(object sender, EventArgs e)
        {
            colorSelector.Show = false;
            int index = rubiksReadedColors.GetMarkedIndex();
            readedColors[index % 3, index / 3] = colorSelector.PressedColor;
            rubiksReadedColors.SetColor(index, colorSelector.PressedColor);
            rubiksReadedColors.UnmarkAll();
        }

        private void On_ConfirmColors(object sender, EventArgs e)
        {
            inCubeRotation = true;
            targetAnimationTimer = 0;
            Color[,] clrs = readedColors;
            switch (currentSideToSet)
            {
                case CubeSide.Front:
                    clrs = readedColors.FlipHorisontally().RotateClockwise(1);
                    targetAngleY = -MathHelper.PiOver2;
                    break;
                case CubeSide.Right:
                    clrs = readedColors.FlipHorisontally().RotateClockwise(2);
                    targetAngleY = -MathHelper.Pi;
                    break;
                case CubeSide.Top:
                    clrs = readedColors.FlipHorisontally().RotateClockwise(3);
                    targetAngleY = -MathHelper.Pi - MathHelper.PiOver2;
                    break;
                case CubeSide.Left:
                    clrs = readedColors.RotateClockwise(3);
                    targetAngleY = -MathHelper.Pi - MathHelper.Pi;
                    break;
                case CubeSide.Back:
                    clrs = readedColors.RotateClockwise( 1);
                    targetAngleY = -MathHelper.Pi;
                    targetAngleX = MathHelper.PiOver2;
                    break;
                case CubeSide.Bottom:
                    clrs = readedColors.RotateClockwise(3);
                    targetAngleX = 0;
                    break;
                default:
                    throw new Exception("Not a side");
            }
            cube.SetSideColor(currentSideToSet, clrs);
            switch (currentSideToSet)
            {
                case CubeSide.Front:
                    currentSideToSet = CubeSide.Right;
                    break;
                case CubeSide.Right:
                    currentSideToSet = CubeSide.Back;
                    break;
                case CubeSide.Top:
                    currentSideToSet = CubeSide.Left;
                    break;
                case CubeSide.Left:
                    currentSideToSet = CubeSide.Bottom;
                    break;
                case CubeSide.Back:
                    currentSideToSet = CubeSide.Top;
                    break;
                case CubeSide.Bottom:
                    //RotationCompleted
                    readFinished = true;
                    System.Windows.Forms.Control.FromHandle(GameEngine.WindowPtr).Controls.Clear();
                    break;
                default:
                    break;
            }
            colorSelector.Show = false;
            rubiksReadedColors.UnmarkAll();
        }

        private void On_TakeImageButtonClick(object sender, EventArgs e)
        {
            if (webCam != null)
            {
                if (stillImageBox.Image != null)
                {
                    stillImageBox.Image.Dispose();
                }
                try
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    // Release any previous buffer
                    if (ip != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(ip);
                        ip = IntPtr.Zero;
                    }
                    // capture image
                    ip = webCam.Click();
                    System.Drawing.Bitmap b = new System.Drawing.Bitmap(webCam.Width, webCam.Height, webCam.Stride, PixelFormat.Format24bppRgb, ip);
                    b.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
                    readedColors = rubiksColorReader.ReadRubiksSide(b, new Vector2((webCam.Width - webCam.Height)/2 + webCam.Height / 6, webCam.Height /6), webCam.Height / 3, webCam.Height/12);

                    System.Drawing.Bitmap res = new System.Drawing.Bitmap(VIDEOWIDTH, VIDEOHEIGHT);

                    using (System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(res))
                    {
                        System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Purple, 1);
                        graph.DrawImage(b, 0, 0, 320, 240);
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                graph.DrawRectangle(pen, 40+30 + 80*j, 30 + 80*i, 20, 20);
                            }
                        }
                        pen.Dispose();
                    }
                    b.Dispose();
                    stillImageBox.Image = res;
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                }
                catch
                {
                    readedColors = new Color[3, 3];
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            readedColors[i, j] = Color.White;
                        }
                    }
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(VIDEOWIDTH, VIDEOHEIGHT);
                    using (System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        System.Drawing.Rectangle size = new System.Drawing.Rectangle(0, 0, VIDEOWIDTH, VIDEOHEIGHT);
                        graph.FillRectangle(System.Drawing.Brushes.White, size);
                    }
                    stillImageBox.Image = bitmap;
                    webCam.Dispose();
                    webCam = null;
                    UpdateCameraDevices();
                }
                finally
                {
                    int k = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {
                            rubiksReadedColors.SetColor(k, readedColors[j, i]);
                            k++;
                        }
                    }
                }
            }
        }

        private void On_WebDeviceChanged(object sender, EventArgs e)
        {
            try
            {
                if (webCam == null || webCam.Name != (string)webCamDevices.SelectedItem)
                {
                    webCam = new WebCamera(cameraDevices.First(x => x.Name == (string)webCamDevices.SelectedItem), VIDEOWIDTH, VIDEOHEIGHT, VIDEOBITSPERPIXEL, cameraFeedBox);
                }
            }
            catch
            {
                if (webCam != null)
                {
                    webCam.Dispose();
                }
                webCam = null;
            }
        }

        float cameraDeviceUpdateTimer = 0;
        const float cameraDevicesUpdateRate = 10; //In seconds

        const float targetAnimationRate = 1;
        float targetAnimationTimer = 0;
        bool inCubeRotation = false;

        float scrambleRate = 0.05f;
        float scrambleTimer = 0;
        public void Update(GameTime gameTime, Input current, Input previous)
        {
            cameraDeviceUpdateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!readFinished)
            {
                if (inCubeRotation)
                {
                    targetAnimationTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    UpdateCubeRotation();
                }
                else
                {
                    confirmReadedColors.Update(current, previous);
                }
                if (cameraDeviceUpdateTimer > cameraDevicesUpdateRate)
                {
                    cameraDeviceUpdateTimer = 0;
                    UpdateCameraDevices();
                }
                scrambleButton.Update(current, previous);
                colorSelector.Update(current, previous);
                rubiksReadedColors.Update(current, previous);
                takeImageButton.Update(current, previous);
            }
            else
            {
                if (scrambleMode)
                {
                    scrambleTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (scrambleRate <= scrambleTimer)
                    {
                        scrambleTimer = 0;
                        cube.Scramble(representedColors);
                        scrambleRate *= 1.03f;
                    }
                }
                if (setSolvePosition)
                {
                    cube.AngleX = (Math.Abs(cube.AngleX) < 0.01f ? 0 : cube.AngleX + (cube.AngleX < 0 ? 0.01f : -0.01f));
                    cube.AngleY = (Math.Abs(cube.AngleY) < 0.01f ? 0 : cube.AngleY + (cube.AngleY < 0 ? 0.01f : -0.01f));
                    cube.AngleZ = (Math.Abs(cube.AngleZ) < 0.01f ? 0 : cube.AngleZ + (cube.AngleZ < 0 ? 0.01f : -0.01f));
                    if (cube.AngleX == 0 && cube.AngleY == 0 && cube.AngleZ == 0)
                    {
                        screenManager.PopScreen();
                        screenManager.PushScreen(new RubiksSolverScreen(cube));
                    }
                }
                else
                {
                    UpdateFinishedCube(gameTime, current, previous);
                }
            }
        }

        float moveTimeRate = 5;
        float moveTimeTimer = 0;
        private void UpdateFinishedCube(GameTime gameTime, Input cur, Input prev)
        {
            //Apply cube rotation animation
            cube.AngleX = cube.AngleX + 0.02f;
            cube.AngleY = cube.AngleY + 0.005f;
            cube.AngleZ = cube.AngleZ + 0.001f;

            if (moveTimeTimer <= moveTimeRate)
            {
                moveTimeTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                float currentMove = -30 / moveTimeRate / 60;
                Vector3 cubePos = cube.Position;
                cubePos.X -= currentMove;
                cube.Position = cubePos;
            }
            else
            {
                scrambleMode = false;
                solveButton.Update(cur, prev);
            }
        }

        private void UpdateCubeRotation()
        {
            float animationSpeedX = (targetAngleX - currentAngleX) / targetAnimationRate / 60;
            float animationSpeedY = (targetAngleY - currentAngleY) / targetAnimationRate / 60;
            float animationSpeedZ = (targetAngleZ - currentAngleZ) / targetAnimationRate / 60;
            cube.AngleX = cube.AngleX + animationSpeedX;
            cube.AngleY = cube.AngleY + animationSpeedY;
            cube.AngleZ = cube.AngleZ + animationSpeedZ;
            if (targetAnimationTimer >= targetAnimationRate)
            {
                currentAngleX = targetAngleX;
                currentAngleY = targetAngleY;
                currentAngleZ = targetAngleZ;
                inCubeRotation = false;
            }
        }

        private void UpdateCameraDevices()
        {
            string markedDevice = (string)webCamDevices.SelectedItem;
            cameraDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            webCamDevices.Items.Clear();
            for (int i = 0; i < cameraDevices.Length; i++)
            {
                webCamDevices.Items.Add(cameraDevices[i].Name);
                if (cameraDevices[i].Name == markedDevice)
                {
                    webCamDevices.SelectedIndex = i;
                }
            }
            if (webCamDevices.Items.Count > 0 && webCamDevices.SelectedItem == null)
            {
                webCamDevices.SelectedIndex = 0;
            }
            else
            {
                On_WebDeviceChanged(this, EventArgs.Empty);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!readFinished)
            {
                //2D
                spriteBatch.Begin();
                webCamBackground.Draw(spriteBatch);
                takeImageButton.Draw(spriteBatch);
                cameraIcon.Draw(spriteBatch);
                confirmReadedColors.Draw(spriteBatch);
                rubiksReadedColors.Draw(spriteBatch);
                colorSelector.Draw(spriteBatch);
                scrambleButton.Draw(spriteBatch);
                spriteBatch.End();
            }
            else
            {
                spriteBatch.Begin();
                if (moveTimeTimer >= moveTimeRate && setSolvePosition == false)
                {
                    solveButton.Draw(spriteBatch);
                }
                spriteBatch.End();
            }
            
            //3D
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            cube.Draw(cubeCamera);
        }

        public void Dispose()
        {
            _content.Dispose();
            if (webCam != null)
            {
                webCam.Dispose();
            }
            if (ip != IntPtr.Zero)
            {
                Marshal.FreeCoTaskMem(ip);
                ip = IntPtr.Zero;
            }
            System.Windows.Forms.Control.FromHandle(GameEngine.WindowPtr).Controls.Clear();
            stillImageBox.Dispose();
            cameraFeedBox.Dispose();
            webCamDevices.Dispose();
        }
    }
}
