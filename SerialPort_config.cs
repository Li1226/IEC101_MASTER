using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace SM
{
    class SerialPort_config
    {
        static string Port_name = string.Empty;
        static string BaudRate = string.Empty;
        static string DataBits = string.Empty;
        static string StopBits = string.Empty;
        static string Parity = string.Empty;
        static SerialPort port1;
        static RichTextBox textbox;

        public void Updata_Serialport_Name(ComboBox MycomboBox)
        {
            Form1 Form1 = new Form1();

            if (Form1.serialPort1.IsOpen)
            {
                return;
            }
            else
            {
                string[] PortArray = SerialPort.GetPortNames();
                MycomboBox.Items.Clear();
                for (int i = 0; i < PortArray.Length; i++)
                {
                    MycomboBox.Items.Add(PortArray[i]);
                }
            }
        }

        public void SerialPort_Save(string Port, string Buad, string Data, string Stop, string Par)
        {
            Port_name = Port;
            BaudRate = Buad;
            DataBits = Data;
            StopBits = Stop;
            Parity = Par;
        }
        public bool SerialPort_Open(SerialPort port, ToolStripMenuItem text,RichTextBox textBox)
        {
            textbox = textBox;
            if (_101_config._101_balance)//平衡模式下打开链接方式为串口
            {
                try
                {
                    Open(port);
                    text.Text = "断开连接";
                    return true;
                }
                catch
                {
                    MessageBox.Show("打开失败！\r\n请检查配置文件！", "提示", MessageBoxButtons.OK);
                    return false;
                }
            }
            MessageBox.Show("打开失败！\r\n请检查配置文件！", "提示", MessageBoxButtons.OK);
            return false;
        }
        private void Open(SerialPort port)
        {
            port.PortName = Port_name;
            port.BaudRate = Convert.ToInt32(BaudRate);
            port.DataBits = Convert.ToInt32(DataBits);
            port.StopBits = Setup_StopBits(StopBits);//设置停止位
            port.Parity = Setup_Parity(Parity);//设置校验位

            port1 = port;
            port.Open();
        }

        //设置停止位
        private StopBits Setup_StopBits(string str)
        {
            if (str == "1")
                return System.IO.Ports.StopBits.One;
            else if (str == "1.5")
                return System.IO.Ports.StopBits.OnePointFive;
            else if (str == "2")
                return System.IO.Ports.StopBits.Two;
            return System.IO.Ports.StopBits.One;
        }
        //设置校验位
        private Parity Setup_Parity(string str)
        {
            if (str == "None")
                return System.IO.Ports.Parity.None;
            else if (str == "Odd")
                return System.IO.Ports.Parity.Odd;
            else if (str == "Even")
                return System.IO.Ports.Parity.Even;
            return System.IO.Ports.Parity.None;
        }

        public bool SerialPort_Close(SerialPort port, ToolStripMenuItem text)
        {
            if (_101_config._101_balance)//平衡模式
            {
                try
                {
                    Close(port);
                    text.Text = "打开连接";
                    return true;
                }
                catch
                {
                    MessageBox.Show("关闭失败！", "提示", MessageBoxButtons.OK);
                    return false;
                }
            }
            return false;
        }

        private void Close(SerialPort port)
        {
            port.Close();
        }

        public void Send_Data(SerialPort port, string str)
        {
            str += "\r\n";
            string Send_Str = str.Replace(" ", "");
            if ((Send_Str.Length / 2) != 0)
            {
                Send_Str = Send_Str.Substring(0, (Send_Str.Length - 1));
            }

            byte[] Send_data = new byte[Send_Str.Length / 2];
            for (int k = 0; k < Send_data.Length; k++)
            {
                Send_data[k] = Convert.ToByte(Send_Str.Substring(k * 2, 2), 16);
            }
            Send_Data(Send_data, Send_data.Length);
        }
        public void Send_Data(byte [] data,int len)
        {
            Analysis_msg(data,len);
            port1.Write(data, 0, len);
        }

        public static void Analysis_msg(byte [] data,int n)
        {
            _101_function _101_Function = new _101_function();
            _101_Function.Show_Data(data, n, textbox);
        }
    }
}
