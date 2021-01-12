using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SM
{
    class _101_receive_handle
    {
        private void Data_Handle_Variable_Type100(byte[] data)
        {
            int n = 6 + _101_config.ipaddr_size;
            byte SQ = data[n++];
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            n += _101_config.cotsize;
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            n += _101_config.asduaddr_size;
            int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
            n += _101_config.infaddr_size;
            int QOI = data[n++];

            if ((asduaddr != _101_config.asduaddr))
            {
                return;
            }
            if ((infaddr != 0) && (SQ != 0x01))
            {
                return;
            }
            if (QOI != (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP_CALL)
            {
                return;
            }

            _101_SendOut_Data Send_Data = new _101_SendOut_Data();
            if (_101_config._101_balance)
            {//平衡模式 回复确认报文 
                Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
            }
            switch (cot)
            {
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT_ACK:
                    _101_config.process = (int)_101_config.IEC_101_Process.GOING;
                    break;

                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_STOP_ACT_ACK:
                    _101_config.process = (int)_101_config.IEC_101_Process.END;//流程结束
                    break;

                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT_END:
                    if ((_101_config.process == (int)_101_config.IEC_101_Process.GOING) &&//初始化总召
                        (_101_config.process_state == (int)_101_config.IEC_101_Process_State.INIT_CALL))
                    {
                        _101_config.process = (int)_101_config.IEC_101_Process.END;//初始化总召结束
                        //_101_config.process_state = (int)_101_config.IEC_101_Process_State.TIME_CHECK;
                        //Send_Data.Send_Msg_Manage_bt();
                    }
                    break;

                default:
                    _101_config.process = (int)_101_config.IEC_101_Process.END;//流程结束
                    break;
            }

        }

        private void Data_Handle_Variable_YK(byte [] data)
        {
            int n = 6 + _101_config.ipaddr_size;
            byte SQ = data[n++];
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            n += _101_config.cotsize;
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            n += _101_config.asduaddr_size;
            int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
            n += _101_config.infaddr_size;
            byte SCODCO = data[n++];

            if (infaddr != YK.Get_Infaddr())//信息体地址错误
            {
                return;
            }
            if (SCODCO != YK.Get_YK_SCO_DCO())//命令词错误
            {
                return;
            }
            _101_config.process = (int)_101_config.IEC_101_Process.END;
            YK.select = true;
        }

        private void Data_Handle_Fixed_bt(byte[] data)
        {
            //_101_function _101_Function = new _101_function();
            _101_SendOut_Data Send_Data = new _101_SendOut_Data();
            byte crtl = data[1];
            int ipaddr = _101_function.octets_to_number(data, 2, _101_config.ipaddr_size);

            int prm = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_PRM) >> 6;
            if (prm == 1)
            {//从机为启动站发起报文，主站应回复确认
                int FC = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_FUN);
                switch (FC)
                {
                    case (int)_101_config.IEC101_CDIR_LK_CODE.FUN_AS://从机发送请求链路状态
                        if ((_101_config.process == (int)_101_config.IEC_101_Process.GOING) &&
                            (_101_config.process_state == (int)_101_config.IEC_101_Process_State.RESET_LINK))
                        {
                            Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_RSBS);
                        }
                        break;

                    case (int)_101_config.IEC101_CDIR_LK_CODE.FUN_RST_LK://从机发送复位远方链路
                        if ((_101_config.process == (int)_101_config.IEC_101_Process.GOING) &&
                            (_101_config.process_state == (int)_101_config.IEC_101_Process_State.RESET_LINK))
                        {
                            Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                            _101_config.process_state = (int)_101_config.IEC_101_Process_State.LINK_STAUS_END;
                        }
                        break;

                    case (int)_101_config.IEC_101_Process_State.TEST_HEART://主站收到从机的心跳测试报文 回复确认
                        if (_101_config.process == (int)_101_config.IEC_101_Process.END)
                        {
                            Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                        }
                        break;

                    default:
                        break;
                }
            }
            else if (prm == 0)
            {//从机为从站，该报文为从机发送的确认报文，对装置发起的报文进行了应答
                int FC = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_FUN);
                switch(FC)
                {
                    case (int)_101_config.IEC101_MDIR_LK_CODE.FUN_RSBS://从机回复链路状态
                        if ((_101_config.process == (int)_101_config.IEC_101_Process.GOING) &&
                            (_101_config.process_state == (int)_101_config.IEC_101_Process_State.LINK_STAUS))
                        {
                            _101_config.process_state = (int)_101_config.IEC_101_Process_State.RESET_LINK;
                            Send_Data.Send_Msg_Manage_bt();//回复复位远方链路
                        }
                        break;

                    case (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK:
                        switch ((int)_101_config.process_state)
                        {
                            case (int)_101_config.IEC_101_Process_State.RESET_LINK://当前在复位远方链路状态
                                break;

                            case (int)_101_config.IEC_101_Process_State.TEST_HEART://心跳测试状态
                                _101_config.process = (int)_101_config.IEC_101_Process.END;
                                break;

                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }
            }

        }

        private void Data_Handle_Variable(byte[] data)
        {
            _101_SendOut_Data Send_Data = new _101_SendOut_Data();
            int n = 4;
            byte crtl = data[n++];
            int prm = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_PRM) >> 6;
            int acd = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_FCB_ACD) >> 5;
            int dfc = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_FCV_DFC) >> 4;
            int cmd = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_FUN);

            int ipaddr = _101_function.octets_to_number(data,n,_101_config.ipaddr_size);
            n += _101_config.ipaddr_size;
            byte type = data[n];

            if (_101_config._101_balance)//平衡校验
            {
                if (cmd != (int)_101_config.IEC101_CDIR_LK_CODE.FUN_SD)
                    return;
            }
            else if (_101_config._101_unbalance)//非平衡
            {
                if (prm != 0x00)
                    return;
                if (cmd != (int)_101_config.IEC101_MDIR_LK_CODE.FUN_RSDT)
                    return;
            }
            switch (type)
            {
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP070:
                    if (_101_config._101_balance)
                    {//平衡模式 回复确认报文 
                        Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                    }
                    if ((_101_config.process == (int)_101_config.IEC_101_Process.GOING) &&
                        (_101_config.process_state == (int)_101_config.IEC_101_Process_State.LINK_STAUS_END))
                    {
                        _101_config.process = (int)_101_config.IEC_101_Process.END;//
                        _101_config.FCB = 1;
                        //_101_config.process_state = (int)_101_config.IEC_101_Process_State.INIT_CALL;
                        //Send_Data.Send_Msg_Manage_bt();
                    }
                    break;

                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP100:
                    Data_Handle_Variable_Type100(data);
                    break;

                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP103:
                    if (_101_config._101_balance)
                    {//平衡模式 回复确认报文 
                        Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                    }
                    _101_config.process = (int)_101_config.IEC_101_Process.END;
                    break;
                
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP104://测试命令
                    if (_101_config._101_balance)
                    {//平衡模式 回复确认报文 
                        Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                    }
                    _101_config.process = (int)_101_config.IEC_101_Process.END;
                    break;
               
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP105://复位进程
                    if (_101_config._101_balance)
                    {//平衡模式 回复确认报文 
                        Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                    }
                    _101_config.process = (int)_101_config.IEC_101_Process.END;
                    break;

                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP045:
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP046:
                    //添加处理遥控
                    Data_Handle_Variable_YK(data);
                    break;

                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP001://单点COS
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP003://双点COS
                    if (_101_config._101_balance)
                    {//平衡模式 回复确认报文 
                        Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                    }
                    if (_101_config.process == (int)_101_config.IEC_101_Process.GOING)
                    {
                        Send_Data.Send_Msg_Manage_bt();
                    }
                    break;

                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP030://单点SOE
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP031://双点SOE
                    if (_101_config._101_balance)
                    {//平衡模式 回复确认报文 
                        Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                    }
                    if (_101_config.process == (int)_101_config.IEC_101_Process.GOING)
                    {
                        Send_Data.Send_Msg_Manage_bt();
                    }
                    break;

                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP009://归一化遥测
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP011://标度化遥测
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP013://浮点型遥测
                    if (_101_config._101_balance)
                    {//平衡模式 回复确认报文 
                        Send_Data.Send_Data_Fixed(0, 0, 0, (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK);
                    }
                    if (_101_config.process == (int)_101_config.IEC_101_Process.GOING)
                    {
                        Send_Data.Send_Msg_Manage_bt();
                    }
                    break;

            }
        }

        private void Receive_Data_Handle(byte[] data)
        {
            
            if (data[0] == (byte)_101_config.IEC_101_Msg.fixed_state_code)
            {//添加固定帧处理
                if (_101_config._101_balance)
                {//添加平衡代码
                    Data_Handle_Fixed_bt(data);
                    return;
                }
                else if (_101_config._101_unbalance)
                { //添加非平衡代码
                    return;
                }
            }
            else if (data[0] == (byte)_101_config.IEC_101_Msg.variable_state_code)
            {//添加可变帧处理 
                Data_Handle_Variable(data);
            }
        }


        public void Receive_Data_Check(byte[] data)
        {
            _101_function _101_Function = new _101_function();
            if ((data[0] != (byte)_101_config.IEC_101_Msg.variable_state_code) && (data[0] != (byte)_101_config.IEC_101_Msg.fixed_state_code))
            {
                return;
            }
            try
            {
                int start = 0;
                while (start < data.Length)
                {
                    int n = _101_Function.Data_Check(data,start);
                    byte[] databuff = new byte[n];
                    for (int i = 0; i < n; i++, start++)
                    {
                        databuff[i] = data[start];
                    }
                    SerialPort_config.Analysis_msg(databuff, databuff.Length);//显示报文解析
                    Receive_Data_Handle(databuff);
                }
            }
            catch {
                return;
            }

        }
    }
}
