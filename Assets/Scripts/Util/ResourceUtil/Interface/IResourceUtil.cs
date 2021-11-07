using System;
using System.Collections;
using UnityEngine;

namespace AppGame.Util
{
    public sealed class FailureInfo
    {
        public bool Interrupt { get; set; }
        public string Message { get; set; }
    }

    public interface IResourceUtil
    {
        /// <summary>
        /// 服务器根目录下资源文件目录的根目录
        /// </summary>
        string SERVER_ROOT_PATH { get; }
        /// <summary>
        /// 是否忽略要下载的文件在服务器上不存在(忽略[true]则不中断下载，否则[false]中断下载)
        /// </summary>
        bool IgnoreFileNotExisted { get; set; }
        /// <summary>
        /// 设置服务器地址
        /// </summary>
        /// <param name="urlPrefix">文件服务器下载地址前缀</param>
        void SetServerPath(string urlPrefix);
        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="fileName">配置文件中的相对路径(需要包括后缀名)</param>
        /// <returns>文件存在返回true，否则返回false</returns>
        bool FileExisted(string fileName);
        /// <summary>
        /// 从网络或缓存中获取资源文件(音频、图片、模型等)
        /// </summary>
        /// <param name="filePath">资源文件相对路径</param>
        /// <param name="success">成功时的回调</param>
        /// <param name="failure">失败时的回调</param>
        /// <param name="forcedDownload">是否强制下载(即使本地存在文件也下载并覆盖)</param>
        /// <returns></returns>
        IEnumerator LoadAsset(string filePath, Action<object> success = null, Action<FailureInfo> failure = null, bool forcedDownload = false);
        /// <summary>
        /// 从网络或缓存中获取音频
        /// </summary>
        /// <param name="audioPath">音频相对路径</param>    
        /// <param name="success">成功时的回调</param>
        /// <param name="failure">失败时的回调</param>
        /// <param name="forcedDownload">是否强制下载(即使本地存在文件也下载并覆盖)</param>
        /// <returns></returns>
        IEnumerator LoadAudio(string audioPath, Action<AudioClip> success = null, Action<FailureInfo> failure = null, bool forcedDownload = false);
        /// <summary>
        /// 从网络或缓存中获取图片
        /// </summary>
        /// <param name="imagePath">图片相对路径</param>    
        /// <param name="success">成功时的回调</param>
        /// <param name="failure">失败时的回调</param>
        /// <param name="forcedDownload">是否强制下载(即使本地存在文件也下载并覆盖)</param>
        /// <returns></returns>
        IEnumerator LoadSprite(string imagePath, Action<Sprite> success = null, Action<FailureInfo> failure = null, bool forcedDownload = false);
        /// <summary>
        /// 从网络或缓存中获取图片
        /// </summary>
        /// <param name="imagePath">图片相对路径</param>    
        /// <param name="success">成功时的回调</param>
        /// <param name="failure">失败时的回调</param>
        /// <param name="forcedDownload">是否强制下载(即使本地存在文件也下载并覆盖)</param>
        /// <returns></returns>
        IEnumerator LoadTexture(string imagePath, Action<Texture2D> success = null, Action<FailureInfo> failure = null, bool forcedDownload = false);
        /// <summary>
        /// 从网络或缓存中获取AssetBundle包
        /// </summary>
        /// <param name="assetBundlePath">AssetBundle包相对路径</param>    
        /// <param name="success">成功时的回调</param>
        /// <param name="failure">失败时的回调</param>
        /// <param name="forcedDownload">是否强制下载(即使本地存在文件也下载并覆盖)</param>
        /// <returns></returns>
        IEnumerator LoadAssetBundle(string assetBundlePath, Action<AssetBundle> success = null, Action<FailureInfo> failure = null, bool forcedDownload = false);
        /// <summary>
        /// 从网络或缓存中获取Manifest文件
        /// </summary>
        /// <param name="assetBundlePath">AssetBundle包相对路径</param>    
        /// <param name="success">成功时的回调</param>
        /// <param name="failure">失败时的回调</param>
        /// <param name="forcedDownload">是否强制下载(即使本地存在文件也下载并覆盖)</param>
        /// <returns></returns>
        IEnumerator LoadManifest(string assetBundlePath, Action<AssetBundleManifest> success = null, Action<FailureInfo> failure = null, bool forcedDownload = false);
        /// <summary>
        /// 删除本地文件
        /// </summary>
        /// <param name="filePath">文件相对路径</param>
        /// <param name="failure">删除成功的回调</param>
        /// <param name="success">删除失败的回调</param>
        void DeleteFile(string filePath, Action<string> success = null, Action<string> failure = null);
        /// <summary>
        /// 删除本地目录
        /// </summary>
        /// <param name="folderPath">目录相对路径</param>
        /// <param name="failure">删除成功的回调</param>
        /// <param name="success">删除失败的回调</param>
        void DeleteFolder(string folderPath, Action<string> success = null, Action<string> failure = null);
        /// <summary>
        /// 删除指定模块的资源文件(图片和音频)
        /// </summary>
        /// <param name="moduleName">模块名称</param>
        /// <param name="success">删除成功的回调</param>
        /// <param name="failure">删除失败的回调</param>
        void DeleteModule(string moduleName, Action<string> success = null, Action<string> failure = null);
        /// <summary>
        /// 从缓存中卸载指定的AssetBundle
        /// </summary>
        /// <param name="assetBundlePath">指定的AssetBundle的名字或路径</param>
        void UnloadAssetBundle(string assetBundlePath);
        /// <summary>
        /// 从缓存中卸载所有的AssetBundle
        /// </summary>
        void UnloadAllAssetBundles();
    }
}