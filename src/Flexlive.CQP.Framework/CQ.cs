using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using Flexlive.CQP.Framework.Utils;
using System.Text;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// CQ提供的基本方法静态类。
    /// </summary>
    public static class CQ
    {
        /// <summary>
        /// 声明对象多线程同步访问锁引用。
        /// </summary>
        [NonSerialized]
        private static Object _syncRoot = null;

        /// <summary>
        /// 酷Q C#代理类型。
        /// </summary>
        private static CQProxyType proxyType = CQProxyType.NativeClr;

        /// <summary>
        /// 代理类型。
        /// </summary>
        public static CQProxyType ProxyType
        {
            get
            {
                return CQ.proxyType;
            }
            set
            {
                CQ.proxyType = value;
            }
        }

        /// <summary>
        /// 存储群成员的缓存。
        /// </summary>
        private static Dictionary<long, Dictionary<long, CQGroupMemberInfo>> _dicCache = null;

        /// <summary>
        /// 静态构造。
        /// </summary>
        static CQ()
        {
            _dicCache = new Dictionary<long, Dictionary<long, CQGroupMemberInfo>>();
            _syncRoot = new object();
        }


        #region CQ码

        /// <summary>
        /// 获取 @指定QQ 的操作代码。
        /// </summary>
        /// <param name="qqNumber">指定的QQ号码。
        /// <para>当该参数为-1时，操作为 @全部成员。</para></param>
        /// <returns>CQ @操作代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_At")]
        public static string CQ码_At(long qqNumber)
        {
            return "[CQ:at,qq=" + (qqNumber == -1 ? "all" : qqNumber.ToString()) + "]";
        }

        /// <summary>
        /// 获取 @指定QQ 的操作代码。
        /// </summary>
        /// <param name="qqNumber">指定的QQ号码。
        /// <para>当该参数为-1时，操作为 @全部成员。</para></param>
        /// <returns>CQ @操作代码。</returns>
        public static string CQCode_At(long qqNumber)
        {
            return "[CQ:at,qq=" + (qqNumber == -1 ? "all" : qqNumber.ToString()) + "]";
        }

        /// <summary>
        /// 获取 指定的emoji表情代码。
        /// </summary>
        /// <param name="id">emoji表情索引ID。</param>
        /// <returns>CQ emoji表情代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_Emoji")]
        public static string CQ码_emoji(int id)
        {
            return "[CQ:emoji,id=" + id + "]";
        }

        /// <summary>
        /// 获取 指定的emoji表情代码。
        /// </summary>
        /// <param name="id">emoji表情索引ID。</param>
        /// <returns>CQ emoji表情代码。</returns>
        public static string CQCode_Emoji(int id)
        {
            return "[CQ:emoji,id=" + id + "]";
        }

        /// <summary>
        /// 获取 指定的表情代码。
        /// </summary>
        /// <param name="id">表情索引ID。</param>
        /// <returns>CQ 表情代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_Face")]
        public static string CQ码_表情(int id)
        {
            return "[CQ:face,id=" + id + "]";
        }

        /// <summary>
        /// 获取 指定的表情代码。
        /// </summary>
        /// <param name="id">表情索引ID。</param>
        /// <returns>CQ 表情代码。</returns>
        public static string CQCode_Face(int id)
        {
            return "[CQ:face,id=" + id + "]";
        }

        /// <summary>
        /// 获取 窗口抖动代码。
        /// </summary>
        /// <returns>CQ 窗口抖动代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_Shake")]
        public static string CQ码_窗口抖动()
        {
            return "[CQ:shake]";
        }

        /// <summary>
        /// 获取 窗口抖动代码。
        /// </summary>
        /// <returns>CQ 窗口抖动代码。</returns>
        public static string CQCode_Shake()
        {
            return "[CQ:shake]";
        }
        
        /// <summary>
        /// 获取 匿名代码。
        /// </summary>
        /// <param name="ignore">是否不强制。</param>
        /// <returns>CQ 匿名代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_Anonymous")]
        public static string CQ码_匿名(bool ignore = false)
        {
            return "[CQ:anonymous" + (ignore ? ",ignore=true" : "") + "]";
        }
        
        /// <summary>
        /// 获取 匿名代码。
        /// </summary>
        /// <param name="ignore">是否不强制。</param>
        /// <returns>CQ 匿名代码。</returns>
        public static string CQCode_Anonymous(bool ignore = false)
        {
            return "[CQ:anonymous" + (ignore ? ",ignore=true" : "") + "]";
        }

        /// <summary>
        /// 获取 发送图片代码。
        /// </summary>
        /// <param name="fileName">图片路径。</param>
        /// <returns>CQ 发送图片代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_Image")]
        public static string CQ码_图片(string fileName)
        {
            return "[CQ:image,file=" + fileName + "]";
        }

        /// <summary>
        /// 获取 发送图片代码。
        /// </summary>
        /// <param name="fileName">图片路径。</param>
        /// <returns>CQ 发送图片代码。</returns>
        public static string CQCode_Image(string fileName)
        {
            return "[CQ:image,file=" + fileName + "]";
        }

        /// <summary>
        /// 获取 发送音乐代码。
        /// </summary>
        /// <param name="id">音乐索引ID。</param>
        /// <returns>CQ 发送音乐代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_Music")]
        public static string CQ码_音乐(int id)
        {
            return "[CQ:music,id=" + id + "]";
        }

        /// <summary>
        /// 获取 发送音乐代码。
        /// </summary>
        /// <param name="id">音乐索引ID。</param>
        /// <returns>CQ 发送音乐代码。</returns>
        public static string CQCode_Music(int id)
        {
            return "[CQ:music,id=" + id + "]";
        }

        /// <summary>
        /// 获取 发送语音代码。
        /// </summary>
        /// <param name="fileName">语音文件路径。</param>
        /// <returns>CQ 发送语音代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_Record")]
        public static string CQ码_语音(string fileName)
        {
            return "[CQ:record,file=" + fileName + "]";
        }

        /// <summary>
        /// 获取 发送语音代码。
        /// </summary>
        /// <param name="fileName">语音文件路径。</param>
        /// <returns>CQ 发送语音代码。</returns>
        public static string CQCode_Record(string fileName)
        {
            return "[CQ:record,file=" + fileName + "]";
        }

        /// <summary>
        /// 获取 链接分享代码。
        /// </summary>
        /// <param name="url">链接地址。</param>
        /// <param name="title">标题。</param>
        /// <param name="content">内容。</param>
        /// <param name="imageUrl">图片地址。</param>
        /// <returns>CQ 链接分享代码。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：CQCode_ShareLink")]
        public static string CQ码_链接分享(string url, string title, string content, string imageUrl)
        {
            return String.Format("[CQ:share,url={0},title={1},content={2},image={3}]", url, title, content, imageUrl);
        }

        /// <summary>
        /// 获取 链接分享代码。
        /// </summary>
        /// <param name="url">链接地址。</param>
        /// <param name="title">标题。</param>
        /// <param name="content">内容。</param>
        /// <param name="imageUrl">图片地址。</param>
        /// <returns>CQ 链接分享代码。</returns>
        public static string CQCode_ShareLink(string url, string title, string content, string imageUrl)
        {
            return String.Format("[CQ:share,url={0},title={1},content={2},image={3}]", url, title, content, imageUrl);
        }

        #endregion

        #region 发送与接收

        /// <summary>
        /// 发送群消息。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="message">群消息内容。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SendGroupMessage")]
        public static void 发送群消息(long groupNumber, string message)
        {
            SendGroupMessage(groupNumber, message);
        }

        /// <summary>
        /// 发送群消息。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="message">群消息内容。</param>
        public static void SendGroupMessage(long groupNumber, string message)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][群聊] 群：{0} {1}", groupNumber, message));

            if (ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SendGroupMessage|{0}|{1}", groupNumber, message.Replace("|", "$内容分割$"));
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SendGroupMessage(CQAPI.GetAuthCode(), groupNumber, message);
            }
        }

        /// <summary>
        /// 发送私聊消息。
        /// </summary>
        /// <param name="qqNumber">QQ号码。</param>
        /// <param name="message">私聊消息内容。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SendPrivateMessage")]
        public static void 发送私聊消息(long qqNumber, string message)
        {
            SendPrivateMessage(qqNumber, message);
        }

        /// <summary>
        /// 发送私聊消息。
        /// </summary>
        /// <param name="qqNumber">QQ号码。</param>
        /// <param name="message">私聊消息内容。</param>
        public static void SendPrivateMessage(long qqNumber, string message)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][私聊] QQ：{0} {1}", qqNumber, message));

            if (ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SendPrivateMessage|{0}|{1}", qqNumber, message.Replace("|", "$内容分割$"));
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SendPrivateMessage(CQAPI.GetAuthCode(), qqNumber, message);
            }
        }

        /// <summary>
        /// 发送讨论组消息。
        /// </summary>
        /// <param name="discussNumber">讨论组号码。</param>
        /// <param name="message">论组消息内容。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SendDiscussMessage")]
        public static void 发送讨论组消息(long discussNumber, string message)
        {
            SendDiscussMessage(discussNumber, message);
        }

        /// <summary>
        /// 发送讨论组消息。
        /// </summary>
        /// <param name="discussNumber">讨论组号码。</param>
        /// <param name="message">论组消息内容。</param>
        public static void SendDiscussMessage(long discussNumber, string message)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][讨论] QQ：{0} {1}", discussNumber, message));

            if (ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SendDiscussMessage|{0}|{1}", discussNumber, message.Replace("|", "$内容分割$"));
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SendDiscussMessage(CQAPI.GetAuthCode(), discussNumber, message);
            }
        }
        
        /// <summary>
        /// 发送赞。
        /// </summary>
        /// <param name="qqNumber">被操作的QQ。</param>
        /// <param name="count">发赞次数。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SendPraise")]
        public static void 发送赞(long qqNumber, int count = 1)
        {
            SendPraise(qqNumber, count);
        }

        /// <summary>
        /// 发送赞(本地C++模式调用一次只能发送一个赞）。
        /// </summary>
        /// <param name="qqNumber">被操作的QQ。</param>
        /// <param name="count">发赞次数。</param>
        public static void SendPraise(long qqNumber, int count = 1)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][发赞] QQ：{0} {1}次", qqNumber, count));

            if (ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SendGood|{0}|{1}", qqNumber, count);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SendLike(CQAPI.GetAuthCode(), qqNumber);
            }
        }

        /// <summary>
        /// 接收语音（返回保存在酷Q的data\record 目录下）。
        /// </summary>
        /// <param name="fileName">保存的文件名。</param>
        /// <param name="postfixName">保存的文件格式。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：ReceiveVoice")]
        public static void 接收语音(string fileName, string postfixName)
        {
            ReceiveVoice(fileName, postfixName);
        }

        /// <summary>
        /// 接收语音（返回保存在酷Q的data\record 目录下）。
        /// </summary>
        /// <param name="fileName">保存的文件名。</param>
        /// <param name="postfixName">保存的文件格式。</param>
        [Obsolete("该方法在本地C++模式下不被支持，请谨慎使用。")]
        public static void ReceiveVoice(string fileName, string postfixName)
        {
            if (ProxyType == CQProxyType.UDP)
            {
                CQLogger.GetInstance().AddLog(String.Format("[↓][语音] QQ：{0} {1}", fileName, postfixName));
                string content = String.Format("ReceiveVoice|{0}|{1}", fileName, postfixName);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
        }

        #endregion

        #region 管理

        /// <summary>
        /// 置群员禁言。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="time">禁言时长（以秒为单位)</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupMemberGag")]
        public static void 置群员禁言(long groupNumber, long qqNumber, long time)
        {
            SetGroupMemberGag(groupNumber, qqNumber, time);
        }

        /// <summary>
        /// 置群员禁言。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="time">禁言时长（以秒为单位)</param>
        public static void SetGroupMemberGag(long groupNumber, long qqNumber, long time)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][禁言] 群：{0} QQ：{1} Time：{2}", groupNumber, qqNumber, time));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupMemberGag|{0}|{1}|{2}", groupNumber, qqNumber, time);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupBan(CQAPI.GetAuthCode(), groupNumber, qqNumber, time);
            }
        }

        /// <summary>
        /// 置群成员名片
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="newName">新的群名称。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupNickName")]
        public static void 置群成员名片(long groupNumber, long qqNumber, string newName)
        {
            SetGroupNickName(groupNumber, qqNumber, newName);
        }

        /// <summary>
        /// 置群成员名片
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="newName">新的群名称。</param>
        public static void SetGroupNickName(long groupNumber, long qqNumber, string newName)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][名片] 群：{0} QQ：{1} {2}", groupNumber, qqNumber, newName));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupNickName|{0}|{1}|{2}", groupNumber, qqNumber, newName.Replace("|", "$内容分割$"));
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupCard(CQAPI.GetAuthCode(), groupNumber, qqNumber, newName);
            }
        }

        /// <summary>
        /// 置群成员专属头衔
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="newName">头衔名称。</param>
        /// <param name="time">过期时间（以秒为单位）。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupHonorName")]
        public static void 置群成员专属头衔(long groupNumber, long qqNumber, string newName, int time)
        {
            SetGroupHonorName(groupNumber, qqNumber, newName, time);
        }

        /// <summary>
        /// 置群成员专属头衔
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="newName">头衔名称。</param>
        /// <param name="time">过期时间（以秒为单位）。</param>
        public static void SetGroupHonorName(long groupNumber, long qqNumber, string newName, int time)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][头衔] 群：{0} QQ：{1} {2}", groupNumber, qqNumber, newName));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupHonor|{0}|{1}|{2}|{3}", groupNumber, qqNumber, newName.Replace("|", "$内容分割$"), time);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupSpecialTitle(CQAPI.GetAuthCode(), groupNumber, qqNumber, newName, time);
            }
        }

        /// <summary>
        /// 置群员移除。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="refuse">是否拒绝再次加群。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupMemberRemove")]
        public static void 置群员移除(long groupNumber, long qqNumber, bool refuse = false)
        {
            SetGroupMemberRemove(groupNumber, qqNumber, refuse);
        }

        /// <summary>
        /// 置群员移除。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="refuse">是否拒绝再次加群。</param>
        public static void SetGroupMemberRemove(long groupNumber, long qqNumber, bool refuse = false)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][踢人] 群：{0} QQ：{1}", groupNumber, qqNumber));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupMemberRemove|{0}|{1}|{2}", groupNumber, qqNumber, refuse);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupKick(CQAPI.GetAuthCode(), groupNumber, qqNumber, refuse ? 1 : 0);
            }
        }

        /// <summary>
        /// 置好友添加请求。
        /// </summary>
        /// <param name="react">请求反馈标识。</param>
        /// <param name="reactType">反馈类型。</param>
        /// <param name="description">备注。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetFriendAddRequest")]
        public static void 置好友添加请求(string react, CQReactType reactType, string description = "")
        {
            SetFriendAddRequest(react, reactType, description);
        }

        /// <summary>
        /// 置好友添加请求。
        /// </summary>
        /// <param name="react">请求反馈标识。</param>
        /// <param name="reactType">反馈类型。</param>
        /// <param name="description">备注。</param>
        public static void SetFriendAddRequest(string react, CQReactType reactType, string description = "")
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][请友] {0} {1}", react, reactType));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetFriendAddRequest|{0}|{1}|{2}", react, (int)reactType, description.Replace("|", "$内容分割$"));
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetFriendAddRequest(CQAPI.GetAuthCode(), react, (int)reactType, description);
            }
        }

        /// <summary>
        /// 置群添加请求。
        /// </summary>
        /// <param name="react">请求反馈标识。</param>
        /// <param name="requestType">请求类型。</param>
        /// <param name="reactType">反馈类型。</param>
        /// <param name="reason">加群原因。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupAddRequest")]
        public static void 置群添加请求(string react, CQRequestType requestType, CQReactType reactType, string reason = "")
        {
            SetGroupAddRequest(react, requestType, reactType, reason);
        }

        /// <summary>
        /// 置群添加请求。
        /// </summary>
        /// <param name="react">请求反馈标识。</param>
        /// <param name="requestType">请求类型。</param>
        /// <param name="reactType">反馈类型。</param>
        /// <param name="reason">加群原因。</param>
        public static void SetGroupAddRequest(string react, CQRequestType requestType, CQReactType reactType, string reason = "")
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][请群] {0} {1} {2}", react, requestType, reactType));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupAddRequest|{0}|{1}|{2}|{3}", react, (int)requestType, (int)reactType, reason.Replace("|", "$内容分割$"));
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupAddRequestV2(CQAPI.GetAuthCode(), react, (int)requestType, (int)reactType, reason);
            }
        }

        /// <summary>
        /// 置群退出。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="dissolution">是否解散。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupExit")]
        public static void 置群退出(long groupNumber, bool dissolution = false)
        {
            SetGroupExit(groupNumber, dissolution);
        }

        /// <summary>
        /// 置群退出。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="dissolution">是否解散。</param>
        public static void SetGroupExit(long groupNumber, bool dissolution = false)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][退群] 群：{0} {1}", groupNumber, dissolution ? "解散" : ""));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupExit|{0}|{1}", groupNumber, dissolution);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupLeave(CQAPI.GetAuthCode(), groupNumber, dissolution ? 1 : 0);
            }
        }

        /// <summary>
        /// 置群管理员。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="admin">是否设置为管理员。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupAdministrator")]
        public static void 置群管理员(long groupNumber, long qqNumber, bool admin)
        {
            SetGroupAdministrator(groupNumber, qqNumber, admin);
        }

        /// <summary>
        /// 置群管理员。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="admin">是否设置为管理员。</param>
        public static void SetGroupAdministrator(long groupNumber, long qqNumber, bool admin)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][管理] 群：{0} QQ：{1}", groupNumber, qqNumber, admin ? "提升为管理员" : "降为成员"));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupAdministrator|{0}|{1}|{2}", groupNumber, qqNumber, admin);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupAdmin(CQAPI.GetAuthCode(), groupNumber, qqNumber, admin ? 1 : 0);
            }
        }

        /// <summary>
        /// 置全群禁言。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="gag">设置或关闭全群禁言。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupAllGag")]
        public static void 置全群禁言(long groupNumber, bool gag)
        {
            SetGroupAllGag(groupNumber, gag);
        }

        /// <summary>
        /// 置全群禁言。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="gag">设置或关闭全群禁言。</param>
        public static void SetGroupAllGag(long groupNumber, bool gag)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][禁言] 群：{0} QQ：{1}", groupNumber, gag ? "全员禁言" : "取消全员禁言"));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupAllGag|{0}|{1}", groupNumber, gag);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupWholeBan(CQAPI.GetAuthCode(), groupNumber, gag ? 1 : 0);
            }
        }

        /// <summary>
        /// 置群匿名设置。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="allow">开启或关闭匿名功能。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupAllowAnonymous")]
        public static void 置群匿名设置(long groupNumber, bool allow)
        {
            SetGroupAllowAnonymous(groupNumber, allow);
        }

        /// <summary>
        /// 置群匿名设置。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="allow">开启或关闭匿名功能。</param>
        public static void SetGroupAllowAnonymous(long groupNumber, bool allow)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][禁言] 群：{0} QQ：{1}", groupNumber, allow ? "开启匿名" : "关闭匿名"));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupAllowAnonymous|{0}|{1}", groupNumber, allow);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupAnonymous(CQAPI.GetAuthCode(), groupNumber, allow ? 1 : 0);
            }
        }

        /// <summary>
        /// 置讨论组退出。
        /// </summary>
        /// <param name="discussNumber">讨论组号码。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetDiscussExit")]
        public static void 置讨论组退出(long discussNumber)
        {
            SetDiscussExit(discussNumber);
        }

        /// <summary>
        /// 置讨论组退出。
        /// </summary>
        /// <param name="discussNumber">讨论组号码。</param>
        public static void SetDiscussExit(long discussNumber)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][退组] 组：{0}", discussNumber));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetDiscussExit|{0}", discussNumber);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetDiscussLeave(CQAPI.GetAuthCode(), discussNumber);
            }
        }

        /// <summary>
        /// 置匿名群员禁言。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="anomymous">被操作的匿名成员名称。</param>
        /// <param name="time">禁言时长（以秒为单位)</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：SetGroupAnonymousMemberGag")]
        public static void 置匿名群员禁言(long groupNumber, string anomymous, long time)
        {
            SetGroupAnonymousMemberGag(groupNumber, anomymous, time);
        }

        /// <summary>
        /// 置匿名群员禁言。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="anomymous">被操作的匿名成员名称。</param>
        /// <param name="time">禁言时长（以秒为单位)</param>
        public static void SetGroupAnonymousMemberGag(long groupNumber, string anomymous, long time)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↑][禁言] 群：{0} 匿名：{1} 时长：{2}", groupNumber, anomymous, time));

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("SetGroupAnonymousMemberGag|{0}|{1}|{2}", groupNumber, anomymous.Replace("|", "$内容分割$"), time);
                CQUDPProxy.GetInstance().SendMessage(content);
            }
            else
            {
                CQAPI.SetGroupAnonymousBan(CQAPI.GetAuthCode(), groupNumber, anomymous, time);
            }
        }

        #endregion

        #region 取基本信息

        /// <summary>
        /// 取群成员信息。
        /// <para>
        /// 多线程同步等待，采用阻塞线程的方式等待客户端返回群成员信息，响应时间较慢，建议使用缓存。
        /// </para>
        /// <para>
        /// 缓存时长1天，超过1天的成员，在下次访问时会通过酷Q重新获取最新信息。
        /// </para>
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="cache">是否使用缓存（使用缓存后，当后第一次访问会通过客户端读取，之后每次都通过缓存获得）。</param>
        [Obsolete("中文方法已经被弃用，请使用英文方法：GetGroupMemberInfo")]
        public static CQGroupMemberInfo 取群成员信息(long groupNumber, long qqNumber, bool cache = true)
        {
            return GetGroupMemberInfo(groupNumber, qqNumber, cache);
        }

        /// <summary>
        /// 取群成员信息。
        /// <para>
        /// 多线程同步等待，采用阻塞线程的方式等待客户端返回群成员信息，响应时间较慢，建议使用缓存。
        /// </para>
        /// <para>
        /// 缓存时长1天，超过1天的成员，在下次访问时会通过酷Q重新获取最新信息。
        /// </para>
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <param name="qqNumber">被操作的QQ号码。</param>
        /// <param name="cache">是否使用缓存（使用缓存后，当后第一次访问会通过客户端读取，之后每次都通过缓存获得）。</param>
        public static CQGroupMemberInfo GetGroupMemberInfo(long groupNumber, long qqNumber, bool cache = true)
        {
                lock (_syncRoot)
                {
                    Dictionary<long, CQGroupMemberInfo> dicMemebers = new Dictionary<long, CQGroupMemberInfo>();

                    if (_dicCache.ContainsKey(groupNumber))
                    {
                        dicMemebers = _dicCache[groupNumber];
                    }
                    else
                    {
                        _dicCache.Add(groupNumber, dicMemebers);
                    }

                    CQGroupMemberInfo member = new CQGroupMemberInfo();

                    if (dicMemebers.ContainsKey(qqNumber))
                    {
                        member = dicMemebers[qqNumber];
                    }
                    else
                    {
                        dicMemebers.Add(qqNumber, member);
                    }

                    if (!cache || member.RefreshDate.Date.AddDays(1) < DateTime.Now)
                    {
                        CQLogger.GetInstance().AddLog(String.Format("[↓][成员] 群：{0} QQ：{1}", groupNumber, qqNumber));
                        string content = String.Empty;

                        if(CQ.ProxyType == CQProxyType.UDP)
                        {
                            content = String.Format("GroupMemberRequest|{0}|{1}", groupNumber, qqNumber);
                            member = CQUDPProxy.GetInstance().GetGroupMemberInfo(content);
                        }
                        if (CQ.ProxyType == CQProxyType.NativeClr)
                        {
                            content = CQAPI.GetGroupMemberInfo(CQAPI.GetAuthCode(), groupNumber, qqNumber, cache ? 1 : 0);
                            member = CQMessageAnalysis.AnalyzeGroupMember(content);
                        }

                        if (cache)
                        {
                            dicMemebers[qqNumber] = member;
                        }
                    }

                    return member;
                }


            return new CQGroupMemberInfo();
        }

        /// <summary>
        /// 取登录QQ。
        /// </summary>
        /// <returns>登录的QQ号码</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：GetLoginQQ")]
        public static long 取登录QQ()
        {
            return GetLoginQQ();
        }

        /// <summary>
        /// 取登录QQ。
        /// </summary>
        /// <returns>登录的QQ号码</returns>
        public static long GetLoginQQ()
        {
            CQLogger.GetInstance().AddLog(String.Format("[↓][帐号] 取登录QQ"));
            
            try
            {
                if (ProxyType == CQProxyType.UDP)
                {
                    string content = String.Format("GetLoginQQRequest");
                    string result = CQUDPProxy.GetInstance().GetStringMessage(content);
                    return Convert.ToInt64(result);
                }
                else
                {
                    return CQAPI.GetLoginQQ(CQAPI.GetAuthCode());
                }
            }
            catch
            {

            }

            return 0;
        }

        /// <summary>
        /// 取登录昵称。
        /// </summary>
        /// <returns>登录的QQ号码昵称。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：GetLoginName")]
        public static string 取登录昵称()
        {
            return GetLoginName();
        }

        /// <summary>
        /// 取登录昵称。
        /// </summary>
        /// <returns>登录的QQ号码昵称。</returns>
        public static string GetLoginName()
        {
            CQLogger.GetInstance().AddLog(String.Format("[↓][帐号] 取登录昵称"));

            try
            {
                if (ProxyType == CQProxyType.UDP)
                {
                    string content = String.Format("GetLoginNameRequest");
                    string result = CQUDPProxy.GetInstance().GetStringMessage(content);
                    return result;
                }
                else
                {
                    return CQAPI.GetLoginNick(CQAPI.GetAuthCode());
                }
            }
            catch
            {

            }

            return "";
        }

        /// <summary>
        /// 取Cookies。
        /// </summary>
        /// <returns>登录的Cookies。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：GetCookies")]
        public static string 取Cookies()
        {
            return GetCookies();
        }

        /// <summary>
        /// 取Cookies。
        /// </summary>
        /// <returns>登录的Cookies。</returns>
        public static string GetCookies()
        {
            CQLogger.GetInstance().AddLog(String.Format("[↓][帐号] 取Cookies"));


            if (ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("GetCookiesRequest");
                string result = CQUDPProxy.GetInstance().GetStringMessage(content);
                return result;
            }
            else
            {
                return CQAPI.GetCookies(CQAPI.GetAuthCode());
            }
        }

        /// <summary>
        /// 取取CsrfToken。
        /// </summary>
        /// <returns>登录的CsrfToken。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：GetCsrfToken")]
        public static int 取CsrfToken()
        {
            return GetCsrfToken();
        }

        /// <summary>
        /// 取取CsrfToken。
        /// </summary>
        /// <returns>登录的CsrfToken。</returns>
        public static int GetCsrfToken()
        {
            CQLogger.GetInstance().AddLog(String.Format("[↓][帐号] 取CsrfToken"));

            if (ProxyType == CQProxyType.UDP)
            {
                string content = String.Format("GetCsrfTokenRequest");
                string result = CQUDPProxy.GetInstance().GetStringMessage(content);
                return Convert.ToInt32(result);
            }
            else
            {
                return CQAPI.GetCsrfToken(CQAPI.GetAuthCode());
            }
        }
        
        /// <summary>
        /// 获取C#插件的应用目录。
        /// </summary>
        /// <returns>应用目录。</returns>
        [Obsolete("中文方法已经被弃用，请使用英文方法：GetCQAppFolder")]
        public static string 取应用目录()
        {
            return GetCQAppFolder();
        }

        /// <summary>
        /// 获取酷Q插件App的目；如果是UDP方式，返回的则为酷Q主目录。
        /// </summary>
        /// <returns></returns>
        public static string GetCQAppFolder()
        {
            if (ProxyType == CQProxyType.UDP)
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            else
            {
                return CQAPI.GetCQAppFolder(CQAPI.GetAuthCode());
            }
        }

        /// <summary>
        /// 获取C#插件的应用目录。
        /// </summary>
        /// <returns>应用目录。</returns>
        public static string GetCSPluginsFolder()
        {
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSharpPlugins");
        }


        //CQ.CQ码_名片分享 ()
        //CQ.CQ码_音乐自定义分享 ()

        //CQ.取陌生人信息 ()

        #endregion

        #region 扩展功能

        /// <summary>
        /// 取QQ昵称。
        /// </summary>
        /// <param name="qqNumber">QQ号码。</param>
        /// <returns>昵称。</returns>
        [Obsolete("该方法非酷Q原生方法，建议使用http://r.pengyou.com/fcg-bin/cgi_get_portrait.fcg?uins={QQ号码}&get_nick=1&_=1438937421131 接口自行获取。")]
        public static string GetQQName(long qqNumber)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↓][昵称] QQ：{0}", qqNumber));

            string url = String.Format("http://r.pengyou.com/fcg-bin/cgi_get_portrait.fcg?uins={0}&get_nick=1&_=1438937421131", qqNumber);
            string content = HttpHelper.Get(url, "r.pengyou.com", Encoding.GetEncoding("GB2312"));

            if (content.Contains("error"))
            {
                return "暂无昵称";
            }
            else
            {
                string[] mc = content.GetMidStrings(",\"", "\",");
                if (mc.Length > 0)
                {
                    return mc[0];
                }
                else
                {
                    return "暂无昵称";
                }
            }
        }

        /// <summary>
        /// 取QQ头像。
        /// </summary>
        /// <param name="qqNumber">QQ号码。</param>
        /// <returns>头像。</returns>
        [Obsolete("该方法非酷Q原生方法，建议使用 http://q.qlogo.cn/headimg_dl?dst_uin={QQ号码}&spec=640&img_type=jpg 接口自行获取。")]
        public static Image GetQQFace(long qqNumber)
        {
            CQLogger.GetInstance().AddLog(String.Format("[↓][头像] QQ：{0}", qqNumber));

            string url = String.Format("http://q.qlogo.cn/headimg_dl?dst_uin={0}&spec=640&img_type=jpg", qqNumber);

            Image img = HttpHelper.GetImage(url);

            return img;
        }

        /// <summary>
        /// 取酷Q登录帐号的所有群列表。
        /// </summary>
        /// <returns>登录帐号所在的群列表。</returns>
        [Obsolete("该方法非酷Q原生方法，且在C++版本中不被支持，建议使用 http://qun.qzone.qq.com/cgi-bin/get_group_list 接口自行获取。")]
        public static List<CQGroupInfo> GetGroupList()
        {
            List<CQGroupInfo> list = new List<CQGroupInfo>();

            if (ProxyType == CQProxyType.UDP)
            {
                try
                {
                    CQLogger.GetInstance().AddLog(String.Format("[↓][帐号] 取群列表"));

                    string content = String.Format("GetGroupListRequest");
                    string result = CQUDPProxy.GetInstance().GetStringMessage(content);

                    string[] msGroupIds = result.GetMidStrings("\"groupid\":", ",\"groupname\":\"");
                    string[] msGroupNames = result.GetMidStrings("groupname\":\"", "\"");

                    if (msGroupIds.Length == msGroupNames.Length)
                    {
                        for (int i = 0; i < msGroupIds.Length; i++)
                        {
                            CQGroupInfo info = new CQGroupInfo();

                            info.GroupNumber = Convert.ToInt64(msGroupIds[i]);
                            info.GroupName = msGroupNames[i];

                            list.Add(info);
                        }
                    }
                }
                catch
                {

                }
            }

            return list;
        }

        /// <summary>
        /// 取酷指定群的成员列表（只能获取到群号、QQ号、管理权限、昵称等信息。其它信息请通过【取群成员信息】方法逐一获取）。
        /// </summary>
        /// <param name="groupNumber">群号码。</param>
        /// <returns>登录帐号所在的群列表。</returns>
        [Obsolete("该方法非酷Q原生方法，且在C++版本中不被支持，建议使用 http://qun.qzone.qq.com/cgi-bin/get_group_member 接口自行获取。")]
        public static List<CQGroupMemberInfo> GetGroupMemberList(long groupNumber)
        {
            List<CQGroupMemberInfo> list = new List<CQGroupMemberInfo>();

            if (ProxyType == CQProxyType.UDP)
            {
                try
                {
                    CQLogger.GetInstance().AddLog(String.Format("[↓][帐号] 群：{0} 取群成员列表", groupNumber));

                    string content = String.Format("GetGroupMemberListRequest|{0}", groupNumber);
                    string result = CQUDPProxy.GetInstance().GetStringMessage(content);

                    string[] msQQIds = result.GetMidStrings("\"uin\":", "}");
                    string[] msQQNames = result.GetMidStrings("\"nick\":\"", "\",\"uin\":");
                    string[] msIsAdmins = result.GetMidStrings("\"ismanager\":", ",");
                    string[] msIsCreators = result.GetMidStrings("\"iscreator\":", ",");

                    if (msQQIds.Length == msQQNames.Length &&
                        msQQNames.Length == msIsAdmins.Length &&
                        msIsAdmins.Length == msIsCreators.Length)
                    {
                        for (int i = 0; i < msQQIds.Length; i++)
                        {
                            CQGroupMemberInfo info = new CQGroupMemberInfo();

                            info.GroupNumber = groupNumber;
                            info.QQNumber = Convert.ToInt64(msQQIds[i]);
                            info.QQName = msQQNames[i];

                            int isAdmin = Convert.ToInt32(msIsAdmins[i]);
                            int isCreator = Convert.ToInt32(msIsCreators[i]);

                            info.Authority = isCreator == 1 ? "群主" : isAdmin == 1 ? "管理" : "成员";

                            list.Add(info);
                        }
                    }
                }
                catch
                {

                }
            }

            return list;
        }

        #endregion


    }
}
