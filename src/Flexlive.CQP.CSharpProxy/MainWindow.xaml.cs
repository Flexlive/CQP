using Flexlive.CQP.Framework;
using Flexlive.CQP.Framework.Utils;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Flexlive.CQP.CSharpProxy
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 选择APP。
        /// </summary>
        private CQAppAbstract _selectApp = null;

        /// <summary>
        /// 托盘菜单。
        /// </summary>
        private NotifyManager _notify = null;

        public MainWindow()
        {
            InitializeComponent();

            this._notify = new NotifyManager(this);

            

            this.Title += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + "                    当前模式：" + (CQ.ProxyType == CQP.Framework.CQProxyType.UDP ? "UDP" : "NavitaCLR");
            this.lsApps.ItemsSource = CQAppContainer.GetInstance().Apps;



            CQLogger.GetInstance().NewLogWrite += CQLogger_NewLogWrite;

            try
            {
                if (CQ.ProxyType == CQProxyType.UDP)
                {
                    CQUDPProxy.GetInstance().Start();
                    this.btnPortSetting.Visibility = System.Windows.Visibility.Visible;

                    string folder = Path.Combine(CQ.GetCQAppFolder(), "app");
                    folder = Path.Combine(folder, "cc.flexlive.cqeproxy");


                    string ipAddress = "127.0.0.1";
                    int port = 18139;

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

                    this.Title = this.Title + ":" + port.ToString();
                }

                LogManager.GetInstance().AddLog(String.Format("[{0}] [＃][系统] CSharp代理启动成功，请手动给挂机QQ发送条信息激活酷Q端代理功能。", DateTime.Now));

                if (CQAppContainer.GetInstance().Apps.Count > 0)
                {
                    LogManager.GetInstance().AddLog(String.Format("[{0}] [＃][系统] 成功加载{1}个应用。", DateTime.Now, CQAppContainer.GetInstance().Apps.Count));
                }
                else
                {
                    LogManager.GetInstance().AddLog(String.Format("[{0}] [％][异常] 没有加载到应用，你可以使用测试功能测试发送消息。", DateTime.Now));
                }
            }
            catch
            {
                LogManager.GetInstance().AddLog(String.Format("[{0}] [％][异常] CSharp代理启动失败，18139端口被占用，请检查。", DateTime.Now));
            }
        }

        #region 重写基类方法...

        /// <summary>
        /// 重写基类的窗体状态改变方法
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStateChanged(EventArgs e)
        {
            //判断当前窗体状态是否为最小化
            if (WindowState == WindowState.Minimized)
            {
                //隐藏当前窗体
                this.Hide();
            }

            //调用基类的方法
            base.OnStateChanged(e);
        }

        /// <summary>
        /// 重写基类的正在关闭方法
        /// </summary>
        /// <param name="args"></param>
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            if (this._notify != null)
            {
                //销毁投盘对象
                this._notify.Dispose();
            }

            //调用基类的方法
            base.OnClosing(e);
        }

        #endregion

        /// <summary>
        /// 日志处理。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CQLogger_NewLogWrite(object sender, CQLogEventArgs e)
        {
            LogManager.GetInstance().AddLog(e.LogMessage);
        }

        /// <summary>
        /// 应用列表，项目选择事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void lsApps_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lsApps.SelectedItem != null)
            {
                this.gdNoSelect.Visibility = Visibility.Hidden;
                this.gdSelected.Visibility = Visibility.Visible;

                this._selectApp = lsApps.SelectedItem as CQAppAbstract;

                if (this._selectApp != null)
                {
                    this.txbName.Text = this._selectApp.Name;
                    this.txbAuthor.Text = this._selectApp.Author;
                    this.txbVersion.Text = this._selectApp.Version.ToString();
                    this.txbDescription.Text = this._selectApp.Description;

                    if(this._selectApp.RunningStatus)
                    {
                        this.btnPluginRunning.Content = "停止";
                    }
                    else
                    {
                        this.btnPluginRunning.Content = "启动";
                    }
                }
            }
            else
            {
                this.gdNoSelect.Visibility = Visibility.Visible;
                this.gdSelected.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// 重新加载应用按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReloadApps_Click(object sender, RoutedEventArgs e)
        {
            this._selectApp = null;
            this.lsApps.ItemsSource = null;
            CQAppContainer.GetInstance().ReloadApps();

            this.lsApps.ItemsSource = CQAppContainer.GetInstance().Apps;
        }

        /// <summary>
        /// 打开应用目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenAppFolder_Click(object sender, RoutedEventArgs e)
        {
            string pluginFolder = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CSharpPlugins");

            //目录不存在则返回空列表。
            if (!Directory.Exists(pluginFolder))
            {
                Directory.CreateDirectory(pluginFolder);
            }

            System.Diagnostics.Process.Start("explorer.exe", pluginFolder);
        }

        /// <summary>
        /// 退出按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 打开测试窗口按钮事件处理方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenTestView_Click(object sender, RoutedEventArgs e)
        {
            //if (CQ.ProxyType == CQProxyType.NativeClr)
            //{
            //    MessageBox.Show("本地CLR模式下无法使用调试功能。如需调试，请直接打开本程序使用UDP方式进行调试。", "提示");
            //    return;
            //}

            TestWindow testWindow = new TestWindow();
            testWindow.ShowDialog();
        }

        /// <summary>
        /// 打开App的设置界面，该界面由App的开发者自行处理及打开的。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenSettingForm_Click(object sender, RoutedEventArgs e)
        {
            if(this._selectApp != null)
            {
                this._selectApp.OpenSettingForm();
            }
        }

        /// <summary>
        /// 应用卸载按钮事件处理方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUnInstall_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectApp != null)
            {
                MessageBoxResult result = MessageBox.Show("卸载插件将删除插件对应的Dll文件，是否继续？", "询问", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    File.Delete(this._selectApp.AssemblyPath);

                    this.btnReloadApps_Click(this.btnReloadApps, null);
                }
            }
            else
            {
                MessageBox.Show("请先选择要卸载的应用。", "提示");
            }
        }

        /// <summary>
        /// 启用插件。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPluginControl_Click(object sender, RoutedEventArgs e)
        {
            if (this._selectApp != null)
            {
                this._selectApp.RunningStatus = !this._selectApp.RunningStatus;

                string pluginFlieName = Path.GetFileNameWithoutExtension(this._selectApp.AssemblyPath);
                CSPluginsConfigManager.GetInstance().SetLoadingStatus(pluginFlieName, this._selectApp.RunningStatus);

                this.btnPluginRunning.Content = this._selectApp.RunningStatus ? "停止" : "启动";

                btnReloadApps_Click(btnReloadApps, new RoutedEventArgs());
            }
        }

        /// <summary>
        /// 代理设置。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPortSetting_Click(object sender, RoutedEventArgs e)
        {
            string folder = Path.Combine(CQ.GetCQAppFolder(), "app");
            folder = Path.Combine(folder, "cc.flexlive.cqeproxy");

            if(!Directory.Exists(folder))
            {
                MessageBox.Show("请确认代理放置在酷Q目录下，并启用了cc.flexlive.cqeproxy。", "错误");
                return;
            }

            string iniFile = Path.Combine(folder, "cc.flexlive.cqeproxy.ini");

            if(!File.Exists(iniFile))
            {
                MessageBox.Show("配置文件不存在。", "错误");
                return;
            }
        }
    }
}
