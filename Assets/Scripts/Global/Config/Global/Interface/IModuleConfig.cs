using System.Collections.Generic;
using System.Linq;

namespace AppGame.Config
{
    /// <summary>
    /// 模块配置接口
    /// </summary>
    public interface IModuleConfig : IConfig
    {
        /// <summary>
        /// 加载模块数据
        /// </summary>
        void Load();
        /// <summary>
        /// 获取所有模块
        /// </summary>
        /// <returns></returns>
        List<ModuleInfo> GetAllModules();
        /// <summary>
        /// 获取指定模块
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <returns></returns>
        ModuleInfo GetModule(string moduleName);
        /// <summary>
        /// 获取指定模块指定图片的路径
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="imageName">图片名称</param>
        /// <returns></returns>
        string GetImagePath(ModuleViews moduleName, string imageName);
    }

    #region 自定义类
    public static class Modules
    {
        public const string PET_DRESS = "PetDress";
        public const string PET_FEED = "PetFeed";
    }

    public class ModuleInfo
    {
        public string Name { get; set; }
        public List<ModuleFile> Files { get; set; }

        public ModuleInfo()
        {
            this.Files = new List<ModuleFile>();
        }
        public override string ToString()
        {
            System.Text.StringBuilder strbContent = new System.Text.StringBuilder();
            strbContent.AppendFormat("ModuleInfo, Name: {0}, ", this.Name);
            if (this.Files.Count > 0)
            {
                foreach (var file in this.Files)
                    strbContent.AppendFormat("[{0}], ", file);
            }

            if (strbContent.Length > 2)
                strbContent = strbContent.Remove(strbContent.Length - 2, 2);

            return strbContent.ToString();
        }
    }

    public enum FileTypes
    {
        Sprite = 0,
        Texture1 = 1,
        Texture2 = 2,
        Spine = 3
    }

    public class ModuleFile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public string AB { get; set; }
        public FileTypes FileType { get; set; }
        public bool Enable { get; set; }

        public override string ToString()
        {
            return string.Format("ModuleFile, Name: {0}, Path: {1}, AB = {2}, Type: {3}", this.Name, this.Path, this.AB, this.FileType);
        }
    }
    #endregion
}
