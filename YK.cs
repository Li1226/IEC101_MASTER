using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SM
{
    public partial class YK : Form
    {
        public YK()
        {
            InitializeComponent();
        }

        public static bool select;
        public static bool execute;
        public static bool cancel;
        static int n;
        static int se;
        static int cot;
        static int infaddr;
        static int type;
        static bool station;// true = 合, false = 分位

        public static byte Get_YK_SCO_DCO()
        {
            byte data = 0;
            if(station)
            {
                data = (byte)(se + 0x01);
            }
            else
            {
                data = (byte)(se + 0x00);
            }
            return data;
        }

        public static bool Get_Station()
        {
            return station;
        }

        public static int Get_Type()
        {
            return type;
        }

        public static int Get_Cot()
        {
            return cot;
        }

        public static int Get_Infaddr()
        {
            return infaddr;
        }

        private bool SetOvertime(int time)//time为秒
        {
            if ((0 <= time) && (time <= 30))
            {
                n = ((time * 1000) / timer1.Interval) + 1;
            }
            else
            {
                MessageBox.Show("校准时间应在0~30秒!\r\n", "提示", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private bool YK_Init()
        {
            try
            {
                infaddr = Get_Infaddr(YK_infaddr.Text);
                if (YK_type.Text == "单点遥控")
                {
                    type = (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP045;
                }
                else if(YK_type.Text == "双点遥控")
                {
                    type = (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP046;
                }
                if (this.YK_OnStation.Checked)
                {
                    station = true;
                }
                else if (this.YK_OffStation.Checked)
                {
                    station = false;
                }
                if (SetOvertime(Convert.ToInt16(YK_overtime.Text)) == false)
                {
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("配置错误！\r\n请检查配置！", "提示", MessageBoxButtons.OK);
                return false;
            }
            return true;
        }

        private int Get_Infaddr(string InfStr)
        {
            int infaddr = 0;
            string Str = InfStr.Replace(" ", "");
            byte[] data = new byte[2];
            for (int k = 0; k < data.Length; k++)
            {
                data[k] = Convert.ToByte(Str.Substring(k * 2, 2), 16);
            }
            infaddr = (data[0] << 8) + data[1];

            return infaddr;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (select)
            {
                select = false;
                this.Explain.AppendText("预置");
                this.Explain.SelectionColor = System.Drawing.Color.Red;
                this.Explain.AppendText("成功\r\n");
                this.Explain.SelectionColor = System.Drawing.Color.Black;
            }
            else if (execute)
            {
                execute = false;
                this.Explain.AppendText("执行");
                this.Explain.SelectionColor = System.Drawing.Color.Red;
                this.Explain.AppendText("成功\r\n");
                this.Explain.SelectionColor = System.Drawing.Color.Black;
            }
            else if (cancel)
            {
                cancel = false;
                this.Explain.AppendText("取消");
                this.Explain.SelectionColor = System.Drawing.Color.Red;
                this.Explain.AppendText("成功\r\n");
                this.Explain.SelectionColor = System.Drawing.Color.Black;
            }
            if (_101_config.process_state == (int)_101_config.IEC_101_Process_State.YK_SELECT)
            {
                if (n == 0)
                {
                    this.Explain.AppendText("预置");
                    this.Explain.SelectionColor = System.Drawing.Color.Red;
                    this.Explain.AppendText("失败\r\n");
                    this.Explain.SelectionColor = System.Drawing.Color.Black;
                    this.timer1.Stop();//时间到关闭定时器
                }
            }
            n--;
        }

        private void YK_choise_Click(object sender, EventArgs e)
        {
            if (YK_Init())
            {
                this.Explain.Text = "";
                cot = (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT;
                se = 0x80;
                this.Explain.AppendText("发送遥控预置...\r\n");
                _101_config.process_state = (int)_101_config.IEC_101_Process_State.YK_SELECT;
                _101_SendOut_Data Send_Data = new _101_SendOut_Data();
                Send_Data.Send_Msg_Manage_bt();
                this.timer1.Start();
            }
        }

        private void YK_Load(object sender, EventArgs e)
        {
            this.YK_overtime.Text = "30";
            this.YK_type.SelectedIndex = 1;
            this.Explain.Text = "地址为16进制地址,例6001\r\n时间范围:0~30S\r\n";
        }
    }
}
