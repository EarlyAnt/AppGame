using System.Collections.Generic;

namespace AppGame.Config
{
    /// <summary>
    /// 多语言资源配置接口
    /// </summary>
    public interface II18NConfig : IConfig
    {
        /// <summary>
        /// 加载资源配置数据
        /// </summary>
        void Load();
        /// <summary>
        /// 获取所有资源配置
        /// </summary>
        /// <returns></returns>
        Dictionary<string, ResConfig> GetAllResConfig();
        /// <summary>
        /// 获取指定语言资源配置
        /// </summary>
        /// <param name="languageShortName">语言简称</param>
        /// <returns></returns>
        ResConfig GetResConfig(string languageShortName);
        /// <summary>
        /// 获取文本
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string GetText(string key);
        /// <summary>
        /// 获取文本配置
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        TextElement GetTextElement(string key);
        /// <summary>
        /// 获取图片文件路径
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="languageShortName">指定语言</param>
        /// <returns></returns>
        string GetImagePath(string key, string languageShortName = "");
        /// <summary>
        /// 获取图片文件所有语言的路径
        /// </summary>
        /// <param name="key">键</param>        
        /// <returns></returns>
        List<string> GetImagePaths(string key);
        /// <summary>
        /// 获取Spine动画中的动画名称
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        string GetAnimationName(string key);
        /// <summary>
        /// 获取音频文件路径
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="languageShortName">指定语言</param>
        /// <returns></returns>
        string GetAudioPath(string key, string languageShortName = "");
        /// <summary>
        /// 获取音频文件所有语言的路径
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        List<string> GetAudioPaths(string key);
        /// <summary>
        /// 打印指定语言的所有配置数据
        /// </summary>
        /// <param name="languageShortName">语言简称</param>
        void PrintAllData(string languageShortName);
    }

    #region 自定义类
    public class ResConfig
    {
        public string Language { get; set; }
        public List<TextElement> TextElements { get; set; }
        public List<ImageElement> ImageElements { get; set; }
        public List<AnimationElement> AnimationElements { get; set; }
        public List<AudioElement> AudioElements { get; set; }

        public ResConfig()
        {
            this.TextElements = new List<TextElement>();
            this.ImageElements = new List<ImageElement>();
            this.AnimationElements = new List<AnimationElement>();
            this.AudioElements = new List<AudioElement>();
        }
    }
    public abstract class ResElement
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
    public class TextElement : ResElement
    {
        public string Text
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Value))
                {
                    string[] parts = this.Value.Split('|');
                    return parts != null && parts.Length == 2 ? parts[0] : "";
                }
                else return "";
            }
        }
        public string FontShortName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.Value))
                {
                    string[] parts = this.Value.Split('|');
                    return parts != null && parts.Length == 2 ? parts[1] : FontConfig.DEFAULT_FONT;
                }
                else return "";
            }
        }
    }
    public class ImageElement : ResElement
    {
    }
    public class AnimationElement : ResElement
    {
    }
    public class AudioElement : ResElement
    {
    }
    #endregion
}
