using Mono.Xml;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

namespace AppGame.Config
{
    /// <summary>
    /// 宝箱产出规则配置读取类
    /// </summary>
    public class ModuleConfig : BaseConfig, IModuleConfig
    {
        /************************************************属性与变量命名************************************************/
        //多语言资源配置数据字典
        private List<ModuleInfo> configs = new List<ModuleInfo>();
        /************************************************私  有  方  法************************************************/
        //读取语言配置文件
        private void ReadConfig(WWW www)
        {
            if (!string.IsNullOrEmpty(www.error))
            {
                Debug.LogErrorFormat("<><ModuleConfig.ReadConfig>Error: {0}" + www.error);
                return;
            }

            if (www.isDone && string.IsNullOrEmpty(www.error))
            {
                try
                {
                    SecurityParser xmlDoc = new SecurityParser();
                    Debug.LogFormat("<><ModuleConfig.ReadConfig>Content: {0}", www.text);

                    xmlDoc.LoadXml(www.text);
                    ArrayList allNodes = xmlDoc.ToXml().Children;
                    foreach (SecurityElement xeModules in allNodes)
                    {//根节点
                        if (xeModules.Tag == "Modules")
                        {//Modules节点
                            ArrayList moduleNodes = xeModules.Children;
                            foreach (SecurityElement xeModule in moduleNodes)
                            {//Module节点
                                if (xeModule.Tag == "Module")
                                {
                                    ModuleInfo moduleInfo = new ModuleInfo()
                                    {
                                        Name = xeModule.Attribute("Name"),
                                    };

                                    ArrayList fileNodes = xeModule.Children;
                                    foreach (SecurityElement xeFile in fileNodes)
                                    {//File节点
                                        if (xeFile.Tag == "File")
                                        {
                                            object fileTypes = Enum.Parse(typeof(FileTypes), xeFile.Attribute("Type"));
                                            moduleInfo.Files.Add(new ModuleFile()
                                            {
                                                Name = xeFile.Attribute("Name"),
                                                Path = xeFile.Attribute("Path"),
                                                Type = fileTypes != null ? (FileTypes)fileTypes : FileTypes.Sprite,
                                                Enable = xeFile.Attribute("Enable") == "1"
                                            });
                                        }
                                    }
                                    this.configs.Add(moduleInfo);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogErrorFormat("<><ModuleConfig.ReadConfig>Error: {0}", ex.Message);
                }
                Debug.Log("<><ModuleConfig.ReadConfig>Load complete");
                this.isLoaded = true;
            }
        }
        /************************************************公  共  方  法************************************************/
        /// <summary>
        /// 加载模块数据
        /// </summary>
        public void Load()
        {
            if (this.isLoaded)
                return;
            this.configs.Clear();
            ConfigLoader.Instance.LoadConfig("Config/Global/ModuleConfig.xml", this.ReadConfig);
        }
        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <returns></returns>
        public List<ModuleInfo> GetAllModules()
        {
            return this.configs;
        }
        /// <summary>
        /// 获取指定模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        public ModuleInfo GetModule(string moduleName)
        {
            if (this.configs != null)
                return this.configs.Find(t => t.Name == moduleName);
            else
                return null;
        }
        /// <summary>
        /// 获取指定模块指定图片的路径
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="imageName">图片名称</param>
        /// <returns></returns>
        public string GetImagePath(ModuleViews moduleName, string imageName)
        {
            if (this.configs == null)
            {
                Debug.LogError("<><ModuleConfig.GetImagePath>Parameter 'moduleInfos' is null");
                return null;
            }

            ModuleInfo moduleInfo = this.configs.Find(t => t.Name == moduleName.ToString("G"));
            if (moduleInfo == null || moduleInfo.Files == null)
            {
                Debug.LogErrorFormat("<><ModuleConfig.GetImagePath>Can't find module: {0}", moduleName);
                return null;
            }

            ModuleFile moduleFile = moduleInfo.Files.Find(t => t.Name == imageName);
            if (moduleFile == null)
            {
                Debug.LogErrorFormat("<><ModuleConfig.GetImagePath>Can't find image: {0}, module: {1}", imageName, moduleName);
                return null;
            }

            return string.Format("Texture/{0}", moduleFile.Path);
        }
        /// <summary>
        /// 获取配置文件是否已经加载完
        /// </summary>
        /// <returns></returns>
        public bool IsLoaded()
        {
            return this.isLoaded;
        }
    }
}
