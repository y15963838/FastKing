using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FastKing
{
    public partial class HalfTransparentChildForm : Form
    {
        Form1 f2;
        public HalfTransparentChildForm(Form1 f)
        {
            InitializeComponent();
            f2 = f;
            

            string skin1 = AppDomain.CurrentDomain.BaseDirectory + "backgroundPic\\01.png";
            string skin2 = AppDomain.CurrentDomain.BaseDirectory + "backgroundPic\\02.png";
            string skin3 = AppDomain.CurrentDomain.BaseDirectory + "backgroundPic\\03.png";
            string skin4 = AppDomain.CurrentDomain.BaseDirectory + "backgroundPic\\04.png";
            comboBox1.Items.AddRange(new string[] { skin1, skin2, skin3, skin4 });
            comboBox1.SelectedItem = skin1;

            this.trackBar1.Scroll += new EventHandler(trackBar1_Scroll);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            UpdateStyles();
        }

        void trackBar1_Scroll(object sender, EventArgs e)
        {
            //设置透明度
            f2.Opacity = (double)trackBar1.Value / 100.0;
        }

        /// <summary>
        /// 边框阴影
        /// </summary>
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 0x20000;
                return createParams;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //设置皮肤
            f2.BackgroundImage = new Bitmap(comboBox1.SelectedItem.ToString());
        }
    }
}
