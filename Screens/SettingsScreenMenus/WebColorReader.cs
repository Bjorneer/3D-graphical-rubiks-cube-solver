using DirectShowLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using RubiksCube3D.IO;
using RubiksCube3D.Models;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RubiksCube3D.Screens.SettingsScreenMenus
{
    class WebColorReader : IDisposable
    {
        //WebCam
        DsDevice[] cameraDevices;
        WebCamera webCam;
        Button takeImageButton;
        Sprite2D cameraIcon;
        Sprite2D background;
        System.Windows.Forms.ComboBox webCamDevices;
        System.Windows.Forms.PictureBox cameraFeedBox;
        System.Windows.Forms.PictureBox stillImageBox;
        System.Drawing.Bitmap originalPicture;
        IntPtr ip = IntPtr.Zero;
        System.Windows.Forms.PictureBox cameraBoxes;

        const int VIDEOWIDTH = 320;
        const int VIDEOHEIGHT = 240;
        const int VIDEOBITSPERPIXEL = 24;


        private bool show = true;
        public bool Show
        {
            get
            {
                return show;
            }
            set
            {
                show = value;
                if (show)
                {
                    if (webCamDevices != null)
                    {
                        webCamDevices.Show();
                    }
                    if (cameraFeedBox != null)
                    {
                        cameraFeedBox.Show();
                    }
                    if (stillImageBox != null)
                    {
                        stillImageBox.Show();
                    }
                }
                else
                {
                    if (webCamDevices != null)
                    {
                        webCamDevices.Hide();
                    }
                    if (cameraFeedBox != null)
                    {
                        cameraFeedBox.Hide();
                    }
                    if (stillImageBox != null)
                    {
                        stillImageBox.Hide();
                    }
                }
            }
        }

        //

        public event EventHandler ExitButtonClick;

        protected virtual void OnExitButtonClick()
        {
            EventHandler e = ExitButtonClick;
            e?.Invoke(this, EventArgs.Empty);
        }
        Button exitBtn;
        System.Drawing.Color currentColor = System.Drawing.Color.White;
        public System.Drawing.Color MarkedColor
        {
            get
            {
                return currentColor;
            }
            private set
            {
                currentColor = value;
                
            }
        }

        public event EventHandler ColorChanged;

        protected virtual void OnColorChanged()
        {
            EventHandler e = ColorChanged;
            e?.Invoke(this, EventArgs.Empty);
        }

        public void Initialize(ContentManager content, GraphicsDevice graphics)
        {
            //WebCamDevices
            webCamDevices = new System.Windows.Forms.ComboBox();
            webCamDevices.Location = new System.Drawing.Point(635, 320);
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
            cameraFeedBox.Location = new System.Drawing.Point(635, 165);
            cameraFeedBox.Name = "CameraFeed";
            cameraFeedBox.Size = new System.Drawing.Size(200, 150);
            cameraFeedBox.TabIndex = 0;
            cameraFeedBox.TabStop = false;
            //StillImage
            stillImageBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            stillImageBox.Location = new System.Drawing.Point(630, 400);
            stillImageBox.Name = "StillImage";
            stillImageBox.Size = new System.Drawing.Size(200, 150);
            stillImageBox.TabIndex = 1;
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
                graph.FillRectangle(b, 0, 0, 200, 150);
                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        graph.DrawRectangle(pen, 25 + 25 - 10 + 50 * j, 25 - 10 + 50 * i, 20, 20);
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

            //CameraButton
            takeImageButton = new Button(new Sprite2D(content.Load<Texture2D>("Sprites/Button"), new Rectangle(635 + 50, 345, 100, 50)));
            cameraIcon = new Sprite2D(content.Load<Texture2D>("Sprites/Camera"), new Rectangle(0, 0, 50, 50));
            cameraIcon.Center(takeImageButton.Bounds);
            takeImageButton.Click += On_TakeImageButtonClick;
            //Background
            background = new Sprite2D(content.Load<Texture2D>("Sprites/ColorChooserBackground"), new Rectangle(585, 100, 300, 525));

            //Extra
            exitBtn = new Button(new Sprite2D(content.Load<Texture2D>("Sprites/XMark"), new Rectangle(865, 100, 20, 20)));
            exitBtn.Click += new EventHandler ((sender, e) => { OnExitButtonClick(); } );

            UpdateCameraDevices();
        }

        private void On_TakeImageButtonClick(object sender, EventArgs e)
        {
            if (webCam != null)
            {
                if (stillImageBox.Image != null)
                {
                    stillImageBox.Image.Dispose();
                }
                if (originalPicture != null)
                {
                    originalPicture.Dispose();
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
                    originalPicture = new System.Drawing.Bitmap(webCam.Width, webCam.Height, webCam.Stride, PixelFormat.Format24bppRgb, ip);
                    originalPicture.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);

                    System.Drawing.Bitmap result = new System.Drawing.Bitmap(200, 150);
                    using (System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(result))
                    {
                        graph.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        graph.DrawImage(originalPicture, 0, 0, 200, 150);
                        System.Drawing.Pen pen = new System.Drawing.Pen(System.Drawing.Color.Purple, 1);
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                graph.DrawRectangle(pen, 25 + 25 - 10 + 50 * j, 25 - 10 + 50 * i, 20, 20);
                            }
                        }
                        pen.Dispose();
                    }
                    stillImageBox.Image = result;
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;

                }
                catch
                {
                    System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(VIDEOWIDTH, VIDEOHEIGHT);
                    using (System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(bitmap))
                    {
                        System.Drawing.Rectangle size = new System.Drawing.Rectangle(0, 0, VIDEOWIDTH, VIDEOHEIGHT);
                        graph.FillRectangle(System.Drawing.Brushes.White, size);
                    }
                    stillImageBox.Image = bitmap;
                    webCam.Dispose();
                    webCam = null;
                }
            }
        }

        private void On_WebDeviceChanged(object sender, EventArgs e)
        {
            try
            {
                if (webCam == null || webCam.Name != (string)webCamDevices.SelectedItem)
                {
                    webCam = new WebCamera(cameraDevices.First(x => x.Name == (string)webCamDevices.SelectedItem), 320, 240, VIDEOBITSPERPIXEL, cameraFeedBox);
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

        float cameraDeviceUpdateTimer = 0;
        float cameraDeviceUpdateRate = 10;
        public void Update(GameTime gameTime, Input current, Input previous)
        {
            if (show)
            {
                cameraDeviceUpdateTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;

                takeImageButton.Update(current, previous);
                exitBtn.Update(current, previous);

                if (current.Mouse.LeftButton == ButtonState.Released && previous.Mouse.LeftButton == ButtonState.Pressed && ContainsImageBox(current.Mouse.Position.ToVector2(), out int idx))
                {
                    if (originalPicture != null)
                    {
                        MarkedColor = originalPicture.GetAverageColor((webCam.Width - webCam.Height)/2 + webCam.Height / 6 + (idx % 3) * webCam.Height / 3, webCam.Height / 6 + (idx / 3) * webCam.Height / 3, webCam.Height / 12);
                        OnColorChanged();
                    }
                }

                if (cameraDeviceUpdateTimer > cameraDeviceUpdateRate)
                {
                    cameraDeviceUpdateTimer = 0;
                    UpdateCameraDevices();
                }
            }
        }

        private bool ContainsImageBox(Vector2 pos, out int boxnum)
        {
            boxnum = 0;
            System.Drawing.Point p = stillImageBox.Location;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (new Rectangle(p.X + 50 -10+ j*50, p.Y + 25 -10 + (i * 50), 20, 20).Contains(pos))
                    {
                        return true;
                    }
                    boxnum++;
                }
            }
            return false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (show)
            {
                background.Draw(spriteBatch);
                takeImageButton.Draw(spriteBatch);
                cameraIcon.Draw(spriteBatch);
                exitBtn.Draw(spriteBatch);
            }
        }

        public void Dispose()
        {
            if (webCam != null)
            {
                webCam.Dispose();
            }
            if (cameraFeedBox != null)
            {
                cameraFeedBox.Dispose();
            }
            if (stillImageBox != null)
            {
                stillImageBox.Dispose();
            }
            if (originalPicture != null)
            {
                originalPicture.Dispose();
            }
            if (webCamDevices != null)
            {
                webCamDevices.Dispose();
            }
        }
    }
}
