using System;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// CQ日志事件参数类。
    /// </summary>
    public class CQLogEventArgs : EventArgs
    {
        /// <summary>
        /// 日志发生的时间。
        /// </summary>
        public DateTime LogTime
        {
            get;
            set;
        }

        /// <summary>
        /// 日志来源。
        /// </summary>
        public string LogSource
        {
            get;
            set;
        }

        /// <summary>
        /// 日志类型。
        /// </summary>
        public string LogType
        {
            get;
            set;
        }

        /// <summary>
        /// 日志信息。
        /// </summary>
        public string LogMessage
        {
            get;
            set;
        }
    }
}
