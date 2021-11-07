using System.Collections.Generic;

namespace AppGame.Config
{
    /// <summary>
    /// 语言类
    /// </summary>
    public class Language
    {
        /// <summary>
        /// 语言名称(简称)
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 语言显示文本(如“中文繁體”)
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// 是否为默认语言
        /// </summary>
        public bool Default { get; set; }
    }

    /// <summary>
    /// 语言配置文件读取接口
    /// </summary>
    public interface ILanConfig : IConfig
    {
        /// <summary>
        /// 加载语言配置数据
        /// </summary>
        void Load();
        /// <summary>
        /// 获取语言文本
        /// </summary>
        /// <param name="languageShortName">语言简称</param>
        /// <returns></returns>
        string GetLanText(string languageShortName);
        /// <summary>
        /// 判断语言是否为默认语言
        /// </summary>
        /// <param name="languageShortName">语言简称</param>
        /// <returns></returns>
        bool IsDefault(string languageShortName);
        /// <summary>
        /// 判断语言简称是否合法
        /// </summary>
        /// <param name="languageShortName"></param>
        /// <returns></returns>
        bool IsValid(string languageShortName);
        /// <summary>
        ///获取所有语言的简称
        /// </summary>
        /// <returns></returns>
        List<string> GetAllLanShortName();
        /// <summary>
        /// 获取所有语言文本
        /// </summary>
        /// <returns></returns>
        List<string> GetAllLanText();
    }
}
