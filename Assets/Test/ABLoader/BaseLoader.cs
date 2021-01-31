using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ABLoader
{
    public abstract class BaseLoader : MonoBehaviour
    {
        [SerializeField]
        protected string assetBundleName;
        [SerializeField]
        private bool autoLoad;
        [SerializeField]
        private BaseLoader next;
        protected AssetBundle assetBundle;
        protected GameObject gameObject;
        private static Dictionary<string, AssetBundle> assetBundles = new Dictionary<string, AssetBundle>();
        protected static AssetBundle manifestAssetBundle;
        protected static StringBuilder strContent = new StringBuilder();

        protected virtual void Start()
        {
            this.LoadManifest();
            if (this.autoLoad)
            {
                this.LoadAB();
            }
        }

        public virtual void LoadAB()
        {
            if (this.next != null)
                this.next.LoadAB();
        }

        protected void LoadManifest()
        {
            if (manifestAssetBundle == null)
            {
                manifestAssetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/Android", Application.persistentDataPath));
            }
        }

        protected AssetBundle GetAssetBundle(string assetBundleName)
        {
            if (assetBundles.ContainsKey(assetBundleName) && assetBundles[assetBundleName] == null)
                assetBundles.Remove(assetBundleName);

            if (!assetBundles.ContainsKey(assetBundleName))
            {
                AssetBundle assetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/{1}", Application.persistentDataPath, this.assetBundleName));
                assetBundles.Add(assetBundleName, assetBundle);

                if (manifestAssetBundle != null)
                {
                    AssetBundleManifest manifest = manifestAssetBundle.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
                    if (manifest != null)
                    {
                        string[] dependencies = manifest.GetAllDependencies(this.assetBundleName);
                        foreach (string dependency in dependencies)
                        {
                            if (!assetBundles.ContainsKey(dependency))
                            {
                                AssetBundle dependencyAssetBundle = AssetBundle.LoadFromFile(string.Format("{0}/Model/{1}", Application.persistentDataPath, dependency));
                                assetBundles.Add(dependency, dependencyAssetBundle);
                            }
                        }
                    }
                }
            }
            this.PrintAssetBundles();
            return assetBundles[assetBundleName];
        }

        protected void PrintAssetBundles()
        {
            strContent.Remove(0, strContent.Length);
            if (assetBundles != null && assetBundles.Count > 0)
            {
                Dictionary<string, AssetBundle> buffer = assetBundles.OrderBy(p => p.Key).ToDictionary(p => p.Key, q => q.Value);
                foreach (var assetBundle in buffer)
                {
                    strContent.AppendFormat("AssetBundle: {0}\n", assetBundle.Key);
                }
            }
            Debug.LogFormat("<><BaseLoader.PrintAssetBundles>\n{0}", strContent.ToString());
        }

        [ContextMenu("1-卸载AB包中未使用的资源")]
        protected void UnloadAssetBundle1()
        {
            if (this.assetBundle != null)
                this.assetBundle.Unload(false);
        }

        [ContextMenu("2-卸载AB包中的所有资源")]
        protected void UnloadAssetBundle2()
        {
            if (this.assetBundle != null)
                this.assetBundle.Unload(true);
        }

        [ContextMenu("3-销毁创建的物体")]
        protected void DestroyObject()
        {
            if (this.gameObject != null)
                GameObject.Destroy(this.gameObject);
            Resources.UnloadUnusedAssets();
        }
    }
}