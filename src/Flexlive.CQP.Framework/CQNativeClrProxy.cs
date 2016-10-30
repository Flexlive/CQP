using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 采用本地C++/Clr方式的数据代理类。
    /// </summary>
    internal class CQNativeClrProxy
    {
        /// <summary>
        /// 接收数据。
        /// </summary>
        public void ReceiveMessage(string msg)
        {
            //接收数据处理线程  
            try
            {
                CQ.ProxyType = CQProxyType.NativeClr;

                CQAppContainer.GetInstance();

                Thread thread = new Thread(new ParameterizedThreadStart(this.AnalyzeMessage));
                thread.IsBackground = true;
                thread.Start(msg);
            }
            catch
            {

            }
        }

        /// <summary>
        /// 解析消息。
        /// </summary>
        /// <param name="data"></param>
        private void AnalyzeMessage(object data)
        {
            //long qq = CQAPI.GetLoginQQ(CQAPI.GetAuthCode());
            //int token = CQAPI.GetCsrfToken(CQAPI.GetAuthCode());
            //string name = CQAPI.GetLoginNick(CQAPI.GetAuthCode());
            //string cookie = CQAPI.GetCookies(CQAPI.GetAuthCode());
            //string folder = CQAPI.GetCQAppFolder(CQAPI.GetAuthCode());

            

            //string info = CQAPI.GetStrangerInfo(CQAPI.GetAuthCode(), 447903278, 0);

            string[] args = data.ToString().Split(new char[] { '|' });

            if (args.Length == 12)
            {
                string eventType = args[0]; //1事件类型
                int subType = Convert.ToInt32(args[1]); //2子类型
                int sendTime = String.IsNullOrEmpty(args[2]) ? 0 : Convert.ToInt32(args[2]); //3发送时间(时间戳)
                long fromGroup = String.IsNullOrEmpty(args[3]) ? 0 : Convert.ToInt64(args[3]); //4来源群号
                long fromDiscuss = String.IsNullOrEmpty(args[4]) ? 0 : Convert.ToInt64(args[4]); //5来源讨论组
                long fromQQ = String.IsNullOrEmpty(args[5]) ? 0 : Convert.ToInt64(args[5]); //6来源QQ
                string fromAnonymous = args[6]; //7来源匿名者
                long beingOperateQQ = String.IsNullOrEmpty(args[7]) ? 0 : Convert.ToInt64(args[7]); //8被操作QQ
                string msg = args[8].Replace("$内容分割$", "|"); //9消息内容
                int font = String.IsNullOrEmpty(args[9]) ? 0 : Convert.ToInt32(args[9]); //10字体
                string responseFlag = args[10]; //11反馈标识(处理请求用)
                string file = args[11]; //12上传文件信息

                switch (eventType)
                {
                    case "PrivateMessage": //私聊消息
                        CQLogger.GetInstance().AddLog(String.Format("[↓][私聊] QQ：{0} {1}", fromQQ, msg));
                        break;
                    case "GroupMessage": //群消息
                        CQLogger.GetInstance().AddLog(String.Format("[↓][群聊] 群：{0} QQ：{1} {2}", fromGroup, fromQQ, msg));
                        break;
                    case "DiscussMessage": //讨论组消息
                        CQLogger.GetInstance().AddLog(String.Format("[↓][讨论] 组：{0} QQ：{1} {2}", fromDiscuss, fromQQ, msg));
                        break;
                    case "GroupUpload": //群文件上传事件
                        CQLogger.GetInstance().AddLog(String.Format("[↓][上传] 群：{0} QQ：{1} {2}", fromGroup, fromQQ, file));
                        break;
                    case "GroupAdmin": //群事件-管理员变动
                        CQLogger.GetInstance().AddLog(String.Format("[↓][管理] 群：{0} QQ：{1}", fromGroup, beingOperateQQ));
                        break;
                    case "GroupMemberDecrease": //群事件-群成员减少
                        CQLogger.GetInstance().AddLog(String.Format("[↓][减员] 群：{0} QQ：{1} OperateQQ：{2}", fromGroup, fromQQ, beingOperateQQ));
                        break;
                    case "GroupMemberIncrease": //群事件-群成员增加
                        CQLogger.GetInstance().AddLog(String.Format("[↓][增员] 群：{0} QQ：{1} OperateQQ：{2}", fromGroup, fromQQ, beingOperateQQ));
                        break;
                    case "FriendAdded": //好友事件-好友已添加
                        CQLogger.GetInstance().AddLog(String.Format("[↓][加友] QQ：{0}", fromQQ));
                        break;
                    case "RequestAddFriend": //请求-好友添加
                        CQLogger.GetInstance().AddLog(String.Format("[↓][请友] QQ：{0} {1}", fromQQ, msg));
                        break;
                    case "RequestAddGroup": //请求-群添加
                        CQLogger.GetInstance().AddLog(String.Format("[↓][请群] 群：{0} QQ：{1} {2}", fromGroup, fromQQ, msg));
                        break;
                }
            }

            CQMessageAnalysis.Analyze(data.ToString());
        }
    }
}
