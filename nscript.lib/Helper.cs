using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace NScript
{
    public class NScriptForm : Form
    {
        public static void Open(string title, string button, Function doit)
        {
            NScriptForm f = new NScriptForm();
            f.button1.Text = button;
            f.Text = title;
            f._doit = doit;

            f.ShowDialog();
        }
        protected Button button1;
        protected TextBox textBox1;
        private Function _doit;

        public NScriptForm()
        {
            InitializeComponent();
        }

        #region Windows Form Designer generated code
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            this.button1.Dock = System.Windows.Forms.DockStyle.Top;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(559, 32);
            this.button1.TabIndex = 1;
            this.button1.Text = "DO";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 32);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(559, 310);
            this.textBox1.TabIndex = 2;
            this.textBox1.Text = "";
            this.AutoScaleBaseSize = new System.Drawing.Size(6, 15);
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(559, 342);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(178)));
            this.Name = "Form1";
            this.Text = "Form";
            this.ResumeLayout(false);

        }
        #endregion

        private void button1_Click(object sender, System.EventArgs e)
        {
            button1.Enabled = false;
            StringBuilder ar = new StringBuilder();
            string path = Directory.GetCurrentDirectory();

            textBox1.Text = "";
            textBox1.Text += ".net version = " + Environment.Version + "\r\n";
            textBox1.Text += "Started : " + DateTime.Now.ToString() + "\r\n";

            _doit(ar);

            textBox1.Text += "Ended : " + DateTime.Now.ToString() + "\r\nDirectories : \r\n\r\n"; ;
            textBox1.Text += ar.ToString();
            button1.Enabled = true;
        }
    }
}
