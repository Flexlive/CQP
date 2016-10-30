using Flexlive.CQP.Framework;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Flexlive.CQP.CSharpProxy
{
    /// <summary>
    /// TestWindow.xaml 的交互逻辑
    /// </summary>
    public partial class TestWindow : Window
    {
        public TestWindow()
        {
            InitializeComponent();

            foreach(string log in LogManager.GetInstance().Logs)
            {
                this.AddLog(log);
            }

            LogManager.GetInstance().NewLogWrite += CQLogger_NewLogWrite;
        }

        /// <summary>
        /// 处理CQ日志事件方法。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CQLogger_NewLogWrite(object sender, CQLogEventArgs e)
        {
            this.AddLog(e.LogMessage);
        }

        /// <summary>
        /// 多线程同步日志。
        /// </summary>
        /// <param name="logMessage"></param>
        public void AddLog(string logMessage)
        {
            this.Dispatcher.Invoke(new Action<string>(PrintLogs), logMessage);
        }

        /// <summary>
        /// 打印日志。
        /// </summary>
        /// <param name="logMessage"></param>
        private void PrintLogs(string logMessage)
        {
            TextBlock tbLog = new TextBlock();
            tbLog.Text = logMessage;
            SolidColorBrush logColor = Brushes.Black;
            if (logMessage.Contains("[＃]"))
            {
                logColor = Brushes.DeepPink;
            }
            if (logMessage.Contains("[↓]"))
            {
                logColor = Brushes.Green;
            }
            if (logMessage.Contains("[↑]"))
            {
                logColor = Brushes.Blue;
            }
            if (logMessage.Contains("[％]"))
            {
                logColor = Brushes.Red;
            }
            tbLog.Foreground = logColor;

            this.lsLogs.Items.Add(tbLog);
            if (this.lsLogs.Items.Count > 100)
            {
                this.lsLogs.Items.RemoveAt(0);
            }

            //滚动到当前日志
            this.lsLogs.ScrollIntoView(tbLog);
        }

        /// <summary>
        /// 发送私聊消息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendPrivateMessage_Click(object sender, RoutedEventArgs e)
        {
            CQ.SendPrivateMessage(Convert.ToInt64(txtQQNumber.Text), txtMessage.Text);
        }

        /// <summary>
        /// 发送群组消息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendGroupMessage_Click(object sender, RoutedEventArgs e)
        {
            CQ.SendGroupMessage(Convert.ToInt64(txtGroupNumber.Text), txtMessage.Text);
        }

        /// <summary>
        /// 发送讨论组消息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendDiscussMessage_Click(object sender, RoutedEventArgs e)
        {
            CQ.SendDiscussMessage(Convert.ToInt64(txtDiscussNumber.Text), txtMessage.Text);
        }

        /// <summary>
        /// 向指定群发送一个笑脸。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendFace_Click(object sender, RoutedEventArgs e)
        {
            CQ.SendGroupMessage(Convert.ToInt64(txtGroupNumber.Text), String.Format("这是一个笑脸{0}", CQ.CQCode_Face(12)));
        }

        /// <summary>
        /// 在指定的群中@某人。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSendAt_Click(object sender, RoutedEventArgs e)
        {
            CQ.SendGroupMessage(Convert.ToInt64(txtGroupNumber.Text), CQ.CQCode_At(Convert.ToInt64(this.txtQQNumber.Text)));
        }

        /// <summary>
        /// 获取登录QQ及昵称。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetLoginQQ_Click(object sender, RoutedEventArgs e)
        {
            string qq = CQ.GetLoginQQ().ToString();
            string name = CQ.GetLoginName();

            this.AddLog(String.Format("[{0}] [＝][测试] 登录QQ：{1} 昵称：{2}", DateTime.Now, qq, name));
        }

        /// <summary>
        /// 获取Cookies。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetCookies_Click(object sender, RoutedEventArgs e)
        {
            string str = CQ.GetCookies().ToString();

            this.AddLog(String.Format("[{0}] [＝][测试] 登录Cookies：{1}", DateTime.Now, str));
        }

        /// <summary>
        /// 获取CsrfToken。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetCsrfToken_Click(object sender, RoutedEventArgs e)
        {
            string str = CQ.GetCsrfToken().ToString();

            this.AddLog(String.Format("[{0}] [＝][测试] 登录CsrfToken：{1}", DateTime.Now, str));
        }

        /// <summary>
        /// 取群成员信息。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetGroupMember_Click(object sender, RoutedEventArgs e)
        {
            CQGroupMemberInfo info = CQ.GetGroupMemberInfo(Convert.ToInt64(txtGroupNumber.Text), Convert.ToInt64(txtQQNumber.Text));

            this.AddLog(String.Format("[{0}] [＝][测试] QQ：{1} 的群名片：{2}， 入群时间：{3}， 最后发言：{4}",
                DateTime.Now, txtQQNumber.Text, info.GroupCard, info.JoinTime, info.LastSpeakingTime));
        }

        /// <summary>
        /// 取QQ昵称。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetQQNickName_Click(object sender, RoutedEventArgs e)
        {
            string name = CQ.GetQQName(Convert.ToInt64(txtQQNumber.Text));

            this.AddLog(String.Format("[{0}] [＝][测试] QQ：{1} 的昵称为：{2}", DateTime.Now, txtQQNumber.Text, name));
        }

        /// <summary>
        /// 取QQ头像。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetFace_Click(object sender, RoutedEventArgs e)
        {
            System.Drawing.Image image = CQ.GetQQFace(Convert.ToInt64(txtQQNumber.Text));

            if (image != null)
            {
                this.imgQQFace.Source = Imaging.CreateBitmapSourceFromHBitmap(((System.Drawing.Bitmap)image).GetHbitmap(),
                    IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());

                this.AddLog(String.Format("[{0}] [＝][测试] QQ：{1} 的头像请看右侧头像框", DateTime.Now, txtQQNumber.Text));
            }
            else
            {
                this.AddLog(String.Format("[{0}] [＝][测试] QQ：{1} 获取头像失败。", DateTime.Now, txtQQNumber.Text));
            }
        }

        /// <summary>
        /// 获取群列表。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetGroups_Click(object sender, RoutedEventArgs e)
        {
            List<CQGroupInfo> groups = CQ.GetGroupList();

            string str = String.Empty;

            foreach (CQGroupInfo info in groups)
            {
                str += info.GroupNumber + ":" + info.GroupName + " ";
            }

            this.AddLog(String.Format("[{0}] [＝][测试] 取群列表 拥有群数：{1} {2}", DateTime.Now, groups.Count, str));
        }

        /// <summary>
        /// 群成员禁言。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetMemberGag_Click(object sender, RoutedEventArgs e)
        {
            CQ.SetGroupMemberGag(Convert.ToInt64(txtGroupNumber.Text), Convert.ToInt64(txtQQNumber.Text), 120);
        }

        /// <summary>
        /// 设置群成员片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetMemberName_Click(object sender, RoutedEventArgs e)
        {
            CQ.SetGroupNickName(Convert.ToInt64(txtGroupNumber.Text), Convert.ToInt64(txtQQNumber.Text), this.txtNickName.Text);
        }

        /// <summary>
        /// 设置专属头衔。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetMemberHonor_Click(object sender, RoutedEventArgs e)
        {
            CQ.SetGroupHonorName(Convert.ToInt64(txtGroupNumber.Text), Convert.ToInt64(txtQQNumber.Text), this.txtNickName.Text, 120);
        }

        /// <summary>
        /// 移除群。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetMemberRemove_Click(object sender, RoutedEventArgs e)
        {
            CQ.SetGroupMemberRemove(Convert.ToInt64(txtGroupNumber.Text), Convert.ToInt64(txtQQNumber.Text));
        }

        /// <summary>
        /// 设置管理员。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetAdmin_Click(object sender, RoutedEventArgs e)
        {
            CQ.SetGroupAdministrator(Convert.ToInt64(txtGroupNumber.Text), Convert.ToInt64(txtQQNumber.Text), true);
        }

        /// <summary>
        /// 取消群管理员。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetNotAdmin_Click(object sender, RoutedEventArgs e)
        {
            CQ.SetGroupAdministrator(Convert.ToInt64(txtGroupNumber.Text), Convert.ToInt64(txtQQNumber.Text), false);
        }

        /// <summary>
        /// 全群禁言。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetAllGag_Click(object sender, RoutedEventArgs e)
        {
            CQ.SetGroupAllGag(Convert.ToInt64(txtGroupNumber.Text), true);
        }

        /// <summary>
        /// 取消全群禁言。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetDisAllGag_Click(object sender, RoutedEventArgs e)
        {
            CQ.SetGroupAllGag(Convert.ToInt64(txtGroupNumber.Text), false);
        }

    }
}
