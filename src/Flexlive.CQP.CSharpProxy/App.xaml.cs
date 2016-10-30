using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using System.Windows.Interop;

namespace Flexlive.CQP.CSharpProxy
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application, IDisposable
    {

        private Mutex m_Mutex = null;


        /// <summary>
        /// 添加启动参数的处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            CQP.Framework.CQ.ProxyType = Framework.CQProxyType.UDP;

            if (e.Args.Length > 0)
            {
                if(e.Args[0].ToUpper() == "CLR")
                {
                    CQP.Framework.CQ.ProxyType = Framework.CQProxyType.NativeClr;
                }
            }

            string folder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app");
            folder = Path.Combine(folder, "cc.flexlive.cqeproxy");

            string ipAddress = "127.0.0.1";
            int port = 18139;

            if (Directory.Exists(folder))
            {
                string iniFile = Path.Combine(folder, "cc.flexlive.cqeproxy.ini");

                if (File.Exists(iniFile))
                {
                    ipAddress = Flexlive.CQP.Framework.Utils.IniFileHelper.GetStringValue(iniFile, "代理配置", "服务器地址", "127.0.0.1");
                    string strPort = Flexlive.CQP.Framework.Utils.IniFileHelper.GetStringValue(iniFile, "代理配置", "服务器端口", "18139");
                    port = Convert.ToInt32(strPort);
                }
            }

            bool canRunNewInstance = true;
            m_Mutex = new System.Threading.Mutex(true,
                "CSharpProxy" + CQP.Framework.CQ.ProxyType.ToString() + port.ToString(), out canRunNewInstance);

            if (!canRunNewInstance)
            {
                m_Mutex = null;

                MessageBox.Show("亲，你已经打开了一个相同的应用，请在右下角任务栏里找找吧。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);

                Current.Shutdown();

                return;
            }

            base.OnStartup(e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(ExitEventArgs e)
        {
            Dispose(true);
            base.OnExit(e);
        }

        private void Dispose(Boolean disposing)
        {
            if (disposing && (m_Mutex != null))
            {
                m_Mutex.ReleaseMutex();
                m_Mutex.Close();
                m_Mutex = null;
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
