using System;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 插件程序集属性接口。
    /// </summary>
    internal interface ICQAssembly
    {
        /// <summary>
        /// 程序集路径。
        /// </summary>
        string AssemblyPath { get; }

        /// <summary>
        /// 获取或设置插件名称。
        /// </summary>
        /// <value>
        /// 插件名称字符串。
        /// </value>
        /// <return>
        /// 插件名称字符串。
        /// </return>
        string Name { get; }

        /// <summary>
        /// 获取或设置插件版本。
        /// </summary>
        /// <value>
        /// 插件版本信息。
        /// </value>
        /// <return>
        /// 插件版本信息。
        /// </return>
        Version Version { get; }

        /// <summary>
        /// 获取或设置插件作者。
        /// </summary>
        /// <value>
        /// 插件作者名称。
        /// </value>
        /// <return>
        /// 插件作者名称。
        /// </return>
        string Author { get; }

        /// <summary>
        /// 获取或设置插件说明。
        /// </summary>
        /// <value>
        /// 插件说明描述信息。
        /// </value>
        /// <return>
        /// 插件说明描述信息。
        /// </return>
        string Description { get; }

        /// <summary>
        /// 打开设置窗口。
        /// </summary>
        void OpenSettingForm();
    }
}
