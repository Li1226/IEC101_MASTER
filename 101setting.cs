using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace SM
{
    public partial class _101setting : Form
    {
        public _101setting()
        {
            InitializeComponent();
        }

        private void _101setting_Load(object sender, EventArgs e)
        {
            SerialPort_config Port_Config = new SerialPort_config();

            //获取串口端口
            Port_Config.Updata_Serialport_Name(this.comboBox5);

            //初始化界面值
            form_init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SerialPort_config Port_Config = new SerialPort_config();
            _101_config _101_config = new _101_config();

            //保存串口设置
            Port_Config.SerialPort_Save(this.comboBox5.Text, this.comboBox6.Text, 
                                        this.comboBox7.Text, this.comboBox8.Text, 
                                        this.comboBox9.Text);

            //保存101配置
            _101_config._101_Save(Convert.ToInt32(this.comboBox1.Text), Convert.ToInt32(this.comboBox2.Text),
                                  Convert.ToInt32(this.comboBox3.Text), Convert.ToInt32(this.comboBox4.Text),
                                  Convert.ToInt32(this.textBox1.Text), Convert.ToInt32(this.textBox2.Text));
            //保存完关闭窗口
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //按取消关闭窗口
            this.Close();
        }

        public void form_init()
        {
            //串口初始化
            try
            {
                comboBox5.SelectedIndex = 0;
            }
            catch//防止没有串口时候 出现错误
            {
                comboBox5.Text = "";
            }
            comboBox6.Text = "9600";
            comboBox7.Text = "8";
            comboBox8.Text = "1";
            comboBox9.Text = "None";

            //101配置初始化
            comboBox1.Text = "1";
            comboBox2.Text = "2";
            comboBox3.Text = "2";
            comboBox4.Text = "2";

            textBox1.Text = "1";
            textBox2.Text = "1";
        }
    }
}
