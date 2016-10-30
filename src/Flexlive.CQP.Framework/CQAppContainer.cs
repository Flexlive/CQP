using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 插件容器类。
    /// </summary>
    public class CQAppContainer
    {
        /// <summary>
        /// 声明单例对象。
        /// </summary>
        private static CQAppContainer _instance = null;

        /// <summary>
        /// 声明对象多线程同步访问锁引用。
        /// </summary>
        [NonSerialized]
        private static Object _syncRoot = null;

        /// <summary>
        /// 隐藏构造函数。
        /// </summary>
        private CQAppContainer()
        {
            //初始化对象多线程同步访问锁。
            Interlocked.CompareExchange<Object>(ref _syncRoot, new Object(), null);

            this.LoadApps();
        }

        /// <summary>
        /// 创建一个 <see cref="CQAppContainer"/> 单例对象。
        /// </summary>
        /// <returns>一个 <see cref="CQAppContainer"/> 对象。</returns>
        public static CQAppContainer GetInstance()
        {
            if(_instance == null)
            {
                _instance = new CQAppContainer();
            }

            return _instance;
        }

        /// <summary>
        /// 声明插件列表对象。
        /// </summary>
        private List<CQAppAbstract> _apps = null;

        /// <summary>
        /// 获取插件列表。
        /// </summary>
        /// <returns>
        /// 插件列表。
        /// </returns>
        public List<CQAppAbstract> Apps
        {
            get
            {
                lock (_syncRoot)
                {
                    return this._apps;
                }
            }
        }

        /// <summary>
        /// 声明插件列表对象。
        /// </summary>
        private Dictionary<string, Object> _dicClrApps = null;

        /// <summary>
        /// CLR加载的插件列表。
        /// </summary>
        internal Dictionary<string, Object> ClrApps
        {
            get
            {
                lock (_syncRoot)
                {
                    return this._dicClrApps;
                }
            }
        }

        /// <summary>
        /// 声明插件方法列表对象。
        /// </summary>
        private Dictionary<string, Dictionary<string, MethodInfo>> _dicClrMethods = null;

        /// <summary>
        /// CLR加载的插件方法列表。
        /// </summary>
        internal Dictionary<string, Dictionary<string, MethodInfo>> ClrMethods
        {
            get
            {
                lock (_syncRoot)
                {
                    return this._dicClrMethods;
                }
            }
        }

        /// <summary>
        /// 重新加载插件列表。
        /// </summary>
        public void ReloadApps()
        {
            //可地增加已经加载的插件卸载代码。

            this.LoadApps();
        }

        /// <summary>
        /// 加载插件列表。
        /// </summary>
        private void LoadApps()
        {
            lock (_syncRoot)
            {
                this._apps = new List<CQAppAbstract>();
                this._dicClrApps = new Dictionary<string, object>();
                this._dicClrMethods = new Dictionary<string,Dictionary<string, MethodInfo>>();

                //获取插件应用目录。
                string pluginFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSharpPlugins");

                //目录不存在则返回空列表。
                if (!Directory.Exists(pluginFolder))
                {
                    Directory.CreateDirectory(pluginFolder);
                    return;
                }

                //遍历所有文件。
                foreach (string pluginFile in Directory.GetFiles(pluginFolder, "*.dll"))
                {
                    string pluginName = Path.GetFileNameWithoutExtension(pluginFile);

                    bool status = CSPluginsConfigManager.GetInstance().GetLoadingStatus(pluginName);

                    try
                    {
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
                                if (CQ.ProxyType == CQProxyType.NativeClr)
                                {
                                    Object theObj = Activator.CreateInstance(type);
                                    this._dicClrApps.Add(pluginFile, theObj);

                                    Dictionary<string, MethodInfo> dicMethods = new Dictionary<string, MethodInfo>();

                                    MethodInfo[] mis = type.GetMethods();

                                    foreach (MethodInfo mi in mis)
                                    {
                                        dicMethods.Add(mi.Name, mi);
                                    }

                                    this._dicClrMethods.Add(pluginFile, dicMethods);

                                    //反射到初始化方法，并执行。
                                    MethodInfo initMethod = type.GetMethod("Initialize");
                                    initMethod.Invoke(theObj, null);

                                    if (status)
                                    {
                                        //反射到启动方法，并执行。
                                        MethodInfo startMethod = type.GetMethod("Startup");
                                        startMethod.Invoke(theObj, null);
                                    }
                                }

                                /**
                                 * 不管是UDP还是NativeClr方式，都执行下面的代码，考虑到NativeClr方式要启动配置界面，所以才这么做；
                                 * 如果只是从酷Q通过C++反射调用页不是从代理界面中打开的，执行下面的代码会抛异常，不会影响程序的运行。
                                 */

                                CQAppAbstract plugin = (CQAppAbstract)Activator.CreateInstance(type, null);

                                plugin.AssemblyPath = pluginFile;
                                plugin.RunningStatus = status;

                                //初始化插件。
                                plugin.Initialize();

                                this._apps.Add(plugin);

                                if (!status)
                                {
                                    plugin.Name = "[未启用]" + plugin.Name;
                                }
                                else
                                {
                                    plugin.Startup();
                                }
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
}
