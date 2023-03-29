using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace FastKing
{
    public partial class Form1 : Form
    {
        private HalfTransparentChildForm childForm;//此为副窗体

        HandwareMonitor monitor;
        ConsoleManager cm;
        OpenAI openAI;

        public Form1()
        {
            InitializeComponent();
            this.Opacity = 1; // 窗体透明度              
            this.childForm = new HalfTransparentChildForm(this);
            this.childForm.Owner = this;    // 这支所属窗体              
            this.childForm.Dock = DockStyle.Fill;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            UpdateStyles();

            //定位
            this.StartPosition = FormStartPosition.Manual;
            int xWidth = SystemInformation.PrimaryMonitorSize.Width;//获取显示器屏幕宽度
            int yHeight = SystemInformation.PrimaryMonitorSize.Height;//高度
            this.Location = new Point(xWidth - 250, yHeight - 500);
            
            this.ShowInTaskbar = false;
            this.Text = "FastKing";
            this.AutoScaleMode = AutoScaleMode.None;
            this.Load += new EventHandler(Form1_Load);
            this.Activated += new EventHandler(OnActivate);
            this.Deactivate += new EventHandler(OnDeactivate);
        }

        #region 自定义事件

        private void Form1_Load(object sender, EventArgs e)
        {
            //UDP
            //var udpInfo = LocalConfig.GetUDP();
            //if (udpInfo != null)
            //{
            //    UDPManager udp = new UDPManager(udpInfo.ip, 9002, udpInfo.ip, 9001);
            //    udp.onReceiveMsg += (msg) => CMDHelper.ResolveMessage(msg);
            //    udp.StartServer();
            //    //MessageBox.Show($"开启UDP成功，IP：{udpInfo.ip}:{udpInfo.port}");  //防止干扰，仅失败时提示
            //}
            //else
            //{
            //    MessageBox.Show($"开启UDP失败!");
            //}


            //TimeCheck
            //TimeCheck timeCheck = new TimeCheck(3,23,45);


            //HandwareMonitor
            monitor = new HandwareMonitor();
            timer1.Interval = 500;
            timer1.Start();
            timer1.Tick += (e1, e2) => {
                OnMonitorUpdate();
            };
            NetMonitor.Start();


            //ConsoleManager
            cm = new ConsoleManager();
            cm.OnDebug += (str, color) =>
            {
                ResultBox.Text = str;
                ResultBox.ForeColor = color;
            };

            //InputBox.ImeChange += (s1, s2) => { MessageBox.Show(InputBox.ImeMode.ToString()); };

            //TextBox
            InputBox.Text = "Enter text...";
            InputBox.ForeColor = Color.Gray;
            InputBox.Font = new Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));


            InputBox.GotFocus += (s1, s2) =>
            {
                InputBox.Text = "";
                InputBox.ForeColor = Color.Black;
                InputBox.Font = new Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                cm.ShowMode();
            };
            InputBox.LostFocus += (s1, s2) =>
            {
                InputBox.Text = "Enter text...";
                InputBox.ForeColor = Color.Gray;
                InputBox.Font = new Font("Times New Roman", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));               
            };
            InputBox.KeyDown += (s1, s2) => {
                if (s2.KeyCode == Keys.Enter)
                {
                    cm.transMode = IsContainsZH(InputBox.Text) ? "en" : "zh";
                    cm.PLL(InputBox.Text);
                }
                else if (s2.KeyCode == Keys.Up)
                {
                    //重现上一输入
                    if (!string.IsNullOrEmpty(cm.lastStr))
                    {
                        InputBox.Text = cm.lastStr;
                        InputBox.Select(InputBox.Text.Length, 0);  //将光标置于 TextBox 控件的内容的末尾
                    }
                }
                else if (s2.KeyCode == Keys.Down)
                {
                    
                }
            };
            

            ResultBox.ForeColor = Color.Green;
            ResultBox.Text = "";
            ResultBox.Font = new Font("Times New Roman", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            ResultBox.SelectionStart = 0;
            ResultBox.SelectionLength = 0;
            Win32.HideCaret(ResultBox.Handle);
            ResultBox.MouseDown += (s1, s2) => {
                Win32.HideCaret(((TextBoxBase)s1).Handle);
            };
        }

        protected void OnActivate(object sender, EventArgs e)
        {
            SetVisible(false);
            ResultBox.Text = "";

            InputBox.Focus();
        }

        protected void OnDeactivate(object sender, EventArgs e)
        {
            SetVisible(true);
        }

        private bool IsContainsZH(string text)
        {
            for (int i = 0; i < text.Length; i++)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(text[i].ToString(), @"[\u4e00-\u9fbb]+"))
                {
                    return true;
                }
            }

            return false;
        }

        private void SetVisible(bool b)
        {
            CPU_Load_Text.Visible = b;
            GPU_Load_Text.Visible = b;
            Memory_Text.Visible = b;
            CPU_Temp.Visible = b;
            GPU_Temp.Visible = b;
            downloadSpeed.Visible = b;
            uploadSpeed.Visible = b;
            pictureBox1.Visible = b;
            pictureBox2.Visible = b;
            pictureBox3.Visible = b;
            pictureBox4.Visible = b;
            pictureBox5.Visible = b;
            InputBox.Visible = !b;
            ResultBox.Visible = !b;
        }

        private void OnMonitorUpdate()
        {
            monitor.UpdateValue();
          
            CPU_Load_Text.Text = $"{monitor.CPU_Load:0.00}%";
            GPU_Load_Text.Text = $"{monitor.GPU_Load:0.00}%";
            Memory_Text.Text = $"{monitor.Memory_Load:0.00}%";

            if(monitor.CPU_Temp == null) CPU_Temp.Text = "00°C";
            else CPU_Temp.Text = $"{monitor.CPU_Temp:0}°C";

            GPU_Temp.Text = $"{monitor.GPU_Temp:0}°C";
            
            downloadSpeed.Text = NetHS(NetMonitor.downloadSpeed);
            uploadSpeed.Text = NetHS(NetMonitor.uploadSpeed);
        }

        string NetHS(float speed)
        {
            if (speed >= 10000)  // >10MB
            {
                return $"{speed/1000:0.0} MB/S";
            }
            else if (speed >= 1000)  //1MB~10MB
            {
                return $"{speed/1000:0.00} MB/S";
            }
            else if (speed < 1000 && speed > 10)
            {
                return $"{speed} KB/S";
            }
            else if (speed < 10 && speed > 0)
            {
                return $"{speed}.0 KB/S";
            }
            else
            {
                return $"0.0 KB/S";
            }
        }

        #endregion



        #region 继承方法

        /// <summary>
        /// 禁止拖动窗体
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0X00A1 && m.WParam.ToInt32() == 2)
            {
                return;
            }
            if (m.Msg == 0xA3)
            {
                return;
            }
            base.WndProc(ref m);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
           
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            int Rgn = Win32.CreateRoundRectRgn(3, 3, this.Width - 1, this.Height - 1, 5, 5);
            Win32.SetWindowRgn(this.Handle, Rgn, true); if (this.childForm != null)
                this.childForm.Size = new Size(this.Size.Width - 17, this.Height - 39);
        }

        //private const int HTCLIENT = 0x1;
        //private const int HTCAPTION = 0x2;
        //private const int WM_NCHITTEST = 0x0084;
        ////实现移动主窗体，并可放大，缩小
        //protected override void WndProc(ref Message message)
        //{
        //    base.WndProc(ref message);

        //    if (message.Msg == WM_NCHITTEST && (int)message.Result == HTCLIENT)
        //    {
        //        message.Result = (IntPtr)HTCAPTION;
        //    }
        //}

        //窗体边框阴影化
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ClassStyle |= 0x20000;
                return createParams;
            }
        }

        #endregion


        #region 控件事件

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        #endregion



        


        


        //副窗体随主窗体变化
        private void Form2_Resize(object sender, EventArgs e)
        {
            if (this.childForm != null)
                this.childForm.Size = new Size(this.Size.Width - 17, this.Height - 39);
        }


        //副窗体随主窗体位置移动
        private void Form2_LocationChanged(object sender, EventArgs e)
        {
            if (this.childForm != null)
                childForm.Location = new Point(this.Location.X + 8, this.Location.Y + 29);
        }


        //窗体关闭，及相关线程也关闭
        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
            Process pro = Process.GetCurrentProcess();
            pro.Kill();
        }


        //转到设置界面
        private void ToSetting(object sender, EventArgs e)
        {
            this.childForm.Show();
            this.childForm.BringToFront();
            //childForm.Location = new Point(this.Location.X + 8, this.Location.Y + 29);
            //this.childForm.Size = new Size(this.Size.Width - 17, this.Height - 39);

            childForm.Location = new Point(this.Location.X, this.Location.Y);
            this.childForm.Size = new Size(this.Size.Width, this.Height);
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void InputBox_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
