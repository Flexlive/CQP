using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 
    /// </summary>
    internal static class CQMessageAnalysis
    {

        /// <summary>
        /// 声明对象多线程同步访问锁引用。
        /// </summary>
        [NonSerialized]
        private static Object _syncRoot = null;

        /// <summary>
        /// 静态构造。
        /// </summary>
        static CQMessageAnalysis()
        {
            //初始化对象多线程同步访问锁。
            Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);
        }

        /// <summary>
        /// 解析数据。
        /// </summary>
        /// <param name="data"></param>
        public static void Analyze(string data)
        {
            lock (_syncRoot)
            {
                string[] args = data.Split(new char[] { '|' });

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

                    if (CQ.ProxyType == CQProxyType.UDP)
                    {
                        foreach (CQAppAbstract app in CQAppContainer.GetInstance().Apps)
                        {
                            if(!app.RunningStatus)
                            {
                                continue;
                            }

                            switch (eventType)
                            {
                                case "PrivateMessage": //私聊消息
                                    app.PrivateMessage(subType, sendTime, fromQQ, msg, font);
                                    break;
                                case "GroupMessage": //群消息
                                    app.GroupMessage(subType, sendTime, fromGroup, fromQQ, fromAnonymous, msg, font);
                                    break;
                                case "DiscussMessage": //讨论组消息
                                    app.DiscussMessage(subType, sendTime, fromDiscuss, fromQQ, msg, font);
                                    break;
                                case "GroupUpload": //群文件上传事件
                                    app.GroupUpload(subType, sendTime, fromGroup, fromQQ, file);
                                    break;
                                case "GroupAdmin": //群事件-管理员变动
                                    app.GroupAdmin(subType, sendTime, fromGroup, beingOperateQQ);
                                    break;
                                case "GroupMemberDecrease": //群事件-群成员减少
                                    app.GroupMemberDecrease(subType, sendTime, fromGroup, fromQQ, beingOperateQQ);
                                    break;
                                case "GroupMemberIncrease": //群事件-群成员增加
                                    app.GroupMemberIncrease(subType, sendTime, fromGroup, fromQQ, beingOperateQQ);
                                    break;
                                case "FriendAdded": //好友事件-好友已添加
                                    app.FriendAdded(subType, sendTime, fromQQ);
                                    break;
                                case "RequestAddFriend": //请求-好友添加
                                    app.RequestAddFriend(subType, sendTime, fromQQ, msg, responseFlag);
                                    break;
                                case "RequestAddGroup": //请求-群添加
                                    app.RequestAddGroup(subType, sendTime, fromGroup, fromQQ, msg, responseFlag);
                                    break;
                            }
                        }
                    }

                    if (CQ.ProxyType == CQProxyType.NativeClr)
                    {
                        List<object> parameters = new List<object>();

                        switch (eventType)
                        {
                            case "PrivateMessage": //私聊消息
                                parameters = new List<object>() { subType, sendTime, fromQQ, msg, font };
                                break;
                            case "GroupMessage": //群消息
                                parameters = new List<object>() { subType, sendTime, fromGroup, fromQQ, fromAnonymous, msg, font };
                                break;
                            case "DiscussMessage": //讨论组消息
                                parameters = new List<object>() { subType, sendTime, fromDiscuss, fromQQ, msg, font};
                                break;
                            case "GroupUpload": //群文件上传事件
                                parameters = new List<object>() { subType, sendTime, fromGroup, fromQQ, file};
                                break;
                            case "GroupAdmin": //群事件-管理员变动
                                parameters = new List<object>() { subType, sendTime, fromGroup, beingOperateQQ};
                                break;
                            case "GroupMemberDecrease": //群事件-群成员减少
                                parameters = new List<object>() { subType, sendTime, fromGroup, fromQQ, beingOperateQQ};
                                break;
                            case "GroupMemberIncrease": //群事件-群成员增加
                                parameters = new List<object>() { subType, sendTime, fromGroup, fromQQ, beingOperateQQ};
                                break;
                            case "FriendAdded": //好友事件-好友已添加
                                parameters = new List<object>() { subType, sendTime, fromQQ};
                                break;
                            case "RequestAddFriend": //请求-好友添加
                                parameters = new List<object>() { subType, sendTime, fromQQ, msg, responseFlag};
                                break;
                            case "RequestAddGroup": //请求-群添加
                                parameters = new List<object>() { subType, sendTime, fromGroup, fromQQ, msg, responseFlag };
                                break;
                        }

                        CallMethod(eventType, parameters.ToArray());
                    }
                }
            }
        }

        /// <summary>
        /// 解析群成员信息。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        internal static CQGroupMemberInfo AnalyzeGroupMember(string data)
        {
            CQGroupMemberInfo info = new CQGroupMemberInfo();

            if (CQ.ProxyType == CQProxyType.UDP)
            {
                try
                {
                    string[] args = data.Split(new char[] { '|' });

                    info.GroupNumber = String.IsNullOrEmpty(args[1]) ? 0 : Convert.ToInt64(args[1]);
                    info.QQNumber = String.IsNullOrEmpty(args[2]) ? 0 : Convert.ToInt64(args[2]);
                    info.QQName = args[3];
                    info.GroupCard = args[4];
                    info.Gender = String.IsNullOrEmpty(args[5]) ? "保密" : args[5] == "0" ? "男" : " 女";
                    info.Age = String.IsNullOrEmpty(args[6]) ? 0 : Convert.ToInt32(args[6]);
                    info.JoinTime = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime().AddSeconds(String.IsNullOrEmpty(args[7]) ? 0 : Convert.ToInt32(args[7]));
                    info.LastSpeakingTime = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime().AddSeconds(String.IsNullOrEmpty(args[8]) ? 0 : Convert.ToInt32(args[8]));
                    info.Authority = String.IsNullOrEmpty(args[9]) ? "成员" : args[9] == "3" ? "群主" : args[9] == "2" ? "管理员" : "成员";
                    info.GroupHonor = args[10];
                    info.Area = args[11];
                    info.LevelName = args[12];
                    info.HasBadRecord = String.IsNullOrEmpty(args[13]) ? false : args[13] == "真" ? true : false;
                    info.CanModifyVestCard = String.IsNullOrEmpty(args[14]) ? false : args[14] == "真" ? true : false;
                    info.HonorExpirationTimes = String.IsNullOrEmpty(args[15]) ? 0 : Convert.ToInt32(args[15]);
                    info.RefreshDate = DateTime.Now;
                }
                catch
                {

                }
            }

            if (CQ.ProxyType == CQProxyType.NativeClr)
            {
                try
                {
                    byte[] memberBytes = Convert.FromBase64String(data);

                    byte[] groupNumberBytes = new byte[8];
                    Array.Copy(memberBytes, 0, groupNumberBytes, 0, 8);
                    Array.Reverse(groupNumberBytes);
                    info.GroupNumber = BitConverter.ToInt64(groupNumberBytes, 0);

                    byte[] qqNumberBytes = new byte[8];
                    Array.Copy(memberBytes, 8, qqNumberBytes, 0, 8);
                    Array.Reverse(qqNumberBytes);
                    info.QQNumber = BitConverter.ToInt64(qqNumberBytes, 0);

                    byte[] nameLengthBytes = new byte[2];
                    Array.Copy(memberBytes, 16, nameLengthBytes, 0, 2);
                    Array.Reverse(nameLengthBytes);
                    short nameLength = BitConverter.ToInt16(nameLengthBytes, 0);

                    byte[] nameBytes = new byte[nameLength];
                    Array.Copy(memberBytes, 18, nameBytes, 0, nameLength);
                    info.QQName = System.Text.Encoding.Default.GetString(nameBytes);

                    byte[] cardLengthBytes = new byte[2];
                    Array.Copy(memberBytes, 18 + nameLength, cardLengthBytes, 0, 2);
                    Array.Reverse(cardLengthBytes);
                    short cardLength = BitConverter.ToInt16(cardLengthBytes, 0);

                    byte[] cardBytes = new byte[cardLength];
                    Array.Copy(memberBytes, 20 + nameLength, cardBytes, 0, cardLength);
                    info.GroupCard = System.Text.Encoding.Default.GetString(cardBytes);

                    byte[] genderBytes = new byte[4];
                    Array.Copy(memberBytes, 20 + nameLength + cardLength, genderBytes, 0, 4);
                    Array.Reverse(genderBytes);
                    info.Gender = BitConverter.ToInt32(genderBytes, 0) == 0 ? "男" : " 女";

                    byte[] ageBytes = new byte[4];
                    Array.Copy(memberBytes, 24 + nameLength + cardLength, ageBytes, 0, 4);
                    Array.Reverse(ageBytes);
                    info.Age = BitConverter.ToInt32(ageBytes, 0);

                    byte[] areaLengthBytes = new byte[2];
                    Array.Copy(memberBytes, 28 + nameLength + cardLength, areaLengthBytes, 0, 2);
                    Array.Reverse(areaLengthBytes);
                    short areaLength = BitConverter.ToInt16(areaLengthBytes, 0);

                    byte[] areaBytes = new byte[areaLength];
                    Array.Copy(memberBytes, 30 + nameLength + cardLength, areaBytes, 0, areaLength);
                    info.Area = System.Text.Encoding.Default.GetString(areaBytes);

                    byte[] addGroupTimesBytes = new byte[4];
                    Array.Copy(memberBytes, 30 + nameLength + cardLength + areaLength, addGroupTimesBytes, 0, 4);
                    Array.Reverse(addGroupTimesBytes);
                    info.JoinTime = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime().AddSeconds(BitConverter.ToInt32(addGroupTimesBytes, 0));

                    byte[] lastSpeakTimesBytes = new byte[4];
                    Array.Copy(memberBytes, 34 + nameLength + cardLength + areaLength, lastSpeakTimesBytes, 0, 4);
                    Array.Reverse(lastSpeakTimesBytes);
                    info.LastSpeakingTime = new DateTime(1970, 1, 1, 0, 0, 0).ToLocalTime().AddSeconds(BitConverter.ToInt32(lastSpeakTimesBytes, 0));

                    byte[] levelNameLengthBytes = new byte[2];
                    Array.Copy(memberBytes, 38 + nameLength + cardLength + areaLength, levelNameLengthBytes, 0, 2);
                    Array.Reverse(levelNameLengthBytes);
                    short levelNameLength = BitConverter.ToInt16(levelNameLengthBytes, 0);

                    byte[] levelNameBytes = new byte[levelNameLength];
                    Array.Copy(memberBytes, 40 + nameLength + cardLength + areaLength, levelNameBytes, 0, levelNameLength);
                    info.LevelName = System.Text.Encoding.Default.GetString(levelNameBytes);

                    byte[] authorBytes = new byte[4];
                    Array.Copy(memberBytes, 40 + nameLength + cardLength + areaLength + levelNameLength, authorBytes, 0, 4);
                    Array.Reverse(authorBytes);
                    int authority = BitConverter.ToInt32(authorBytes, 0);
                    info.Authority = authority == 3 ? "群主" : (authority == 2 ? "管理员" : "成员");

                    byte[] badBytes = new byte[4];
                    Array.Copy(memberBytes, 44 + nameLength + cardLength + areaLength + levelNameLength, badBytes, 0, 4);
                    Array.Reverse(badBytes);
                    info.HasBadRecord = BitConverter.ToInt32(badBytes, 0) == 1;

                    byte[] titleLengthBytes = new byte[2];
                    Array.Copy(memberBytes, 48 + nameLength + cardLength + areaLength + levelNameLength, titleLengthBytes, 0, 2);
                    Array.Reverse(titleLengthBytes);
                    short titleLength = BitConverter.ToInt16(titleLengthBytes, 0);

                    byte[] titleBytes = new byte[titleLength];
                    Array.Copy(memberBytes, 50 + nameLength + cardLength + areaLength + levelNameLength, titleBytes, 0, titleLength);
                    info.GroupHonor = System.Text.Encoding.Default.GetString(titleBytes);

                    byte[] titleExpireBytes = new byte[4];
                    Array.Copy(memberBytes, 50 + nameLength + cardLength + areaLength + levelNameLength + titleLength, titleExpireBytes, 0, 4);
                    Array.Reverse(titleExpireBytes);
                    info.HonorExpirationTimes = BitConverter.ToInt32(titleExpireBytes, 0);

                    byte[] modifyCardBytes = new byte[4];
                    Array.Copy(memberBytes, 54 + nameLength + cardLength + areaLength + levelNameLength + titleLength, titleExpireBytes, 0, 4);
                    Array.Reverse(titleExpireBytes);
                    info.CanModifyVestCard = BitConverter.ToInt32(titleExpireBytes, 0) == 1;
                }
                catch
                {

                }
            }

            return info;
        }

        /// <summary>
        /// 代理为NativeClr方式时，调用执行方法。
        /// </summary>
        /// <param name="methodName"></param>
        /// <param name="parameters"></param>
        private static void CallMethod(string methodName, object[] parameters)
        {
            //获取插件应用目录。
            string pluginFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSharpPlugins");

            foreach (string pluginFile in Directory.GetFiles(pluginFolder, "*.dll"))
            {
                try
                {
                    string pluginFileName = Path.GetFileNameWithoutExtension(pluginFile);

                    if (!CSPluginsConfigManager.GetInstance().GetLoadingStatus(pluginFileName))
                    {
                        continue;
                    }

                    //将Dll加载到二进制数组，再从数组加载Dll类。
                    Assembly assembly = Assembly.Load(File.ReadAllBytes(pluginFile));

                    //遍历程序集中所有的数据类型。
                    foreach (Type type in assembly.GetTypes())
                    {
                        if (!type.IsClass || type.IsNotPublic)
                        {
                            continue;
                        }

                        //获取全部继承的接口。
                        Type[] tempInterfaces = type.GetInterfaces();

                        //判断是否继承自ICQAssembly。
                        if (tempInterfaces.Select(s => s.Name).Contains("ICQAssembly"))
                        {
                            //Object theObj = Activator.CreateInstance(type);
                            Object theObj = CQAppContainer.GetInstance().ClrApps[pluginFile];
                            //反射到初始化方法，并执行。
                            //MethodInfo mi = type.GetMethod(methodName);
                            Dictionary<string, MethodInfo> dicMethods = CQAppContainer.GetInstance().ClrMethods[pluginFile];
                            MethodInfo mi = dicMethods[methodName];
                            Object returnValue = mi.Invoke(theObj, parameters);
                        }
                    }
                }
                catch
                {
                }
            }
        }
    }
}
