
namespace FastKing
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.CPU_Load_Text = new System.Windows.Forms.Label();
            this.GPU_Load_Text = new System.Windows.Forms.Label();
            this.Memory_Text = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.InputBox = new System.Windows.Forms.RichTextBox();
            this.ResultBox = new System.Windows.Forms.RichTextBox();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.pictureBox5 = new System.Windows.Forms.PictureBox();
            this.downloadSpeed = new System.Windows.Forms.Label();
            this.uploadSpeed = new System.Windows.Forms.Label();
            this.GPU_Temp = new System.Windows.Forms.Label();
            this.CPU_Temp = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).BeginInit();
            this.SuspendLayout();
            // 
            // CPU_Load_Text
            // 
            this.CPU_Load_Text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.CPU_Load_Text, "CPU_Load_Text");
            this.CPU_Load_Text.ForeColor = System.Drawing.Color.LightCoral;
            this.CPU_Load_Text.Name = "CPU_Load_Text";
            this.CPU_Load_Text.UseMnemonic = false;
            this.CPU_Load_Text.Click += new System.EventHandler(this.label1_Click);
            // 
            // GPU_Load_Text
            // 
            this.GPU_Load_Text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.GPU_Load_Text, "GPU_Load_Text");
            this.GPU_Load_Text.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.GPU_Load_Text.Name = "GPU_Load_Text";
            this.GPU_Load_Text.UseMnemonic = false;
            // 
            // Memory_Text
            // 
            this.Memory_Text.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.Memory_Text, "Memory_Text");
            this.Memory_Text.ForeColor = System.Drawing.Color.Lime;
            this.Memory_Text.Name = "Memory_Text";
            this.Memory_Text.UseMnemonic = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.BackgroundImage = global::FastKing.Properties.Resources.CPU__1_;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.BackgroundImage = global::FastKing.Properties.Resources.GPU;
            resources.ApplyResources(this.pictureBox2, "pictureBox2");
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox3.BackgroundImage = global::FastKing.Properties.Resources.ram;
            resources.ApplyResources(this.pictureBox3, "pictureBox3");
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.TabStop = false;
            // 
            // InputBox
            // 
            resources.ApplyResources(this.InputBox, "InputBox");
            this.InputBox.Name = "InputBox";
            this.InputBox.TabStop = false;
            this.InputBox.TextChanged += new System.EventHandler(this.InputBox_TextChanged);
            // 
            // ResultBox
            // 
            this.ResultBox.BackColor = System.Drawing.Color.Black;
            this.ResultBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            resources.ApplyResources(this.ResultBox, "ResultBox");
            this.ResultBox.ForeColor = System.Drawing.Color.White;
            this.ResultBox.Name = "ResultBox";
            this.ResultBox.ReadOnly = true;
            this.ResultBox.TabStop = false;
            // 
            // pictureBox4
            // 
            this.pictureBox4.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox4.BackgroundImage = global::FastKing.Properties.Resources.下载__1_;
            resources.ApplyResources(this.pictureBox4, "pictureBox4");
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            this.pictureBox5.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox5.BackgroundImage = global::FastKing.Properties.Resources.上传__1_;
            resources.ApplyResources(this.pictureBox5, "pictureBox5");
            this.pictureBox5.Name = "pictureBox5";
            this.pictureBox5.TabStop = false;
            // 
            // downloadSpeed
            // 
            this.downloadSpeed.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.downloadSpeed, "downloadSpeed");
            this.downloadSpeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.downloadSpeed.Name = "downloadSpeed";
            this.downloadSpeed.UseMnemonic = false;
            // 
            // uploadSpeed
            // 
            this.uploadSpeed.BackColor = System.Drawing.Color.WhiteSmoke;
            resources.ApplyResources(this.uploadSpeed, "uploadSpeed");
            this.uploadSpeed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.uploadSpeed.Name = "uploadSpeed";
            this.uploadSpeed.UseMnemonic = false;
            // 
            // GPU_Temp
            // 
            this.GPU_Temp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            resources.ApplyResources(this.GPU_Temp, "GPU_Temp");
            this.GPU_Temp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.GPU_Temp.Name = "GPU_Temp";
            this.GPU_Temp.UseMnemonic = false;
            // 
            // CPU_Temp
            // 
            this.CPU_Temp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            resources.ApplyResources(this.CPU_Temp, "CPU_Temp");
            this.CPU_Temp.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.CPU_Temp.Name = "CPU_Temp";
            this.CPU_Temp.UseMnemonic = false;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Window;
            resources.ApplyResources(this, "$this");
            this.Controls.Add(this.CPU_Temp);
            this.Controls.Add(this.GPU_Temp);
            this.Controls.Add(this.uploadSpeed);
            this.Controls.Add(this.downloadSpeed);
            this.Controls.Add(this.pictureBox5);
            this.Controls.Add(this.pictureBox4);
            this.Controls.Add(this.ResultBox);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.Memory_Text);
            this.Controls.Add(this.GPU_Load_Text);
            this.Controls.Add(this.CPU_Load_Text);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox5)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label CPU_Load_Text;
        private System.Windows.Forms.Label GPU_Load_Text;
        private System.Windows.Forms.Label Memory_Text;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.RichTextBox InputBox;
        private System.Windows.Forms.RichTextBox ResultBox;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.PictureBox pictureBox5;
        private System.Windows.Forms.Label downloadSpeed;
        private System.Windows.Forms.Label uploadSpeed;
        private System.Windows.Forms.Label GPU_Temp;
        private System.Windows.Forms.Label CPU_Temp;
    }
}

