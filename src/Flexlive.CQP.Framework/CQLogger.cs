using System;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 日志管理。
    /// </summary>
    public class CQLogger
    {
        //单例对象。
        private static CQLogger _instance = null;

        /// <summary>
        /// 初始化 <see cref="CQLogger"/> 的一个单例。
        /// </summary>
        /// <returns></returns>
        public static CQLogger GetInstance()
        {
            if(_instance == null)
            {
                _instance = new CQLogger();
            }

            return _instance;
        }

        /// <summary>
        /// 隐藏构造函数。
        /// </summary>
        private CQLogger()
        {

        }

        /// <summary>
        /// 新日志产生事件。
        /// </summary>
        public event EventHandler<CQLogEventArgs> NewLogWrite = null;

        /// <summary>
        /// 添加日志。
        /// </summary>
        /// <param name="logMessage">日志消息。</param>
        internal void AddLog(string logMessage)
        {
            //判断事件是否被初始化
            if(this.NewLogWrite != null)
            {
                //拼组成日志。
                string content = String.Format("[{0}] {1}", DateTime.Now, logMessage);

                //触发日志事件。
                this.NewLogWrite(this, new CQLogEventArgs() { LogMessage = content });
            }
        }
    }
}
