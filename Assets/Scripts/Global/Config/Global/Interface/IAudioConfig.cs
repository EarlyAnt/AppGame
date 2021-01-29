using System.Collections.Generic;

namespace AppGame.Config
{
    /// <summary>
    /// 音频配置文件读取接口
    /// </summary>
    public interface IAudioConfig : IConfig
    {
        /// <summary>
        /// 加载音频配置数据
        /// </summary>
        void Load();
        /// <summary>
        /// 获取所有音频
        /// </summary>
        /// <returns></returns>
        List<Audio> GetAllAudios();
        /// <summary>
        /// 获取指定名称的音频
        /// </summary>
        /// <param name="name">音频名称</param>
        /// <returns></returns>
        Audio GetAudio(string name);
    }

    public class Audio
    {
        public string Name { get; set; }
        public string Storage { get; set; }
        public string Des { get; set; }
        public List<AudioFile> Files { get; set; }
        public Audio() { this.Files = new List<AudioFile>(); }
    }

    public class AudioFile
    {
        public string Path { get; set; }
    }
}
