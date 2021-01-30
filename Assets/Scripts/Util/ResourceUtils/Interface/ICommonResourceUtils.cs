using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AppGame.Util
{
    public interface ICommonResourceUtils
    {
        /// <summary>
        /// 检查资源文件
        /// </summary>
        /// <param name="callback">检查结果回调(true-有文件需要下载，false-无文件需要下载或有错误)</param>
        void CheckResources(Action<bool> callback);
        /// <summary>
        /// 获取需要更新的文件列表
        /// </summary>
        /// <returns></returns>
        List<AssetFile> GetUpdateFileList();
        /// <summary>
        /// 获取根据本地各个配置文件统计出来的所有资源文件的列表
        /// </summary>
        /// <returns></returns>
        List<AssetFile> GetAllFileList();
        /// <summary>
        /// 检查所有模块的文件是否缺失
        /// </summary>
        /// <returns></returns>
        bool CheckAllModule();
        /// <summary>
        /// 检查指定模块的文件是否缺失
        /// </summary>
        /// <param name="view">模块页</param>
        /// <returns></returns>
        bool CheckModule(ModuleViews view);
    }

    /// <summary>
    /// 文件类型
    /// </summary>
    public static class FileTypes
    {
        public const string AUDIO = ".mp3";
        public const string SPRITE = ".png";
        public const string TEXTURE = ".png";
        public const string ASSET_BUNDLE = ".ab";
        public const string MANIFEST = ".manifest";
        /// <summary>
        /// 判断指定的文件是否是AudioClip文件(.mp3)
        /// </summary>
        /// <param name="fileName">指定文件的文件名</param>
        /// <returns></returns>
        public static bool IsAudio(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                FileInfo fileInfo = new FileInfo(fileName);
                return fileInfo != null && fileInfo.Extension == AUDIO;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><FileTypes.IsAudio>Error: {0}", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 判断指定的文件是否是Sprite文件(.png)
        /// </summary>
        /// <param name="fileName">指定文件的文件名</param>
        /// <returns></returns>
        public static bool IsSprite(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                FileInfo fileInfo = new FileInfo(fileName);
                return fileInfo != null && fileInfo.Extension == SPRITE;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><FileTypes.IsSprite>Error: {0}", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 判断指定的文件是否是Texture文件(.png)
        /// </summary>
        /// <param name="fileName">指定文件的文件名</param>
        /// <returns></returns>
        public static bool IsTexture(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                FileInfo fileInfo = new FileInfo(fileName);
                return fileInfo != null && fileInfo.Extension == SPRITE;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><FileTypes.IsTexture>Error: {0}", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 判断指定的文件是否是AssetBundle文件(.ab)
        /// </summary>
        /// <param name="fileName">指定文件的文件名</param>
        /// <returns></returns>
        public static bool IsAssetBundle(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                FileInfo fileInfo = new FileInfo(fileName);
                return fileInfo != null && fileInfo.Extension == ASSET_BUNDLE;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><FileTypes.IsAssetBundle>Error: {0}", ex.Message);
                return false;
            }
        }
        /// <summary>
        /// 判断指定的文件是否是Manifest文件(.manifest)
        /// </summary>
        /// <param name="fileName">指定文件的文件名</param>
        /// <returns></returns>
        public static bool IsManifest(string fileName)
        {
            try
            {
                if (string.IsNullOrEmpty(fileName))
                    return false;

                FileInfo fileInfo = new FileInfo(fileName);
                return (fileInfo != null && fileInfo.Extension == MANIFEST) || fileInfo.Name.ToLower() == "android";
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><FileTypes.IsManifest>Error: {0}", ex.Message);
                return false;
            }
        }
    }

    /// <summary>
    /// 资源文件
    /// </summary>
    public sealed class AssetFile
    {
        /// <summary>
        /// 不带文件后缀名的相对路径(某配置文件中配置的，或几个配置文件中的配置组合出来的相对路径)
        /// </summary>
        public string Path { get; set; }
        /// <summary>
        /// 绝对路径(经过拼接得到本地路径或服务器路径)
        /// </summary>
        public string FullPath { get; set; }
        /// <summary>
        /// 文件的MD5码
        /// </summary>
        public string MD5 { get; set; }
        /// <summary>
        /// 是否有新版本(true-服务器上有新版本文件，强制下载；false-服务器上没有新版本文件，不强制下载)
        /// </summary>
        public bool HasNewVersion { get; set; }
        /// <summary>
        /// 获取文件类型(后缀名)
        /// </summary>
        /// <returns>返回文件后缀名或空字符串</returns>
        public string GetFileType()
        {
            try
            {
                if (string.IsNullOrEmpty(this.FullPath))
                    return string.Empty;

                FileInfo fileInfo = new FileInfo(this.FullPath);
                return fileInfo != null ? fileInfo.Extension : string.Empty;
            }
            catch (Exception ex)
            {
                Debug.LogErrorFormat("<><FileTypes.GetFileType>Error: {0}", ex.Message);
                return string.Empty;
            }
        }
        /// <summary>
        /// 输出自描述信息
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Path: {0}, FullPath: {1}, MD5: {2}, FileType: {3}", this.Path, this.FullPath, this.MD5, this.GetFileType());
        }
    }
}