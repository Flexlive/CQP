using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 酷Q C#代理的类型。
    /// </summary>
    public enum CQProxyType
    {
        /// <summary>
        /// 基于UDP通讯的代理方式。
        /// </summary>
        UDP,
 
        /// <summary>
        /// 通过托管C++实现本地的CLR调用。
        /// </summary>
        NativeClr,
    }
}
