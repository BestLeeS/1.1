using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using AForge.Video.DirectShow;

namespace Cam
{
    public partial class ShotForm : Form
    {
        #region 窗口拖动API
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();
        #endregion
        //VideoSourcePlayer player=new VideoSourcePlayer
        public Image Image
        {
            get
            {
                return imageBox.GetSelectedImage();
            }
        }
        public ShotForm()
        {
            InitializeComponent();
            btnShot.Text = "拍照";
            imageBox.Visible = false;
            videoSourcePlayer.Visible = true;
            btnOK.Enabled = false;
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            var info = videoDevices[0];//选取第一个,此处可作灵活改动
            VideoCaptureDevice videoSource = new VideoCaptureDevice(info.MonikerString);
            videoSourcePlayer.VideoSource = videoSource;
            videoSourcePlayer.Start();
        }

        private void btnShot_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnShot.Text == "拍照")
                {
                    Bitmap bitMap = ImageHelper.ResizeImage(videoSourcePlayer.GetCurrentVideoFrame(), imageBox.Width, imageBox.Height);
                    imageBox.Image = bitMap;
                    //拍照完成后关摄像头并刷新同时关窗体
                    //videoSourcePlayer.SignalToStop();
                    //videoSourcePlayer.WaitForStop();

                    videoSourcePlayer.Visible = false;
                    //358X441   
                    //Rectangle rec = new Rectangle(100, 20, 119, 147);
                    Rectangle rec = new Rectangle(100, 20, 102, 126);
                    imageBox.SelectedRectangle = rec;
                    imageBox.Visible = true;
                    btnShot.Text = "重拍";
                    //btnOK.Visible = true;
                    btnOK.Enabled = true;
                }
                else
                {
                    imageBox.Visible = false;
                    videoSourcePlayer.Visible = true;
                    btnShot.Text = "拍照";
                    btnOK.Enabled = false;
                }
                // this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("摄像头异常：" + ex.Message);
            }
        }
        private void ShotForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoSourcePlayer != null && videoSourcePlayer.IsRunning)
            {
                videoSourcePlayer.SignalToStop();
                videoSourcePlayer.WaitForStop();
            }
        }
        private void btnSelect_Click(object sender, EventArgs e)
        {
            Image image = imageBox.GetSelectedImage();
            btnOK.Enabled = true;
        }
        private void btnOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK; 
        }

        private void ShotForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Y < 40)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }
    }
}
