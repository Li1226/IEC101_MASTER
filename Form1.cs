using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Threading;//sleep

namespace SM
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void ToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            //设置平衡模式标志位
            _101_config _101_Config = new _101_config();
            _101_Config._101_Balance(true);

            //打开配置窗口
            _101setting _101Setting = new _101setting();
            _101Setting.ShowDialog();
        }

        private void ToolStripMenuItem9_Click(object sender, EventArgs e)
        {
            SerialPort_config port_Config = new SerialPort_config();
            if (serialPort1.IsOpen)//关闭连接
            {
                port_Config.SerialPort_Close(serialPort1, ToolStripMenuItem9);
            }
            else//打开连接
            {
                port_Config.SerialPort_Open(serialPort1, ToolStripMenuItem9,this.richTextBox1);
                //设置打开连接就发送请求连接报文--->后面需加定时器，定时发送连接报文
                _101_SendOut_Data Send_Data = new _101_SendOut_Data();
                _101_config.process_state = (int)_101_config.IEC_101_Process_State.UNCONNECT;
                Send_Data.Send_Msg_Manage_bt();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SerialPort_config port_Config = new SerialPort_config();
            port_Config.Send_Data(serialPort1, textBox2.Text);//发送数据
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void ToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            _101_function _101_Function = new _101_function();
            string time = System.DateTime.Now.ToString("yyyy-MM-dd_HH_mm_ss");
            saveFileDialog1.FileName = time;//文件名字
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _101_Function.Save_Data(saveFileDialog1.FileName, richTextBox1.Text);
            }
        }

        private void ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            //打开测试选项窗口
            _TextOptions textOptions = new _TextOptions();
            textOptions.Show();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            Thread.Sleep(100);
            int n = serialPort1.BytesToRead;
            if (n == 0)
                return;
            byte[] data = new byte[n];
            serialPort1.Read(data, 0, n);
            //添加接收到报文处理函数
            _101_receive_handle _101_Receive_Handle = new _101_receive_handle();
            _101_Receive_Handle.Receive_Data_Check(data);
        }

        private void ToolStripMenuItem10_Click(object sender, EventArgs e)
        {
            YK ykshow = new YK();
            ykshow.Show();
        }
    }
}
