using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM
{
    class _101_config
    {
        static public int process;//记录是否在进程中
        static public int process_state;//记录流程状态

        public enum IEC_101_Process{
            UNSTART = 0x00,
            GOING,
            END
        }
        public enum IEC_101_Process_State {
            UNCONNECT = 0x00,   //未连接
            LINK_STAUS,         //请求链路连接
            RESET_LINK,         //复位链路
            LINK_STAUS_END,     //初始化结束
            INIT_CALL,          //初始化总召
            TIME_READ,          //时间读取
            TIME_CHECK,         //时间同步
            TEST_LOOPBACK,      //测试命令 Type104
            TEST_HEART,         //心跳测试 平衡模式特有
            RESET_LOOPBACK,     //复位进程 Type105
            YK_SELECT,          //遥控选择
            YK_EXECUTE,         //遥控执行
            YK_CANCEL,          //遥控取消
        };

        public enum IEC_101_Msg {
            variable_state_code = 0x68,
            fixed_state_code = 0x10,
            end_code = 0x16,
            STATR_PRM = 0x40,
            ANSWER_PRM = 0x00,
            SET_FCB_ACD = 0x20,
            SET_FCV_DFC = 0x10
        };
        //主动发送方向的链路层功能码
        public enum IEC101_CDIR_LK_CODE
        {
            FUN_RST_LK = 0,          // 发送帧-复位远方链路
            FUN_RST_CU,              // 发送帧-复位远动终端的用户进程
            FUN_TEST,                // 用于平衡式传输过程测试链路功能
            FUN_SD,                  // 发送帧-传送数据
            FUN_GD,                  // 发送帧-广播方式传送数据
            FUN_AS = 9,              // 请求帧-召唤链路状态
            FUN_A1,                  // 请求帧-召唤1级数据
            FUN_A2                   // 请求帧-召唤2级数据
        };
        //接收方向的链路层功能码
        public enum IEC101_MDIR_LK_CODE
        {
            FUN_ACK = 0,             // 确认帧-确认
            FUN_ACKBS,               // 确认帧-否定认可
            FUN_RSDT = 8,            // 响应帧-以数据包响应请求帧
            FUN_RSNO,                // 响应帧-无所召唤的数据帧
            FUN_RSBS = 11            // 响应帧-从站以链路状态响应主站请求
        };

        public enum IEC101_CONTROL_DEF {
        CN_CTL_LK_MAIN = 0x80,           // 控制域主站标识
        CN_CTL_LK_PRM = 0x40  ,          // 控制域启动报文位
        CN_CTL_LK_FCB_ACD = 0x20 ,       // 帧计数位/要求访问位
        CN_CTL_LK_FCV_DFC = 0x10 ,       // 帧计数有效位/数据流控制位
        CN_CTL_LK_FUN = 0x0f ,           // 帧功能数据
        };

        //ASDU监视方向(monitor direction)的类型标识
        public enum IEC101_ASDU_MDIR_TYP_CODE
        {
            EN_IEC101_ASDU_TYP001 = 0x01,           // 不带时标的单点信息
            EN_IEC101_ASDU_TYP002,                  // 带时标的单点信息
            EN_IEC101_ASDU_TYP003,                  // 不带时标的双点信息
            EN_IEC101_ASDU_TYP004,                  // 带时标的双点信息
            EN_IEC101_ASDU_TYP009 = 0x09,          // 归一化测量值
            EN_IEC101_ASDU_TYP011 = 0x0b,          // 标度化测量值
            EN_IEC101_ASDU_TYP013 = 0x0d,          // 测量值，短浮点数
            EN_IEC101_ASDU_TYP015 = 0x0f,          // 电度值
            EN_IEC101_ASDU_TYP030 = 0x1e,          // 带时标的单点信息
            EN_IEC101_ASDU_TYP031 = 0x1f,          // 带时标的双点信息
            EN_IEC101_ASDU_TYP042 = 0x2a,          // 带时标的故障信息
            EN_IEC101_ASDU_TYP045 = 0x2d,          // 单点遥控
            EN_IEC101_ASDU_TYP046 = 0x2e,          // 双点遥控
            EN_IEC101_ASDU_TYP055 = 0x37,                   //参数预置/激活------(南网)
            EN_IEC101_ASDU_TYP070 = 0x46,          //初始化完成asdu
            EN_IEC101_ASDU_TYP100 = 0x64,          //总召唤确认
            EN_IEC101_ASDU_TYP101,
            EN_IEC101_ASDU_TYP103 = 0x67,          //对时确认asdu
            EN_IEC101_ASDU_TYP104,                //测试帧
            EN_IEC101_ASDU_TYP105,                //复位进程
            EN_IEC101_ASDU_TYP108 = 0x6C,                   //参数读取------(南网)
        };

        //传送原因
        public enum _IEC101_ASDU_COT_CODE_
        {
            EN_IEC101_ASDU_COT_NULL = 0x00,      // 无效值
            EN_IEC101_ASDU_COT_CYC = 0x01,      // 循环
            EN_IEC101_ASDU_COT_BACK = 0x02,      // 背景扫描
            EN_IEC101_ASDU_COT_SPONT = 0x03,      // 自发(突发)
            EN_IEC101_ASDU_COT_INIT,                // 初始化
            EN_IEC101_ASDU_COT_RESP,                // 请求或被请求
            EN_IEC101_ASDU_COT_ACT = 0x06,          // 激活
            EN_IEC101_ASDU_COT_ACT_ACK,             // 激活确认
            EN_IEC101_ASDU_COT_STOP_ACT,            // 停止激活
            EN_IEC101_ASDU_COT_STOP_ACT_ACK,        // 停止激活确认
            EN_IEC101_ASDU_COT_ACT_END,             // 激活结束
            EN_IEC101_ASDU_COT_RESP_CALL = 0x14,    // 响应总召唤
            EN_IEC101_ASDU_COT_RESP_CALL_Inrol1,    // 响应组1总召唤

            EN_IEC101_ASDU_COT_RESP_UnknownASDU = 0x2C,    // 响应不支持该ASDU
            EN_IEC101_ASDU_COT_RESP_UnknownCOT,          // 响应不支持该COT
            EN_IEC101_ASDU_COT_RESP_UnknownCOA,          // 响应不支持该COA
            EN_IEC101_ASDU_COT_RESP_UnknownINF,          // 响应不支持该INF

        };

        static public bool _101_balance;//平衡模式标志位
        static public bool _101_unbalance;//非平衡标志位

        static public int cotsize;//传送原因长度
        static public int ipaddr_size;//链路地址长度
        static public int asduaddr_size;//装置地址长度
        static public int infaddr_size;//信息体地址长度

        static public int FCB;//FCB位
        static public int ipaddr;//链路地址
        static public int asduaddr;//装置地址

        public const int General_Call_Ti100 = 0;
        public const int Read_Clock_Ti103 = 1;
        public const int Synchro_Clock_Ti103 = 2;
        public const int Link_Test_Ti104 = 3;
        public const int Heart_Test = 4;
        public const int Reset_Ti105 = 5;
        static public int num;//记录选中测试的个数
        static public bool[] Test_Options = new bool[6];
        static public bool[] Check_Options = new bool[6];

        //设置平衡模式
        public void _101_Balance(bool i)
        {
            _101_balance = i;
        }

        //101规约保存参数
        public void _101_Save(int cot, int ipsize, int asdusize, int inf, int ip, int asdu)
        {
            cotsize = cot; 
            ipaddr_size = ipsize;
            asduaddr_size = asdusize;
            infaddr_size = inf;

            ipaddr = ip;
            asduaddr = asdu;
        }

        public void AutoTest_init()
        {
            //process = (int)IEC_101_Process.UNSTART;
            //process_state = (int)IEC_101_Process_State.UNCONNECT;
            //FCB = 1;
            num = 0;
            for (int i = 0; i < Test_Options.Length; i++)
            {
                Test_Options[i] = false;
                Check_Options[i] = false;
            }
        }

        public void Setup_Options(int i)
        {
            num++;
            switch (i)
            {
                case General_Call_Ti100:
                    Test_Options[0] = true;
                    break;

                case Read_Clock_Ti103:
                    Test_Options[1] = true;
                    break;

                case Synchro_Clock_Ti103:
                    Test_Options[2] = true;
                    break;

                case Link_Test_Ti104:
                    Test_Options[3] = true;
                    break;

                case Heart_Test:
                    Test_Options[4] = true;
                    break;

                case Reset_Ti105:
                    Test_Options[5] = true;
                    break;

                default:
                    break;
            }
        }

        public void Setup_Check(int i)
        {
            switch (i)
            {
                case General_Call_Ti100:
                    Check_Options[0] = true;
                    break;

                case Read_Clock_Ti103:
                    Check_Options[1] = true;
                    break;

                case Synchro_Clock_Ti103:
                    Check_Options[2] = true;
                    break;

                case Link_Test_Ti104:
                    Check_Options[3] = true;
                    break;

                case Heart_Test:
                    Check_Options[4] = true;
                    break;

                case Reset_Ti105:
                    Check_Options[5] = true;
                    break;

                default:
                    break;
            }
        }
    }
}
