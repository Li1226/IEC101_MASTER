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
    public partial class AutoTest : Form
    {
        static bool SendFlag;

        public AutoTest()
        {
            InitializeComponent();
        }

        private void AutoTest_Load(object sender, EventArgs e)
        {
            AutoTestInit();
        }

        private void VisitTime_Tick(object sender, EventArgs e)
        {
            if (SendFlag)
            {
                if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                {
                    bool[] Options = _101_config.Test_Options;
                    for (int i = 0; i < Options.Length; i++)
                    {
                        if (Options[i] == true)
                        {
                            textBox1.AppendText(Show_Explain(i, true));
                            _101_SendOut_Data SendData = new _101_SendOut_Data();
                            SendData.Jumpto_Send_Data(i);
                            SendFlag = false;
                            return;
                        }
                    }
                }
            }
            else
            {
                if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                {
                    bool[] Options = _101_config.Test_Options;
                    for (int i = 0; i < Options.Length; i++)
                    {
                        if (Options[i] == true)
                        {
                            this.progressBar1.PerformStep();
                            textBox1.AppendText(Show_Explain(i, false));
                            Options[i] = false;
                            SendFlag = true;
                            return;
                        }
                    }
                }
            }
        }

        private void AutoTestInit()
        {
            this.progressBar1.Step = (100 / (_101_config.num) + 1);//设置进度条
            textBox1.AppendText("开始测试......\r\n");
            SendFlag = true;
            this.VisitTime.Enabled = true;
        }

        private string Show_Explain(int i, bool type)
        {
            switch (i)
            {
                case _101_config.General_Call_Ti100:
                    if (type == true)
                    {
                        return "正在测试总召唤命令中...\r\n";
                    }
                    else
                    {
                        return "总召唤测试完成\r\n";
                    }
                case _101_config.Read_Clock_Ti103:
                    if (type == true)
                    {
                        return "正在测试读取时钟命令中...\r\n";
                    }
                    else
                    {
                        return "读取时钟命令测试完成\r\n";
                    }
                case _101_config.Synchro_Clock_Ti103:
                    if (type == true)
                    {
                        return "正在测试时钟同步命令中...\r\n";
                    }
                    else
                    {
                        return "时钟同步测试完成\r\n";
                    }
                case _101_config.Link_Test_Ti104:
                    if (type == true)
                    {
                        return "正在测试测试命令中...\r\n";
                    }
                    else
                    {
                        return "测试命令测试完成\r\n";
                    }
                case _101_config.Heart_Test:
                    if (type == true)
                    {
                        return "正在测试心跳测试命令中...\r\n";
                    }
                    else
                    {
                        return "心跳测试命令测试完成\r\n";
                    }
                case _101_config.Reset_Ti105:
                    if (type == true)
                    {
                        return "正在测试复位进程命令中...\r\n";
                    }
                    else
                    {
                        return "复位进程命令测试完成\r\n";
                    }
            }
            return "";
        }

    }
}
