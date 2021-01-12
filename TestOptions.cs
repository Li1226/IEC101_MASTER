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
    public partial class _TextOptions : Form
    {
        public _TextOptions()
        {
            InitializeComponent();
        }
        
        private void _TextOptions_Load(object sender, EventArgs e)
        {
            _101_config _101_Config = new _101_config();
            _101_Config.AutoTest_init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _101_config _101_Config = new _101_config();
            int flag = 0;
            for (int i = 0; i< checkedListBox1.Items.Count; i++)
            {
                if (checkedListBox1.GetItemChecked(i))
                {
                    _101_Config.Setup_Options(i);
                    flag++;
                }
            }
            if (flag == 0)//没有选择选项
            {
                MessageBox.Show("没有进行选择！", "提示", MessageBoxButtons.OK);
            }
            else //打开自动测试窗口
            {
                AutoTest autoTest = new AutoTest();
                autoTest.Show();
                this.Hide();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
