using System;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 群信息。
    /// </summary>
    public class CQGroupInfo
    {
        /// <summary>
        /// 创建一个 <see cref="CQGroupInfo"/> 实例。
        /// </summary>
        public CQGroupInfo()
        {
            this.GroupName = String.Empty;
            this.GroupNumber = 0;
        }

        /// <summary>
        /// 获取或设置群号码。
        /// </summary>
        public long GroupNumber
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置群号码。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：GroupNumber")]
        public long 群号
        {
            get
            {
                return this.GroupNumber;
            }
        }

        /// <summary>
        /// 获取或设置群名称。
        /// </summary>
        public string GroupName
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置群名称。
        /// </summary>
        [Obsolete("中文属性已经被弃用，请使用英文属性：GroupName")]
        public string 群名称
        {
            get
            {
                return this.GroupName;
            }
        }
    }
}
