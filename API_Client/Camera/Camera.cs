using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace Cam
{
    public class Camera
    {
        public static Image Shot()
        {
            //Bitmap bitmap = null;
            Image image = null;
            using (ShotForm frm = new ShotForm())
            {
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    image = frm.Image;
                    //bitmap = new Bitmap(image);
                }
            }
            return image;
        }

    }
}
