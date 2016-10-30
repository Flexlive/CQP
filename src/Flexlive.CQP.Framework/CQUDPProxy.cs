using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Flexlive.CQP.Framework.Utils;

namespace Flexlive.CQP.Framework
{
    delegate void ThreadCallBackDelegate(string msg);

    /// <summary>
    /// CQ易语言程序代理类。
    /// </summary>
    public class CQUDPProxy
    {
        /// <summary>
        /// 声明对象多线程同步访问锁引用。
        /// </summary>
        [NonSerialized]
        private static Object _syncRoot = null;

        /// <summary>
        /// 线程回调委托。
        /// </summary>
        private ThreadCallBackDelegate _callback = null;

        /// <summary>
        /// 线程同步等待锁对象。
        /// </summary>
        private static AutoResetEvent myResetEvent = new AutoResetEvent(false);

        /// <summary>
        /// 定义接收到的消息字符串。
        /// </summary>
        private string _strMessage = String.Empty;


        /// <summary>
        /// 单例对象。
        /// </summary>
        private static CQUDPProxy instance = null;

        private EndPoint RemotePoint;
        private Socket mySocket;
        private bool RunningFlag = false; 

        /// <summary>
        /// 创建一个实例。
        /// </summary>
        /// <returns>一个 <see cref="CQUDPProxy"/> 实例。</returns>
        public static CQUDPProxy GetInstance()
        {
            if(instance == null)
            {
                instance = new CQUDPProxy();
            }

            return instance;
        }

        /// <summary>
        /// 隐藏默认构造函数。
        /// </summary>
        private CQUDPProxy()
        {
            //初始化对象多线程同步访问锁。
            Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);

            this._callback = AnalyzeMessage;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Start()
        {
            //初始化插件容器。
            CQAppContainer.GetInstance();

            string ipAddress = "127.0.0.1";
            int port = 18139;

            string folder = Path.Combine(CQ.GetCQAppFolder(), "app");
            folder = Path.Combine(folder, "cc.flexlive.cqeproxy");

            if (Directory.Exists(folder))
            {
                string iniFile = Path.Combine(folder, "cc.flexlive.cqeproxy.ini");

                if (File.Exists(iniFile))
                {
                    ipAddress = IniFileHelper.GetStringValue(iniFile, "代理配置", "服务器地址", "127.0.0.1");
                    string strPort = IniFileHelper.GetStringValue(iniFile, "代理配置", "服务器端口", "18139");
                    port = Convert.ToInt32(strPort);
                }
            }

            IPAddress ip = IPAddress.Parse(ipAddress);
            int listenPort = port;

            IPEndPoint listenIPEndPoint = new IPEndPoint(ip, listenPort);

            //定义网络类型，数据连接类型和网络协议UDP  
            this.mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //绑定网络地址  
            this.mySocket.Bind(listenIPEndPoint);

            //得到客户机IP  
            int clientPort = 18131;
            IPEndPoint ipep = new IPEndPoint(ip, clientPort);
            RemotePoint = (EndPoint)(ipep);

            //启动一个新的线程，执行方法this.ReceiveHandle，  
            //以便在一个独立的进程中执行数据接收的操作  
            RunningFlag = true;
            Thread thread = new Thread(new ThreadStart(this.ReceiveMessage));
            thread.IsBackground = true;
            thread.Start();  
        }

        /// <summary>
        /// 接收数据。
        /// </summary>
        private void ReceiveMessage()
        {
            //接收数据处理线程  
            string msg;
            byte[] data = new byte[4096];
            while (RunningFlag)
            {
                if (mySocket == null || mySocket.Available < 1)
                {
                    Thread.Sleep(200);
                    continue;
                }

                try
                {
                    //跨线程调用控件  
                    //接收UDP数据报，引用参数RemotePoint获得源地址  
                    int rlen = mySocket.ReceiveFrom(data, ref RemotePoint);
                    msg = Encoding.Default.GetString(data, 0, rlen);

                    if (msg.StartsWith("GroupMemberResult") ||
                        msg.StartsWith("GetLoginQQResult") ||
                        msg.StartsWith("GetLoginNameResult") ||
                        msg.StartsWith("GetCookiesResult") ||
                        msg.StartsWith("GetCsrfTockenResult") ||
                        msg.StartsWith("GetGroupListResult") ||
                        msg.StartsWith("GetGroupMemberListResult"))
                    {
                        this._strMessage = msg;
                        myResetEvent.Set();
                    }
                    else
                    {
                        Thread thread = new Thread(new ParameterizedThreadStart(this.AnalyzeMessage));
                        thread.IsBackground = true;
                        thread.Start(msg);
                    }
                }
                catch
                {

                }
            }
        }

        /// <summary>
        /// 解析消息。
        /// </summary>
        /// <param name="data"></param>
        private void AnalyzeMessage(object data)
        {
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

        /// <summary>
        /// 发送数据。
        /// </summary>
        /// <param name="message">要发送的数据内容。</param>
        internal void SendMessage(string message)
        {
            byte[] data = Encoding.Default.GetBytes(message);

            this.mySocket.SendTo(data, data.Length, SocketFlags.None, RemotePoint);
        }

        /// <summary>
        /// 获取群成员列表，阻塞线程，等待客户端响应（有风险，待测试）。
        /// </summary>
        internal CQGroupMemberInfo GetGroupMemberInfo(string message)
        {
            byte[] data = Encoding.Default.GetBytes(message);
            this.mySocket.SendTo(data, data.Length, SocketFlags.None, RemotePoint);

            myResetEvent.WaitOne(2000);

            try
            {
                CQGroupMemberInfo info = CQMessageAnalysis.AnalyzeGroupMember(this._strMessage);
                this._strMessage = String.Empty;
                return info;
            }
            catch
            {
                return new CQGroupMemberInfo();
            }
        }

        /// <summary>
        /// 从酷Q获取文本型返回数据。
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        internal string GetStringMessage(string message)
        {
            byte[] data = Encoding.Default.GetBytes(message);
            this.mySocket.SendTo(data, data.Length, SocketFlags.None, RemotePoint);

            myResetEvent.WaitOne(2000);

            try
            {
                string[] result = this._strMessage.Split(new char[] { '|' });
                this._strMessage = String.Empty;
                return result[1];
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
