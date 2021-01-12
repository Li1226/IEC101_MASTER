using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM
{
    class _101_SendOut_Data
    {

        private void Send_Data_Transmit( byte[] data, int len)
        {
            _101_function _101_Function = new _101_function();

            if (data[0] == (byte)_101_config.IEC_101_Msg.fixed_state_code)
                data[len++] = _101_Function.Get_CS(data, 1, 3);
            else
                data[len++] = _101_Function.Get_CS(data,4,len-4);
            data[len++] = (byte)_101_config.IEC_101_Msg.end_code;

            SerialPort_config serialPort = new SerialPort_config();
            serialPort.Send_Data(data, len);
        }

        public void Send_Data_Fixed(int PRM, int FCB, int FCV, int CMD)
        {
            int n = 0;
            byte[] data = new byte[4+_101_config.ipaddr_size];

            data[n++] = (byte)_101_config.IEC_101_Msg.fixed_state_code;
            data[n++] = _101_function.Data_Get_Cctrl(PRM,FCB,FCV,CMD);
            _101_function.number_to_octets(ref data, 2, _101_config.ipaddr, _101_config.ipaddr_size);
            n += _101_config.ipaddr_size;
            Send_Data_Transmit( data, n);
        }

        private void Send_Data_Variable_INIT_CALL_Type100()//总召唤
        {
            byte[] data = new byte[256];
            int n = 0;
            
            data[n] = (int)_101_config.IEC_101_Msg.variable_state_code;
            n += 3;
            data[n++] = (int)_101_config.IEC_101_Msg.variable_state_code;
            data[n++] = _101_function.Data_Get_Cctrl(1,_101_config.FCB,1,(int)_101_config.IEC101_CDIR_LK_CODE.FUN_SD);
            _101_function.number_to_octets(ref data,n,_101_config.ipaddr,_101_config.ipaddr_size);
            n += _101_config.ipaddr_size;
            data[n++] = (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP100;
            data[n++] = 1;
            _101_function.number_to_octets(ref data,n,(int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT,_101_config.cotsize);
            n += _101_config.cotsize;
            _101_function.number_to_octets(ref data, n, _101_config.asduaddr, _101_config.asduaddr_size);
            n += _101_config.asduaddr_size;
            _101_function.number_to_octets(ref data, n, 0, _101_config.infaddr_size);
            n += _101_config.infaddr_size;
            data[n++] = (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP_CALL;
            
            data[1] =  data[2] = (byte)(n - 4);
            Send_Data_Transmit(data, n);
        }

        private void Send_Data_Variable_TimeCheck_Type103()
        {
            byte[] data = new byte[256];
            int n = 0;

            data[n] = (int)_101_config.IEC_101_Msg.variable_state_code;
            n += 3;
            data[n++] = (int)_101_config.IEC_101_Msg.variable_state_code;
            data[n++] = _101_function.Data_Get_Cctrl(1, _101_config.FCB, 1, (int)_101_config.IEC101_CDIR_LK_CODE.FUN_SD);
            _101_function.number_to_octets(ref data, n, _101_config.ipaddr, _101_config.ipaddr_size);
            n += _101_config.ipaddr_size;
            data[n++] = (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP103;
            data[n++] = 1;
            if (_101_config.process_state == (int)_101_config.IEC_101_Process_State.TIME_CHECK)
            {//时钟同步
                _101_function.number_to_octets(ref data, n, (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT, _101_config.cotsize);
                n += _101_config.cotsize;
                _101_function.number_to_octets(ref data, n, _101_config.asduaddr, _101_config.asduaddr_size);
                n += _101_config.asduaddr_size;
                _101_function.number_to_octets(ref data, n, 0, _101_config.infaddr_size);
                n += _101_config.infaddr_size;
                //添加时间7字节
                data[n++] = (byte)(System.DateTime.Now.Millisecond + (System.DateTime.Now.Second * 1000));
                data[n++] = (byte)(System.DateTime.Now.Millisecond + (System.DateTime.Now.Second * 1000) >> 8);
                data[n++] = (byte)(System.DateTime.Now.Minute & 0x3f);
                data[n++] = (byte)(System.DateTime.Now.Hour & 0x1f);
                data[n++] = (byte)(System.DateTime.Now.Day & 0x1f);
                data[n++] = (byte)(System.DateTime.Now.Month & 0x0f);
                data[n++] = (byte)((System.DateTime.Now.Year - 2000) & 0x7f);

                data[1] = data[2] = (byte)(n - 4);
                Send_Data_Transmit(data, n);
            }
            else if (_101_config.process_state == (int)_101_config.IEC_101_Process_State.TIME_READ)
            { //时钟读取
                _101_function.number_to_octets(ref data, n, (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP, _101_config.cotsize);
                n += _101_config.cotsize;
                _101_function.number_to_octets(ref data, n, _101_config.asduaddr, _101_config.asduaddr_size);
                n += _101_config.asduaddr_size;
                _101_function.number_to_octets(ref data, n, 0, _101_config.infaddr_size);
                n += _101_config.infaddr_size;
                //添加时间7字节
                data[n++] = 0;
                data[n++] = 0;
                data[n++] = 0;
                data[n++] = 0;
                data[n++] = 0;
                data[n++] = 0;
                data[n++] = 0;

                data[1] = data[2] = (byte)(n - 4);
                Send_Data_Transmit(data, n);
            }
        }

        private void Send_Data_Variable_TextLoopback_Type104()
        {
            byte[] data = new byte[256];
            int n = 0;

            data[n] = (int)_101_config.IEC_101_Msg.variable_state_code;
            n += 3;
            data[n++] = (int)_101_config.IEC_101_Msg.variable_state_code;
            data[n++] = _101_function.Data_Get_Cctrl(1, _101_config.FCB, 1, (int)_101_config.IEC101_CDIR_LK_CODE.FUN_SD);
            _101_function.number_to_octets(ref data, n, _101_config.ipaddr, _101_config.ipaddr_size);
            n += _101_config.ipaddr_size;
            data[n++] = (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP104;
            data[n++] = 1;
            _101_function.number_to_octets(ref data, n, (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT, _101_config.cotsize);
            n += _101_config.cotsize;
            _101_function.number_to_octets(ref data, n, _101_config.asduaddr, _101_config.asduaddr_size);
            n += _101_config.asduaddr_size;
            _101_function.number_to_octets(ref data, n, 0, _101_config.infaddr_size);
            n += _101_config.infaddr_size;
            //FBP
            data[n++] = 0xaa;
            data[n++] = 0x55;

            data[1] = data[2] = (byte)(n - 4);
            Send_Data_Transmit(data, n);
        }

        private void Send_Data_Variable_YK()
        {
            byte[] data = new byte[256];
            int n = 0;

            data[n] = (int)_101_config.IEC_101_Msg.variable_state_code;
            n += 3;
            data[n++] = (int)_101_config.IEC_101_Msg.variable_state_code;
            data[n++] = _101_function.Data_Get_Cctrl(1, _101_config.FCB, 1, (int)_101_config.IEC101_CDIR_LK_CODE.FUN_SD);
            _101_function.number_to_octets(ref data, n, _101_config.ipaddr, _101_config.ipaddr_size);
            n += _101_config.ipaddr_size;
            data[n++] = (byte)YK.Get_Type();
            data[n++] = 1;
            _101_function.number_to_octets(ref data, n, YK.Get_Cot(), _101_config.cotsize);
            n += _101_config.cotsize;
            _101_function.number_to_octets(ref data, n, _101_config.asduaddr, _101_config.asduaddr_size);
            n += _101_config.asduaddr_size;
            _101_function.number_to_octets(ref data, n, YK.Get_Infaddr(), _101_config.infaddr_size);
            n += _101_config.infaddr_size;
            //单双命令
            data[n++] = YK.Get_YK_SCO_DCO();

            data[1] = data[2] = (byte)(n - 4);
            Send_Data_Transmit(data, n);
        }

        private void Send_Data_Variable_ResetLoopback_Type105()
        {
            byte[] data = new byte[256];
            int n = 0;

            data[n] = (int)_101_config.IEC_101_Msg.variable_state_code;
            n += 3;
            data[n++] = (int)_101_config.IEC_101_Msg.variable_state_code;
            data[n++] = _101_function.Data_Get_Cctrl(1, _101_config.FCB, 1, (int)_101_config.IEC101_CDIR_LK_CODE.FUN_SD);
            _101_function.number_to_octets(ref data, n, _101_config.ipaddr, _101_config.ipaddr_size);
            n += _101_config.ipaddr_size;
            data[n++] = (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP105;
            data[n++] = 1;
            _101_function.number_to_octets(ref data, n, (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT, _101_config.cotsize);
            n += _101_config.cotsize;
            _101_function.number_to_octets(ref data, n, _101_config.asduaddr, _101_config.asduaddr_size);
            n += _101_config.asduaddr_size;
            _101_function.number_to_octets(ref data, n, 0, _101_config.infaddr_size);
            n += _101_config.infaddr_size;
            //QRP
            data[n++] = 1;

            data[1] = data[2] = (byte)(n - 4);
            Send_Data_Transmit(data, n);
        }

        public void Send_Msg_Manage_bt()
        {
            //从机未在线
            if ((int)_101_config.process_state == (int)_101_config.IEC_101_Process_State.UNCONNECT)
            {
                _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                _101_config.process_state = (int)_101_config.IEC_101_Process_State.LINK_STAUS;
            }
            
            //发送报文
            switch ((int)_101_config.process_state)
            {
                case (int)_101_config.IEC_101_Process_State.LINK_STAUS://请求链路连接
                    Send_Data_Fixed(1, _101_config.FCB, 0, (int)_101_config.IEC101_CDIR_LK_CODE.FUN_AS);
                    _101_config.process = (int)_101_config.IEC_101_Process.GOING;//进行流程中
                    break;

                case (int)_101_config.IEC_101_Process_State.RESET_LINK://复位远方链路
                    Send_Data_Fixed(1,0,0,(int)_101_config.IEC101_CDIR_LK_CODE.FUN_RST_LK);
                    _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                    break;

                case (int)_101_config.IEC_101_Process_State.INIT_CALL://初始化总召唤
                    //添加发送长帧
                    if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                    {
                        Send_Data_Variable_INIT_CALL_Type100();
                        _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                    }
                    break;

                case (int)_101_config.IEC_101_Process_State.YK_SELECT://遥控选择
                    //if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                    //{
                        Send_Data_Variable_YK();
                        _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                    //}
                    break;

                case (int)_101_config.IEC_101_Process_State.TIME_READ:
                case (int)_101_config.IEC_101_Process_State.TIME_CHECK:
                    if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                    {
                        Send_Data_Variable_TimeCheck_Type103();
                        _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                    }
                    break;

                case (int)_101_config.IEC_101_Process_State.TEST_LOOPBACK:
                    if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                    {
                        Send_Data_Variable_TextLoopback_Type104();
                        _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                    }
                    break;

                case (int)_101_config.IEC_101_Process_State.TEST_HEART://心跳测试
                    if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                    {
                        Send_Data_Fixed(1, 0, 0, (int)_101_config.IEC101_CDIR_LK_CODE.FUN_TEST);
                        _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                    }
                    break;

                case (int)_101_config.IEC_101_Process_State.RESET_LOOPBACK://复位进程
                    if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                    {
                        Send_Data_Variable_ResetLoopback_Type105();
                        _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                    }
                    break;
            }
        }

        public void Jumpto_Send_Data(int i)
        {
            _101_SendOut_Data Send_Data = new _101_SendOut_Data();
            switch (i)
            {
                case _101_config.General_Call_Ti100:
                    _101_config.process_state = (int)_101_config.IEC_101_Process_State.INIT_CALL;
                    Send_Data.Send_Msg_Manage_bt();
                    break;

                case _101_config.Read_Clock_Ti103:
                    _101_config.process_state = (int)_101_config.IEC_101_Process_State.TIME_READ;
                    Send_Data.Send_Msg_Manage_bt();
                    break;

                case _101_config.Synchro_Clock_Ti103:
                    _101_config.process_state = (int)_101_config.IEC_101_Process_State.TIME_CHECK;
                    Send_Data.Send_Msg_Manage_bt();
                    break;

                case _101_config.Link_Test_Ti104:
                    _101_config.process_state = (int)_101_config.IEC_101_Process_State.TEST_LOOPBACK;
                    Send_Data.Send_Msg_Manage_bt();
                    break;

                case _101_config.Heart_Test:
                    _101_config.process_state = (int)_101_config.IEC_101_Process_State.TEST_HEART;
                    Send_Data.Send_Msg_Manage_bt();
                    break;

                case _101_config.Reset_Ti105:
                    _101_config.process_state = (int)_101_config.IEC_101_Process_State.RESET_LOOPBACK;
                    Send_Data.Send_Msg_Manage_bt();
                    break;

                default:
                    break;
            }
        }
    }
}
