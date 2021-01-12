using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;//sleep

namespace SM
{
    class _101_function
    {
        public void Show_Data(byte[] data, int n, RichTextBox show)
        {
            string str = string.Empty;
            if (GetMsgDirection(data) == 0)//下行
            {
                show.SelectionColor = System.Drawing.Color.SeaGreen;
            }
            else if (GetMsgDirection(data) == 1)//上行
            {
                show.SelectionColor = System.Drawing.Color.DodgerBlue;
            }

            if (data[0] == (byte)_101_config.IEC_101_Msg.fixed_state_code)
            {
                if (_101_config._101_balance)
                {
                    str = Show_Data_Analysis_CA(data,n);
                    show.AppendText(str + "\r\n\r\n");
                    show.ScrollToCaret();
                }
                else if (_101_config._101_unbalance)
                { 
                    
                }
            }
            else if (data[0] == (byte)_101_config.IEC_101_Msg.variable_state_code)
            {//添加可变帧处理 
                str += Show_Data_Variable(data,n);
                show.AppendText(str + "\r\n\r\n");
                show.ScrollToCaret();
            }
        }

        private string Show_Data_Variable(byte[] data, int n)
        {
            string str = string.Empty;
            str += Show_Data_Analysis_CA(data,n);
            byte type = data[5 + _101_config.ipaddr_size];
            //show.AppendText("类型标识[" + type.ToString("x2") + "]:"+ type.ToString() +"        " + Get_Tpye_Str(type));
            str += ("类型标识[" + type.ToString("x2") + "]:" + type.ToString() + "        " + Get_Tpye_Str(type));
            switch (type)
            {
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP070:
                    str += ShowDataVariable_Type70(data);
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP100:
                    str += ShowDataVariable_Type100(data);
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP103:
                    str += ShowDataVariable_Type103(data);
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP104://测试命令
                    str += ShowDataVariable_Type104(data);
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP105://复位进程
                    str += ShowDataVariable_Type105(data);
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP045:
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP046:
                    //添加处理遥控
                    str += ShowDataVariable_Type45_46(data);
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP001://单点COS
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP003://双点COS
                    //添加处理COS
                    str += ShowDataVariable_COS(data);
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP030://单点SOE
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP031://双点SOE
                    //添加处理SOE
                     str += ShowDataVariable_SOE(data);
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP009://归一化遥测
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP011://标度化遥测
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP013://浮点型遥测
                    //添加处理遥测
                    str += ShowDataVariable_YC(data);
                    break;
                default:
                    break;
            }
            return str;
        }

        private string ShowDataVariable_Type45_46(byte[] data)
        {
            string str = string.Empty;
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));
            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;
            //信息体
            int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
            str += ("信息体地址[" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) + "]:" + infaddr.ToString() + 
                    "  点号:" + (infaddr - 24577).ToString() +"\r\n");
            n += _101_config.infaddr_size;
            //命令限定词
            int DCO = data[n++];
            str += Get_YK_SCO_DCO(DCO);

            return str;
        }

        private string ShowDataVariable_Type104(byte [] data)
        {
            string str = string.Empty;
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));
            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;
            //信息体
            int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
            str += ("信息体地址[" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) + "]:" + infaddr.ToString() + "\r\n");
            n += _101_config.infaddr_size;
            //测试固定字FBP
            str += ("测试固定字FBP:" + data[n++].ToString("X2") + " "+data[n++].ToString("X2") + "\r\n");

            return str;
        }

        private string ShowDataVariable_Type105(byte[] data)
        {
            string str = string.Empty;
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));
            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;
            //信息体
            int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
            str += ("信息体地址[" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) + "]:" + infaddr.ToString() + "\r\n");
            n += _101_config.infaddr_size;
            //复位进程命令限定词QRP
            int QRP = data[n++];
            str += ("复位进程命令限定词QRP[" + QRP.ToString("X2") + "]:" + Get_Type105_QRP(QRP) + "\r\n");

            return str;

        }

        private string ShowDataVariable_Type103(byte[] data)
        {
            string str = string.Empty;
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));

            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;
            //信息体
            int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
            str += ("信息体地址[" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) + "]:" + infaddr.ToString() + "\r\n");
            n += _101_config.infaddr_size;
            //7字节时间
            int milliseconds = data[n] + (data[n+1] << 8);
            n += 2;
            int minutes = data[n] & 0x3f;
            int IV = data[n++] & 0x80;
            int hours = data[n++] & 0x1f;
            int day = data[n++] & 0x1f;
            int months = data[n++] & 0x0f;
            int year = (data[n++] & 0x7f) + 2000;
            if (IV == 0)
            {
                str += ("IV：0         时间有效\r\n");
            }
            else 
            {
                str += ("IV：0         时间无效\r\n");
            }
            str += ("时间:" + year.ToString() + "年" + months.ToString() + "月" + day.ToString() + "日"
                            + hours.ToString() + "时" + minutes.ToString() + "分" + milliseconds.ToString() + "毫秒\r\n");

            return str;
        }

        private string ShowDataVariable_YC(byte[] data)
        {
            string str = string.Empty;
            byte type = data[5 + _101_config.ipaddr_size];
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            //show.AppendText("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));
            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            //show.AppendText("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            //show.AppendText("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;

            if ((SQ & 0x80) == 0x80)//vsq=1 顺序
            {
                int num = (SQ & 0x7f);
                int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
                n += _101_config.infaddr_size;
                if (type == (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP013)
                { //浮点型 4字节
                    for (int i = 0; i < num; i++)
                    {
                        //show.AppendText("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                        //               "  点号:" + (infaddr - 16385).ToString() + Get_YC_Str(data,n,0)  + "\r\n");
                        str += ("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                                "  点号:" + (infaddr - 16385).ToString() + Get_YC_Str(data,n,0)  + "\r\n");
                        infaddr++;
                        n += 5;
                    }
                }
                else
                {//2字节
                    for (int i = 0; i < num; i++)
                    {
                        //show.AppendText("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                        //                "  点号:" + (infaddr - 16385).ToString() + Get_YC_Str(data, n, 1) + "\r\n");
                        str  += ("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                                 "  点号:" + (infaddr - 16385).ToString() + Get_YC_Str(data, n, 1) + "\r\n");
                        infaddr++;
                        n += 3;
                    }
                }
            }
            else//vsq=0 不顺序
            {
                if (type == (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP013)
                { //浮点型 4字节
                    int num = (SQ & 0x7f);
                    int infaddr = 0;
                    for (int i = 0; i < num; i++)
                    {
                        infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
                        n += _101_config.infaddr_size;
                        //show.AppendText("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                        //                "  点号:" + (infaddr - 16385).ToString() + Get_YC_Str(data, n, 0) + "\r\n");
                        str += ("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                                "  点号:" + (infaddr - 16385).ToString() + Get_YC_Str(data, n, 0) + "\r\n");
                        n += 5;
                    }
                }
                else
                {//2字节
                    int num = (SQ & 0x7f);
                    int infaddr = 0;
                    for (int i = 0; i < num; i++)
                    {
                        infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
                        n += _101_config.infaddr_size;
                        //show.AppendText("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                        //                "  点号:" + (infaddr - 16385).ToString() + Get_YC_Str(data, n, 1) + "\r\n");
                        str += ("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                                "  点号:" + (infaddr - 16385).ToString() + Get_YC_Str(data, n, 1) + "\r\n");
                        n += 3;
                    }
                }
            }
            return str;
        }

        private string ShowDataVariable_COS(byte[] data)
        {
            string str = string.Empty;
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            //show.AppendText("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));

            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            //show.AppendText("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            //show.AppendText("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;

            if ((SQ & 0x80) == 0x80)//vsq=1 顺序
            {
                int num = (SQ & 0x7f);
                int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
                n += _101_config.infaddr_size;
                for (int i = 0; i < num; i++)
                {
                    //show.AppendText("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                    //                "  点号:" + (infaddr - 1).ToString() + Get_SIQ_Str_cossoe(data[n++],0) + "\r\n");
                    str += ("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                            "  点号:" + (infaddr - 1).ToString() + Get_SIQ_Str_cossoe(data[n++],0) + "\r\n");
                    infaddr++;
                }
            }
            else//vsq=0 不顺序
            {
                int infaddr = 0;
                int num = (SQ & 0x7f);
                for(int i = 0; i < num; i++)
                {
                    infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
                    n += _101_config.infaddr_size;
                    //show.AppendText("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                    //                "  点号:" + (infaddr - 1).ToString() + Get_SIQ_Str_cossoe(data[n++], 0) + "\r\n");
                    str += ("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                            "  点号:" + (infaddr - 1).ToString() + Get_SIQ_Str_cossoe(data[n++], 0) + "\r\n");
                }
            }

            return str;
        }

        private string ShowDataVariable_SOE(byte[] data)
        {
            string str = string.Empty;
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            //show.AppendText("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));

            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            //show.AppendText("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            //show.AppendText("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;

            if ((SQ & 0x80) == 0x80)//vsq=1 顺序
            {
                //show.AppendText("VSQ=1    错误\r\n");
                str += ("VSQ=1    错误\r\n");
            }
            else//vsq=0 不顺序
            {
                int infaddr = 0;
                int num = (SQ & 0x7f);
                for (int i = 0; i < num; i++)
                {
                    infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
                    n += _101_config.infaddr_size;
                    //show.AppendText("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                    //                "  点号:" + (infaddr - 1).ToString() + Get_SIQ_Str_cossoe(data[n++], 0) + "\r\n");
                    str += ("信息体地址0x" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) +
                            "  点号:" + (infaddr - 1).ToString() + Get_SIQ_Str_cossoe(data[n++], 0) + "\r\n");
                    int milliseconds = data[n] + (data[n + 1] << 8);
                    n += 2;
                    int minutes = data[n] & 0x3f;
                    int IV = data[n++] & 0x80;
                    int hours = data[n++] & 0x1f;
                    int day = data[n++] & 0x1f;
                    int months = data[n++] & 0x0f;
                    int year = (data[n++] & 0x7f) + 2000;
                    //show.AppendText("时间:" + year.ToString() + "年" + months.ToString() + "月" + day.ToString() + "日"
                    //                + hours.ToString() + "时" + minutes.ToString() + "分" + milliseconds.ToString() + "毫秒\r\n");
                    str += ("时间:" + year.ToString() + "年" + months.ToString() + "月" + day.ToString() + "日"
                            + hours.ToString() + "时" + minutes.ToString() + "分" + milliseconds.ToString() + "毫秒\r\n");

                }
            }
            return str;

        }

        private string ShowDataVariable_Type100(byte [] data)
        {
            string str = string.Empty;
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));

            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;
            
            //信息体
            int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
            str += ("信息体地址[" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) + "]:" + infaddr.ToString() + "\r\n");
            n += _101_config.infaddr_size;
            
            //召唤限定词QOI
            int QOI = data[n++];
            if(QOI == 0x14)
                str += ("召唤限定词[" + QOI.ToString("x2") + "]:" + QOI.ToString() + "    总召唤\r\n");
            else
                str += ("召唤限定词[" + QOI.ToString("x2") + "]:" + QOI.ToString() + "\r\n");

            return str;
        }

        private string ShowDataVariable_Type70(byte [] data)
        {
            string str = string.Empty;
            int n = 6 + _101_config.ipaddr_size;
            //sq
            byte SQ = data[n++];
            str += ("可变结构限定词[" + SQ.ToString("x2") + "]:" + SQ.ToString() + "   " + Get_SQ_Str(SQ));

            //cot
            int cot = _101_function.octets_to_number(data, n, _101_config.cotsize);
            str += ("传送原因[" + cot.ToString("x2") + "]:" + cot.ToString() + "         " + Get_COT_Str(cot));
            n += _101_config.cotsize;
            //asduaddr
            int asduaddr = _101_function.octets_to_number(data, n, _101_config.asduaddr_size);
            str += ("数据单元公共地址[" + Get_AsduAddr_Str(data, n) + "]:" + asduaddr.ToString() + "\r\n");
            n += _101_config.asduaddr_size;
            //信息体
            int infaddr = _101_function.octets_to_number(data, n, _101_config.infaddr_size);
            str += ("信息体地址[" + Get_InfAddr_Str(infaddr, _101_config.infaddr_size) + "]:" + infaddr.ToString() + "\r\n");
            n += _101_config.infaddr_size;
            //初始化进程命令限定词QRP
            int qrp = data[n];
            str += ("初始化原因[" + qrp.ToString("x2") + "]:" + qrp.ToString() + Get_COI_Str(qrp) + "\r\n");

            return str;
        }

        private string Get_YK_SCO_DCO(int data)
        {
            string str = string.Empty;
            int se = (data & 0x80) >> 7;
            int DCS = (data & 0x03);

            str += ("命令限定词:0x" + data.ToString("x2"));
            if (se == 1)
            {
                str += ("  选择  ");
            }
            else if (se == 0)
            {
                str += ("  执行  ");
            }

            if (DCS == 1)
            {
                str += ("  动作:分闸  ");
            }
            else if (DCS == 0)
            {
                str += ("  动作:合闸  ");
            }
            else
            {
                str += "  不允许  ";
            }

            str += "\r\n";
            return str;
        }

        private string Get_YC_Str(byte [] data,int start,int type)//type=0->4字节  type=1->2字节
        {
            string str = string.Empty;
            if (type == 0)
            {
                float f = BitConverter.ToSingle(data,start);
                start += 4;
                str += "  值:" + f.ToString() + "    " + Get_YC_QDS(data[start++]);
            }
            else if (type == 1)
            {
                byte buff_low = data[start++];
                byte buff_high = data[start++];
                if ((buff_high & 0x80) != 0)//补码
                {
                    int temp = (buff_high << 8) + (buff_low);
                    temp ^= 0xffff;
                    temp = (temp & 0x7fff) + 1;
                    float f = temp / 32767;
                    str += "  值:-" + f.ToString() + "    " + Get_YC_QDS(data[start++]);
                }
                else//正数
                { 
                    float f = ((((buff_high << 8) + (buff_low)) & 0x7fff) / 32767);
                    str += "  值:" + f.ToString() + "    " + Get_YC_QDS(data[start++]);
                }
            }
            return str;
        }

        private string Get_Tpye_Str(int type)
        {
            string str = string.Empty;
            switch (type)
            {
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP001:
                    str = "单点信息\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP002:
                    
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP003:
                    str = "单点信息\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP009:
                    str = "测量值，归一化值\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP011:
                    str = "测量值，标度化值\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP013:
                    str = "测量值，短浮点数\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP030:
                    str = "带CP56Time2a时标的单点信息\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP031:
                    str = "带CP56Time2a时标的双点信息\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP045:
                    str = "单点命令\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP046:
                    str = "双点命令\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP055:
                    str = "预置/激活参数命令\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP070:
                    str = "初始化结束\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP100:
                    str = "召唤命令\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP103:
                    str = "时钟同步及时钟读取\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP104:
                    str = "测试命令\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP105:
                    str = "复位进程\r\n";
                    break;
                case (int)_101_config.IEC101_ASDU_MDIR_TYP_CODE.EN_IEC101_ASDU_TYP108:
                    str = "读取参数\r\n";
                    break;
            }

            return str;
        }

        private string Get_Type105_QRP(int QRP)
        {
            string str = string.Empty;
            switch(QRP)
            {
                case 0:
                    str += "未采用";
                    break;
                case 1:
                    str += "进程的总复位";
                    break;
                case 2:
                    str += "复位事件缓存区等待处理的带时标信息";
                    break;
                default:
                    break;
            }
            return str;
        }

        private string Get_YC_QDS(int QDS)
        {
            int IV = (QDS & 0x80) >> 7;
            int NT = (QDS & 0x40) >> 6;
            int SB = (QDS & 0x20) >> 5;
            int BL = (QDS & 0x10) >> 4;
            int OV = (QDS & 0x01);
            string str = string.Empty;
            str += "品质描述词:" + QDS.ToString() + " ";

            if (OV == 1)
            {
                str += "溢出/";
            }
            else
            {
                str += "未溢出/";
            }

            if (BL == 1)
            {
                str += "被闭锁/";
            }
            else
            {
                str += "未被闭锁/";
            }

            if (SB == 1)
            {
                str += "被取代/";
            }
            else
            {
                str += "未被取代/";
            }

            if (NT == 1)
            {
                str += "非当前值/";
            }
            else
            {
                str += "当前值/";
            }

            if (IV == 1)
            {
                str += "有效/";
            }
            else
            {
                str += "无效/";
            }

            return str;

        }

        private string Get_COI_Str(int COI)
        {
            string str = string.Empty;
            int UI7 = COI & 0x7f;
            int BS1 = COI & 0x80;
            switch (BS1)
            {
                case 0:
                    str += "  未改变当地参数的初始化  ";
                    break;
                case 1:
                    str += "  改变当地参数的初始化  ";
                    break;
            }
            switch (UI7)
            {
                case 0:
                    str += "  当地电源合上  ";
                    break;
                case 1:
                    str += "  当地手动复位  ";
                    break;
                case 2:
                    str += "  远方复位  ";
                    break;
            }
            return str;
        }

        private string Get_SIQ_Str_cossoe(int SIQ,int i)//i=0->单点  i=1->双点
        {
            int IV = (SIQ & 0x80) >> 7;
            int NT = (SIQ & 0x40) >> 6;
            int SB = (SIQ & 0x20) >> 5;
            int BL = (SIQ & 0x10) >> 4;
            string str = string.Empty;
            if (i == 0)//单点信息
            {
                int SPI = (SIQ & 0x01);
                if (SPI == 1)
                {
                    str += "  状态:合  ";
                }
                else if (SPI == 0)
                {
                    str += "  状态:分  ";
                }
            }
            else if (i == 1)
            {
                int DPI = (SIQ & 0x03);
                switch (DPI)
                {
                    case 0:
                        str += "  状态:不确定  ";
                        break;
                    case 1:
                        str += "  状态:分  ";
                        break;
                    case 2:
                        str += "  状态:合  ";
                        break;
                    case 3:
                        str += "  状态:不确定  ";
                        break;
                }
            }

            str += "品质描述词:" + SIQ.ToString() + " ";

            if (BL == 1){
                str += "被闭锁/";
            }
            else{
                str += "未被闭锁/";
            }

            if (SB == 1)
            {
                str += "被取代/";
            }
            else
            {
                str += "未被取代/";
            }

            if (NT == 1)
            {
                str += "当前值/";
            }
            else
            {
                str += "非当前值/";
            }

            if (IV == 1)
            {
                str += "有效/";
            }
            else
            {
                str += "无效/";
            }

            return str;
        }

       /* private string Get_InfAddr_Str(byte[] data, int start)
        {
            string str = string.Empty;
            if (_101_config.infaddr_size == 1)
            {
                str = data[start].ToString("X2");
            }
            else if (_101_config.infaddr_size == 2)
            {
                for (int i = 0; i < 2; i++)//byte 数据转成字符串
                {
                    str += data[start].ToString("X2");
                    start++;
                    if (i == 0)
                        str += " ";
                }
            }
            return str;
        }*/

        private string Get_InfAddr_Str(int data, int size)
        {
            string str = string.Empty;
            if (size == 1)
            {
                str = (data & 0x00ff).ToString("X2");
            }
            else if (size == 2)
            {
                str += ((data & 0xff00) >> 8).ToString("X2");
                str += (data & 0x00ff).ToString("X2");
            }
            return str;
        }

        private string Get_AsduAddr_Str(byte [] data,int start)
        {
            string str = string.Empty;
            if (_101_config.asduaddr_size == 1)
            {
                str = data[start].ToString("X2");
            }
            else if (_101_config.asduaddr_size == 2) 
            {
                for (int i = 0; i < 2; i++)//byte 数据转成字符串
                {
                    str += data[start].ToString("X2");
                    start++;
                    if (i == 0)
                        str += " ";
                }
            }
            return str;
        }

        private string Get_COT_Str(int cot)
        {
            string str = string.Empty;
            switch (cot)
            {
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_NULL:
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_CYC:
                    str = "周期\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_BACK:
                    str = "背景扫描\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_SPONT:
                    str = "突发（自发）\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_INIT:
                    str = "初始化结束\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP:
                    str = "请求或被请求\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT:
                    str = "激活\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT_ACK:
                    str = "激活确认\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_STOP_ACT:
                    str = "停止激活\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_STOP_ACT_ACK:
                    str = "停止激活确认\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_ACT_END:
                    str = "激活结束\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP_CALL:
                    str = "总召唤\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP_UnknownASDU:
                    str = "未知类型标识\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP_UnknownCOT:
                    str = "未知传送原因\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP_UnknownCOA:
                    str = "未知的应用服务数据单元公共地址\r\n";
                    break;
                case (int)_101_config._IEC101_ASDU_COT_CODE_.EN_IEC101_ASDU_COT_RESP_UnknownINF:
                    str = "未知的信息对象地址\r\n";
                    break;
            }
            return str;
        }

        private string Get_SQ_Str(byte data)
        {
            string str = string.Empty;
            int sq = (data & 0x80) >> 7;
            string num = (data & 0x7f).ToString();
            if (sq == 1)
            {
                str = "SQ=1  信息体顺序，信息体个数:" + num + "\r\n";
            }
            else if (sq == 0)
            {
                str = "SQ=0  信息体非顺序，信息体个数:" + num + "\r\n";
            }
            else
            {
                str = "SQ错误！";
            }
            return str;
        }

        private string Get_Data_Str(byte[] data,int n)
        {
            string DataStr = string.Empty;
            for (int i = 0; i < n; i++)//byte 数据转成字符串
            {
                DataStr += data[i].ToString("X2") + " ";
            }
            return DataStr;
        }

        private int GetMsgDirection(byte[] data)
        {
            int crtl = 0;
            if (data[0] == (int)_101_config.IEC_101_Msg.fixed_state_code)
            {
                crtl = data[1];
            }
            else if (data[0] == (int)_101_config.IEC_101_Msg.variable_state_code)
            {
                crtl = data[4];
            }
            int dir = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_MAIN) >> 7;

            return dir;
        }

        private string Show_Data_Analysis_CA(byte[] data,int n)
        {
            string str = string.Empty;
            byte crtl = 0;
            int ipaddr = 0;
            if (data[0] == (int)_101_config.IEC_101_Msg.fixed_state_code)
            {
                crtl = data[1];
                ipaddr = _101_function.octets_to_number(data, 2, _101_config.ipaddr_size);
            }
            else if (data[0] == (int)_101_config.IEC_101_Msg.variable_state_code)
            {
                crtl = data[4];
                ipaddr = _101_function.octets_to_number(data, 5, _101_config.ipaddr_size);
            }
            string time = System.DateTime.Now.ToString("[HH:mm:ss:fff]");

            string DataStr = Get_Data_Str(data,n);
            str += time;
            //dir
            int dir = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_MAIN) >> 7;
            if (dir == 0)
            {
                str += ("  下行\r\n" + DataStr + "\r\n");
            }
            else if (dir == 1)
            {
                str += ("  上行\r\n" + DataStr + "\r\n");
            }
            //链路地址
            str += ("链路地址：" + ipaddr + "\r\n");
            str += ("dir=" + dir + "\r\n");

            //prm
            int prm = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_PRM) >> 6;
            if (prm == 1)
            {
                str += ("prm=" + prm + "                  报文来自启动站\r\n");
            }
            else if (prm == 0)
            {
                str += ("prm=" + prm + "                  报文来自从动站\r\n");
            }
            else
            {
                str += ("prm=" + prm + "\r\n");
            }

            //FCB
            int fcb = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_FCB_ACD) >> 5;
            int fcv = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_FCV_DFC) >> 4;
            if ((dir == 0) && (fcv == 1))
            { 
                str += ("FCB=" + fcb + "\r\n");
            }
            //功能码
            int fc = (crtl & (int)_101_config.IEC101_CONTROL_DEF.CN_CTL_LK_FUN);
            str += ("链路功能码[" + fc.ToString("X2") + "]：" + fc + "  " + Get_FC_Str(prm, fc));

            return str;
        }

        private string Get_FC_Str(int prm,int fc)
        {
            string str = string.Empty;
            if (prm == 1)//报文来自启动站
            {
                switch (fc)
                {
                    case (int)_101_config.IEC101_CDIR_LK_CODE.FUN_RST_LK://复位链路
                        str = "    复位远方链路\r\n";
                        break;
                    case (int)_101_config.IEC101_CDIR_LK_CODE.FUN_RST_CU://复位用户进程
                        str = "    复位用户进程\r\n";
                        break;
                    case (int)_101_config.IEC101_CDIR_LK_CODE.FUN_TEST://链路测试功能
                        str = "    链路测试\r\n";
                        break;
                    case (int)_101_config.IEC101_CDIR_LK_CODE.FUN_SD://确认用户数据
                        str = "    发送、确认用户数据\r\n";
                        break;
                    case (int)_101_config.IEC101_CDIR_LK_CODE.FUN_GD://发送无回应用户数据
                        str = "    发送、无回答用户数据\r\n";
                        break;
                    case (int)_101_config.IEC101_CDIR_LK_CODE.FUN_AS://请求链路连接
                        str = "    请求链路连接\r\n";
                        break;
                    default:
                        break;
                }
            }
            else if (prm == 0)//报文来自从动站
            {
                switch (fc)
                {
                    case (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACK:
                        str = "    链路肯定确认\r\n";
                        break;
                    case (int)_101_config.IEC101_MDIR_LK_CODE.FUN_ACKBS:
                        str = "    否定确认\r\n";
                        break;
                    case (int)_101_config.IEC101_MDIR_LK_CODE.FUN_RSDT:
                        str = "    有请求的用户数据\r\n";
                        break;
                    case (int)_101_config.IEC101_MDIR_LK_CODE.FUN_RSNO:
                        str = "    无请求的用户数据\r\n";
                        break;
                    case (int)_101_config.IEC101_MDIR_LK_CODE.FUN_RSBS:
                        str = "    链路完好\r\n";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                str = "链路功能码错误！";
            }
            return str;
        }

        public static void number_to_octets(ref byte[] data,int start,int ip,int size)
        {
            if ((ip == 0) || (size == 0))
                return;
            for (int i = 0; i < size; i++)
            {
                data[start] = (byte)((0xFF << (8 * i) & ip) >> (8 * i));
                start++;
            }
        }

        public static int octets_to_number(byte [] data,int start,int size)
        {
            int ip = 0;
            if (size == 0)
                return -1;
            for (int i = 0; i < size ; i++)
            {
                ip += (data[start] << (8 * i));
                start++;
            }
            return ip;
        }

        //报文检测
        public int Data_Check(byte [] data,int start) 
        {
            int len;
            if (data[start] == (byte)_101_config.IEC_101_Msg.fixed_state_code)//固定帧
            {
                if (data[start + _101_config.ipaddr_size + 4 - 1] != (byte)_101_config.IEC_101_Msg.end_code)
                    return -1;
                if (data.Length < (_101_config.ipaddr_size + 4))
                    return -1;
                if (Check_CS(data, start + 1, _101_config.ipaddr_size + 1) != (int)data[start + _101_config.ipaddr_size + 2])
                    return -1;
                len = _101_config.ipaddr_size + 4;
            }
            else//可变帧
            {
                if (data[start + 3] != (byte)_101_config.IEC_101_Msg.variable_state_code)
                    return -1;
                if (data[start + 1] != data[start + 2])
                    return -1;
                else
                     len = (int)data[start + 1];
                if (data[start + len + 6 - 1] != (byte)_101_config.IEC_101_Msg.end_code)
                    return -1;
                if (data.Length < len + 6)
                    return -1;
                if (Check_CS(data, start + 4, len) != (int)data[start + len + 6 - 2])
                    return -1;
                len += 6;
            }
            return len;
        }

        public static byte Data_Get_Cctrl(int PRM, int FCB, int FCV, int CMD)
        {
            byte crtl = 0;
            int aof, dov, dir = 0;
            if (PRM == 1)
            {
                if (FCV == 1)
                {
                    dov = (int)_101_config.IEC_101_Msg.SET_FCV_DFC;
                    if (_101_config.FCB == 1)
                    {
                        aof = (int)_101_config.IEC_101_Msg.SET_FCB_ACD;
                        _101_config.FCB = 0;
                    }
                    else
                    {
                        aof = 0;
                        _101_config.FCB = 1;
                    }
                }
                else
                {
                    aof = 0;
                    dov = 0;
                }
                crtl |= (byte)(dir | (int)_101_config.IEC_101_Msg.STATR_PRM | aof | dov | CMD);
            }
            else
            {
                if (FCB == 1)
                    aof = (int)_101_config.IEC_101_Msg.SET_FCB_ACD;
                else
                    aof = 0;
                if (FCV == 1)
                    dov = (int)_101_config.IEC_101_Msg.SET_FCV_DFC;
                else
                    dov = 0;
                crtl |= (byte)(dir | (int)_101_config.IEC_101_Msg.ANSWER_PRM | aof | dov | CMD);
            }
            return crtl;
        }

        //检查校验和
        public int Check_CS(byte[] data,int start,int len)
        {
            int cs = 0;
            for (int i = 0; i < len; i++)
            {
                cs += data[start];
                start++;
            }
            cs &= 0xff;
            return cs;
        }

        public byte Get_CS(byte [] data,int start,int len)
        {
            byte cs = 0;
            for (int i = 0; i < (len - 1); i++)
            {
                cs += data[start];
                start++;
            }
            return cs;
        }

        //保存数据
        public void Save_Data(string path,string Save_data)
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(path, true))
                {
                    sw.Write(Save_data);
                    sw.Close();
                }
            }
            catch
            {
                MessageBox.Show("保存失败！", "提示", MessageBoxButtons.OK);
            }
        }

    }
}
