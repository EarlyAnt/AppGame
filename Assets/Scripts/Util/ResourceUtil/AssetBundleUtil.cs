using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AppGame.Util
{
    class AssetBundleUtil : IAssetBundleUtil
    {
        [Inject]
        public IResourceUtil ResourceUtils { get; set; }
        private Dictionary<string, Coroutine> asyncOperations = new Dictionary<string, Coroutine>();
        private Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();
        private AssetBundleManifest manifest;
        private string platform
        {
            get
            {
#if UNITY_ANDROID && !UNITY_EDITOR
                return "Android";
#elif UNITY_IOS && !UNITY_EDITOR
                return "iOS";
#else
                return "Android";
#endif
            }
        }

        /// <summary>
        /// 加载manifest文件
        /// </summary>
        public IEnumerator LoadManifest()
        {
            string fileName = string.Format("{0}/Model/{1}/manifest", Application.persistentDataPath, platform);
            if (File.Exists(fileName))
            {//如果本地存在manifest文件，则直接加载
                AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile(fileName);
                if (manifestAssetBundle != null)
                {
                    this.manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                }
            }
            else
            {//否则，从服务器上获取manifest文件
                yield return this.ResourceUtils.LoadAssetBundle(string.Format("Model/{0}/manifest", platform), (manifestAssetBundle) =>
                {
                    if (manifestAssetBundle != null)
                    {
                        this.manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    }
                }, null);
            }
        }
        /// <summary>
        /// 从AssetBundle包中加载所需要的物体
        /// </summary>
        /// <param name="assetBundleName">AssetBundle包的名称</param>
        /// <param name="prefabName">预制体的名称</param>
        /// <param name="success">成功时的回调</param>
        /// <param name="failure">失败时的回调</param>
        public void LoadAssetAsync(string assetBundleName, string prefabName, Action<GameObject> success, Action<string> failure = null)
        {
            this.CheckAssetBundleBuffer(assetBundleName);
            this.LoadSharedAssets(assetBundleName, () =>
            {//从AssetBundle包中创建物体前，先加载此物体的依赖项资源

                if (!this.CheckAssetBundleBuffer(assetBundleName))
                {//如果缓存中不存在指定的AssetBundle包，则加载
                    string fileName = string.Format("{0}/Model/{1}/{2}.ab", Application.persistentDataPath, platform, assetBundleName);
                    if (File.Exists(fileName))
                    {//本地存在AssetBundle包文件，则直接加载
                        AssetBundle assetBundle = AssetBundle.LoadFromFile(fileName);
                        this.assetBundles.Add(assetBundleName, assetBundle);
                        this.LoadAsset(this.assetBundles[assetBundleName], prefabName, success, failure);
                    }
                    else
                    {//否则，从服务器上获取AssetBundle包文件
                        this.CollectAsyncOperation(assetBundleName, AsyncActionHelper.Instance.StartCoroutine(this.ResourceUtils.LoadAssetBundle(string.Format("Model/{0}/{1}.ab", platform, assetBundleName), (assetBundle) =>
                            {
                                if (assetBundle != null)
                                {
                                    this.assetBundles.Add(assetBundleName, assetBundle);
                                    this.LoadAsset(this.assetBundles[assetBundleName], prefabName, success, failure);
                                }
                            }, (failureInfo) =>
                            {
                                if (failure != null)
                                    failure(failureInfo.Message);
                                return;
                            })));
                    }
                }
                else
                {//否则直接使用缓存中的AssetBundle包文件
                    this.LoadAsset(this.assetBundles[assetBundleName], prefabName, success, failure);
                }
            });
        }
        /// <summary>
        /// 获取指定的AssetBundle包
        /// </summary>
        /// <param name="assetBundleName">AssetBundle包的名称</param>
        /// <param name="success">成功时的回调</param>
        /// <param name="failure">失败时的回调</param>
        public void LoadAssetBundleAsync(string assetBundleName, Action<AssetBundle> success, Action<string> failure = null)
        {
            this.CheckAssetBundleBuffer(assetBundleName);
            this.LoadSharedAssets(assetBundleName, () =>
            {//从AssetBundle包中创建物体前，先加载此物体的依赖项资源

                if (!this.CheckAssetBundleBuffer(assetBundleName))
                {//如果缓存中不存在指定的AssetBundle包，则加载
                    string fileName = string.Format("{0}/Model/{1}/{2}.ab", Application.persistentDataPath, platform, assetBundleName);
                    if (File.Exists(fileName))
                    {//本地存在AssetBundle包文件，则直接加载
                        AssetBundle assetBundle = AssetBundle.LoadFromFile(fileName);
                        this.assetBundles.Add(assetBundleName, assetBundle);
                        if (success != null) success(assetBundle);
                    }
                    else
                    {//否则，从服务器上获取AssetBundle包文件
                        this.CollectAsyncOperation(assetBundleName, AsyncActionHelper.Instance.StartCoroutine(this.ResourceUtils.LoadAssetBundle(string.Format("Model/{0}/{1}.ab", platform, assetBundleName), (assetBundle) =>
                            {
                                if (assetBundle != null)
                                {
                                    this.assetBundles.Add(assetBundleName, assetBundle);
                                    if (success != null) success(assetBundle);
                                }
                            }, (failureInfo) =>
                            {
                                if (failure != null)
                                    failure(failureInfo.Message);
                                return;
                            })));
                    }
                }
                else
                {//否则直接返回缓存中的AssetBundle包文件
                    if (success != null) success(this.assetBundles[assetBundleName]);
                }
            });
        }
        /// <summary>
        /// 停止加载AssetBundle及其中的物体
        /// </summary>
        /// <param name="assetBundleName">AssetBundle包的名称</param>
        public void StopLoadAsset(string assetBundleName)
        {
            if (this.asyncOperations.ContainsKey(assetBundleName) && this.asyncOperations[assetBundleName] != null)
            {
                AsyncActionHelper.Instance.StopCoroutine(this.asyncOperations[assetBundleName]);
                this.asyncOperations.Remove(assetBundleName);
            }
        }
        /// <summary>
        /// 加载manifest文件
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="callback">加载manifest文件及依赖项资源后的回调</param>
        private void LoadSharedAssets(string assetBundleName, Action callback)
        {
            if (this.manifest == null)
            {//加载manifest文件
                string fileName = string.Format("{0}/Model/{1}/manifest", Application.persistentDataPath, platform);
                if (File.Exists(fileName))
                {//如果本地存在manifest文件，则直接加载
                    AssetBundle manifestAssetBundle = AssetBundle.LoadFromFile(fileName);
                    if (manifestAssetBundle != null)
                    {
                        this.manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                        this.LoadSharedAssetBundle(assetBundleName, callback);//根据manifest文件中描述的依赖项，加载依赖项的AssetBundle
                    }
                }
                else
                {//否则，从服务器上获取manifest文件
                    this.CollectAsyncOperation(assetBundleName, AsyncActionHelper.Instance.StartCoroutine(this.ResourceUtils.LoadAssetBundle(string.Format("Model/{0}/manifest", platform), (manifestAssetBundle) =>
                    {
                        if (manifestAssetBundle != null)
                        {
                            this.manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                            this.LoadSharedAssetBundle(assetBundleName, callback);//根据manifest文件中描述的依赖项，加载依赖项的AssetBundle
                        }
                    }, null)));
                }
            }
            else
            {
                this.LoadSharedAssetBundle(assetBundleName, callback);//根据manifest文件中描述的依赖项，加载依赖项的AssetBundle
            }
        }
        /// <summary>
        /// 加载依赖项资源
        /// </summary>
        /// <param name="assetBundleName"></param>
        private void LoadSharedAssetBundle(string assetBundleName, Action callback)
        {
            if (this.manifest != null)
            {
                string[] dependencies = manifest.GetAllDependencies(string.Format("{0}.ab", assetBundleName));
                if (dependencies != null && dependencies.Length > 0)
                {
                    List<string> needToLoadDependencies = new List<string>();
                    needToLoadDependencies.AddRange(dependencies);//依赖项资源计数器(计数缓存)
                    foreach (string dependency in dependencies)
                    {//第一次遍历，仅加载本地存在的依赖项资源，并计数
                        if (!this.CheckAssetBundleBuffer(dependency))
                        {
                            if (File.Exists(string.Format("{0}/Model/{1}/{2}", Application.persistentDataPath, platform, dependency)))
                            {//如果本地存在manifest文件，则直接加载
                                AssetBundle dependentAssetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/{1}/{2}", Application.persistentDataPath, platform, dependency));
                                if (dependentAssetBundle != null)
                                {
                                    this.assetBundles.Add(dependency, dependentAssetBundle);
                                    needToLoadDependencies.Remove(dependency);//成功从本地加载依赖项资源，从计数器中移除此依赖项的注册
                                }
                            }
                        }
                        else needToLoadDependencies.Remove(dependency);//本地缓存中已有此依赖项资源，从计数器中移除此依赖项的注册
                    }

                    if (needToLoadDependencies.Count > 0)
                    {//如果存在本地没有的依赖项资源，则进行第一次遍历，从服务器中获取
                        string[] dependencyArray = needToLoadDependencies.ToArray();
                        for (int i = 0; i < dependencyArray.Length; i++)
                        {
                            string dependency = dependencyArray[i];
                            if (!this.CheckAssetBundleBuffer(dependency))
                            {//否则，从服务器上获取manifest文件
                                this.CollectAsyncOperation(assetBundleName, AsyncActionHelper.Instance.StartCoroutine(this.ResourceUtils.LoadAssetBundle(string.Format("Model/{0}/{1}", platform, dependency), (dependentAssetBundle) =>
                                {
                                    if (dependentAssetBundle != null)
                                    {
                                        Debug.LogFormat("<><AssetBundleUtils.LoadSharedAssetBundle>Remote dependency: {0}, {1}", dependency, dependentAssetBundle.name);
                                        this.assetBundles.Add(dependency, dependentAssetBundle);
                                        needToLoadDependencies.Remove(dependency);
                                        if ((needToLoadDependencies.Count == 0 || i + 1 == dependencyArray.Length) && callback != null)
                                            AsyncActionHelper.Instance.QueueOnMainThread(callback);//如果剩余的依赖项也已经从服务器中加载完毕，或者第二次遍历已经结束，则直接执行回调
                                    }
                                }, null)));
                            }
                        }
                    }
                    else if (callback != null) callback();
                }
                else if (callback != null) callback();
            }
        }
        /// <summary>
        /// 从AssetBundle包中实例化资源
        /// </summary>
        /// <param name="assetBundle">AssetBundle包</param>
        /// <param name="prefabName">预制体名称</param>
        /// <param name="failure">失败时的回调</param>
        /// <returns></returns>
        private void LoadAsset(AssetBundle assetBundle, string prefabName, Action<GameObject> success, Action<string> failure)
        {
            if (assetBundle == null && failure != null)
            {
                string errorText = "asset bundle is null";
                Debug.LogErrorFormat("<><AssetBundleUtils.LoadAsset>{0}", errorText);
                if (failure != null) failure(errorText);
                return;
            }

            try
            {
                GameObject prefab = assetBundle.LoadAsset(prefabName) as GameObject;
                if (prefab != null)
                {
                    prefab.SetActive(false);
                    GameObject gameObject = GameObject.Instantiate(prefab) as GameObject;
                    if (gameObject != null)
                    {
                        gameObject.SetActive(false);
                        if (success != null)
                            success(gameObject);
                    }
                    else if (gameObject == null)
                    {
                        string errorText = string.Format("asset bundle [{0}] can't load the prefab '{1}'", assetBundle.name, prefabName);
                        Debug.LogErrorFormat("<><AssetBundleUtils.LoadAssetAsync>{0}", errorText);
                        if (failure != null) failure(errorText);
                    }
                }
                else
                {
                    string errorText = string.Format("asset bundle [{0}] can't load the prefab '{1}'", assetBundle.name, prefabName);
                    Debug.LogErrorFormat("<><AssetBundleUtils.LoadAssetAsync>{0}", errorText);
                    if (failure != null) failure(errorText);
                }
                //this.PrintAssetBundles();
            }
            catch (Exception ex)
            {
                string errorText = string.Format("Unknown error: {0}", ex.Message);
                Debug.LogErrorFormat("<><AssetBundleUtils.LoadAsset>{0}", errorText);
                if (failure != null) failure(errorText);
            }
        }
        /// <summary>
        /// 卸载AssetBundle包
        /// </summary>
        /// <param name="assetBundleName">AssetBundle包的名称</param>
        public void UnloadAsset(string assetBundleName)
        {
            if (!string.IsNullOrEmpty(assetBundleName) && this.assetBundles.ContainsKey(assetBundleName))
            {
                if (this.assetBundles[assetBundleName] != null)
                    this.assetBundles[assetBundleName].Unload(true);
                this.assetBundles.Remove(assetBundleName);
                this.ResourceUtils.UnloadAssetBundle(assetBundleName);
            }
        }
        /// <summary>
        /// 校验AssetBundle缓存池(如果已加载的AssetBundle为空，则视为已被回收，需要重新加载)
        /// </summary>
        /// <param name="assetBundleName"></param>
        private bool CheckAssetBundleBuffer(string assetBundleName)
        {
            if (this.assetBundles.ContainsKey(assetBundleName))
            {
                if (this.assetBundles[assetBundleName] == null)
                {
                    this.assetBundles.Remove(assetBundleName);
                    return false;
                }
                else return true;
            }
            else return false;
        }
        /// <summary>
        /// 收集异步操作协程
        /// </summary>
        /// <param name="assetBundleName">AssetBundle包的名称</param>
        /// <param name="coroutine">异步操作协程</param>
        /// <returns></returns>
        private void CollectAsyncOperation(string assetBundleName, Coroutine coroutine)
        {
            if (!this.asyncOperations.ContainsKey(assetBundleName))
                this.asyncOperations.Add(assetBundleName, coroutine);
            else
                this.asyncOperations[assetBundleName] = coroutine;
        }
        /// <summary>
        /// 打印AssetBundle缓存池
        /// </summary>
        protected void PrintAssetBundles()
        {
            StringBuilder strContent = new StringBuilder();
            if (this.assetBundles != null && this.assetBundles.Count > 0)
            {
                Dictionary<string, AssetBundle> buffer = this.assetBundles.OrderBy(p => p.Key).ToDictionary(p => p.Key, q => q.Value);
                foreach (var kvp in buffer)
                {
                    strContent.AppendFormat("AssetBundle: {0}, {1}\n", kvp.Key, kvp.Value == null ? "null" : kvp.Value.name);
                }
            }
            Debug.LogFormat("<><AssetBundleUtils.PrintAssetBundles>\n{0}", strContent.ToString());
        }
    }
}