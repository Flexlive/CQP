using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Flexlive.CQP.Framework
{
    /// <summary>
    /// 插件配置管理类。
    /// </summary>
    public class CSPluginsConfigManager
    {
        /// <summary>
        /// 单实例对象。
        /// </summary>
        private static CSPluginsConfigManager _instance = null;

        /// <summary>
        /// 应用加载状态。
        /// </summary>
        private Dictionary<string, bool> _dicPluginsLoadingStatus = null;

        /// <summary>
        /// 创建一个实例。
        /// </summary>
        private CSPluginsConfigManager()
        {
            this._dicPluginsLoadingStatus = new Dictionary<string, bool>();

            this.LoadConfig();
        }

        

        /// <summary>
        /// 获取一个单例对象。
        /// </summary>
        /// <returns></returns>
        public static CSPluginsConfigManager GetInstance()
        {
            if(_instance == null)
            {
                _instance = new CSPluginsConfigManager();
            }

            return _instance;
        }

        /// <summary>
        /// 获取插件加载状态。
        /// </summary>
        /// <param name="pluginName"></param>
        /// <returns></returns>
        public bool GetLoadingStatus(string pluginName)
        {
            if(this._dicPluginsLoadingStatus.ContainsKey(pluginName))
            {
                return this._dicPluginsLoadingStatus[pluginName];
            }

            return false;
        }

        /// <summary>
        /// 设置插件加载状态。
        /// </summary>
        /// <param name="pluginName"></param>
        /// <param name="status"></param>
        public void SetLoadingStatus(string pluginName, bool status)
        {
            if (this._dicPluginsLoadingStatus.ContainsKey(pluginName))
            {
                this._dicPluginsLoadingStatus[pluginName] = status;
            }
            else
            {
                this._dicPluginsLoadingStatus.Add(pluginName, status);
            }

            this.SaveConfig();
        }

        /// <summary>
        /// 读取配置。
        /// </summary>
        private void LoadConfig()
        {
            string cfgFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "conf");
            if (!Directory.Exists(cfgFileFolder))
            {
                Directory.CreateDirectory(cfgFileFolder);
            }

            string cfgFilePath = Path.Combine(cfgFileFolder, "CSPlugins.cfg");
            if (File.Exists(cfgFilePath))
            {
                XElement element = XElement.Load(cfgFilePath);

                foreach (XElement xmlConfig in element.Elements())
                {
                    try
                    {
                        _dicPluginsLoadingStatus.Add(xmlConfig.Attribute("Name").Value,
                            Convert.ToBoolean(xmlConfig.Attribute("Status").Value));
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 存储配置。
        /// </summary>
        private void SaveConfig()
        {
            string cfgFileFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "conf");
            if (!Directory.Exists(cfgFileFolder))
            {
                Directory.CreateDirectory(cfgFileFolder);
            }

            string cfgFilePath = Path.Combine(cfgFileFolder, "CSPlugins.cfg");

            XElement xml = new XElement("Plugins");
            
            foreach(string key in this._dicPluginsLoadingStatus.Keys)
            {
                XElement xmlConfig = new XElement("Plugin", new XAttribute("Name", key), new XAttribute("Status", this._dicPluginsLoadingStatus[key].ToString()));

                xml.Add(xmlConfig);
            }

            xml.Save(cfgFilePath);
        }
    }
}
