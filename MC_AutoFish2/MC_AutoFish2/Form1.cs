using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MC_AutoFish2
{
    public partial class Form1 : Form
    {
        [System.Runtime.InteropServices.DllImport("user32")]
        private static extern int mouse_event(int mouseevent, int dx, int dy, int cButtons, int dwExtraInfo);
        [DllImport("User32")]
        public extern static void SetCursorPos(int x, int y);

        private bool gofishing=false; 
        //移动鼠标 
        const int MOUSEEVENTF_MOVE = 0x0001;
        //模拟鼠标左键按下 
        const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        //模拟鼠标左键抬起 
        const int MOUSEEVENTF_LEFTUP = 0x0004;
        //模拟鼠标右键按下 
        const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //模拟鼠标右键抬起 
        const int MOUSEEVENTF_RIGHTUP = 0x0010;
        //模拟鼠标中键按下 
        const int MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        //模拟鼠标中键抬起 
        const int MOUSEEVENTF_MIDDLEUP = 0x0040;
        //标示是否采用绝对坐标 
        const int MOUSEEVENTF_ABSOLUTE = 0x8000;


        public Form1()
        {
            InitializeComponent();  
            timer1.Interval = 1000;  
        }

        private void button1_Click(object sender, EventArgs e)
        {

            var ps = Process.GetProcessesByName("minecraft").ToList();
            if (ps == null || ps.Count == 0) { MessageBox.Show("没有检测到游戏，请开启游戏后重试"); return; }
            gofishing = !gofishing;
            if (gofishing) {
                button1.Text = "钓鱼中....";
                richTextBox1.Add("钓鱼已开启，" + DateTime.Now.ToString("MM-dd HH:mm:ss"));
                timer1.Enabled = true;
                timer2.Enabled = checkBox2.Checked;
                timer3.Enabled = checkBox3.Checked;
            } 
            else 
            { 
                button1.Text = "开始";
                richTextBox1.Add("钓鱼已结束，" + DateTime.Now.ToString("MM-dd HH:mm:ss"));
            };
            groupBox1.Enabled = !gofishing;
           


        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString()==" "&& gofishing)
            {
                gofishing = !gofishing; ; //满足条件后执行事件
                groupBox1.Enabled = !gofishing;
                timer1.Enabled = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            //连续两次点击--
            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, 0, 0, 0, 0); 
            if (checkBox1.Checked)
            {
                Random RA = new Random();
                timer1.Interval = RA.Next(Convert.ToInt32(textBox1.Text), (Convert.ToInt32(textBox1.Text) + 500));
            }
            Console.WriteLine(timer1.Interval.ToString());
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            if (gofishing)
            {
                timer1.Enabled=false;
                timer2.Interval = 60000;
                gofishing = false;
            }
            else
            {
                timer1.Enabled = true;
                timer2.Interval = 300000;
                gofishing = true;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar)) && e.KeyChar != (char)8)
            {
                e.Handled = true;
            }
            if (textBox1.Text.Length==1&& (e.KeyChar==(char)Keys.Delete|| e.KeyChar == (char)Keys.Back))
            {
                e.Handled = true;
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            var ps = Process.GetProcessesByName("minecraft").ToList();
            if (ps == null || ps.Count == 0) { MessageBox.Show("检测到游戏已关闭，请查看账号状态！");

                richTextBox1.Add("封号警告：检测到游戏已关闭，请查看账号状态！，" + DateTime.Now.ToString("MM-dd HH:mm:ss"));
                 }
        }
    }
    public static class TextBoxHelper 
    {
        public static void Add(this RichTextBox rtbox, string text)
        {
            if (string.IsNullOrEmpty(rtbox.Text))
            {
                rtbox.Text = text;
            }
            else
            {
                rtbox.AppendText(Environment.NewLine);
                rtbox.AppendText(text);
            }
        }
    }
}
