using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Flexlive.CQP.CSharpProxy
{
    /// <summary>
    /// 托盘管理类
    /// </summary>
    public class NotifyManager : IDisposable {
        /// <summary>
        /// 托盘对象
        /// </summary>
        public System.Windows.Forms.NotifyIcon MyNotifyIcon {
            get;
            private set;
        }

        /// <summary>
        /// 调用托盘的窗体
        /// </summary>
        private MainWindow window = null;

        /// <summary>
        /// 构造函数
        /// </summary>
        private NotifyManager()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="window"></param>
        public NotifyManager(MainWindow window)
        {
            this.window = window;
            //初始化托盘
            this.InitializeNotifyIcon();
        }

        /// <summary>
        /// 初始化系统托盘对象
        /// </summary>
        private void InitializeNotifyIcon() {
            //初始化系统托盘对象
            this.MyNotifyIcon = new System.Windows.Forms.NotifyIcon();

            //初始化资源管理类
            ResourceManager resource = new ResourceManager(typeof(Properties.Resources));
            //赋值托盘图标
            this.MyNotifyIcon.Icon = (System.Drawing.Icon)resource.GetObject("AppIcon");
            //显示托盘图标
            this.MyNotifyIcon.Visible = true;
            //托盘图标提示内容
            this.MyNotifyIcon.Text = "酷Q C# 代理 v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

            //为托盘对象添加双击事件
            this.MyNotifyIcon.DoubleClick += new EventHandler(this.myNotifyIcon_DoubleClick);
            //为托盘对象添加鼠标按下事件
            this.MyNotifyIcon.MouseDown += new System.Windows.Forms.MouseEventHandler(myNotifyIcon_MouseDown);
        }

        /// <summary>
        /// 系统托盘双击事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myNotifyIcon_DoubleClick(object sender, EventArgs e) {
            //显示主窗体
            this.window.Show();

            //放在所有窗口前面
            this.window.Activate();

            //调整窗体显示状态为正常
            this.window.WindowState = WindowState.Normal;
        }

        /// <summary>
        /// 系统托盘鼠标按下事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void myNotifyIcon_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e) {
            //判断按下的鼠标键是否为右键
            if (e.Button == System.Windows.Forms.MouseButtons.Right) {
                //从资源中获取右键菜单对象
                ContextMenu menuItem = (ContextMenu)this.window.FindResource("NotifierContextMenu");

                //遍历菜单项
                foreach (object item in menuItem.Items) {
                    //判断如果为菜单
                    if (item is MenuItem) {
                        //获取当前菜单
                        MenuItem menu = item as MenuItem;
                        //添加事件
                        menu.Click += new RoutedEventHandler(ContextMenu_Click);
                    }
                }

                //显示右键菜单
                menuItem.IsOpen = true;
            }
        }

        /// <summary>
        /// 托盘菜单单击事件方法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_Click(object sender, RoutedEventArgs e) {
            //获取单击的菜单对象
            MenuItem menu = (MenuItem)sender;

            //判断执行事件的Image控件名称
            switch (menu.Tag.ToString()) {
                case "cmnuShow":
                    //显示主窗体
                    this.window.Show();

                    //放在所有窗口前面
                    this.window.Activate();

                    //调整窗体显示状态为正常
                    this.window.WindowState = WindowState.Normal;
                    break;
                case "cmnuExit":
                    //关闭当前窗体
                    this.window.Close();
                    break;
            }
        }

        #region IDisposable 成员

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose() {
            //销毁投盘对象
            this.MyNotifyIcon.Dispose();
        }

        #endregion
    }
}
