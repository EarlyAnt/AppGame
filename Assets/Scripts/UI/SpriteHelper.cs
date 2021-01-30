using AppGame.Config;
using AppGame.UI;
using AppGame.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// UI图片缓存工具
/// </summary>
public class SpriteHelper : BaseView
{
    /************************************************属性与变量命名************************************************/
    public static SpriteHelper Instance { get; private set; }
    [Inject]
    public IModuleConfig ModuleConfig { get; set; }
    [Inject]
    public IResourceUtils ResourceUtils { get; set; }
    [SerializeField, Range(0f, 60f)]
    private float GCDelaySeconds = 10f;
    private List<ModuleInfo> moduleInfos = new List<ModuleInfo>();
    private Dictionary<ModuleViews, Dictionary<string, Sprite>> buffer = new Dictionary<ModuleViews, Dictionary<string, Sprite>>();
    private Dictionary<ModuleViews, Coroutine> clearBufferCoroutines = new Dictionary<ModuleViews, Coroutine>();
    /************************************************Unity方法与事件***********************************************/
    protected override void Awake()
    {
        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
        this.StartCoroutine(this.LoadAllModuleInfos());
    }
    /************************************************自 定 义 方 法************************************************/
    private IEnumerator LoadAllModuleInfos()
    {
        while (this.ModuleConfig == null)
            yield return null;

        this.moduleInfos = this.ModuleConfig.GetAllModules();
        if (this.moduleInfos == null)
            Debug.LogError("<><SpriteHelper.LoadModuleInfo>moduleInfos is null");
    }

    public void LoadModuleSprites(ModuleViews moduleName)
    {
        ModuleInfo moduleInfo = this.moduleInfos.Find(t => t.Name == moduleName.ToString("G"));
        if (moduleInfo != null)
        {
            foreach (ModuleFile file in moduleInfo.Files)
                this.LoadTexture(moduleName, string.Format("Texture/{0}.png", file.Path));
        }
        else
        {
            Debug.LogErrorFormat("<><SpriteHelper.GetAssetPath>Module config is null, '{0}'", moduleName);
        }
    }

    public void LoadTexture(ModuleViews moduleName, string imagePath, System.Action<Sprite> success = null, System.Action<string> failure = null)
    {
        this.StopClearBuffer(moduleName);
        if (!this.HasImage(moduleName, imagePath))
        {
            this.StartCoroutine(this.ResourceUtils.LoadTexture(imagePath,
                                           (sprite) =>
                                           {
                                               this.RegisterImage(moduleName, imagePath, sprite);
                                               if (success != null) success(sprite);
                                           },
                                           (failureInfo) => Debug.LogErrorFormat("<><SpriteHelper.LoadTexture>Unknown error: {0}", failureInfo.Message)));
        }
        else if (success != null) success(this.LoadTextureFromBuffer(imagePath));
    }

    public void LoadTextureByName(ModuleViews moduleName, string imageName, System.Action<Sprite> success = null, System.Action<string> failure = null)
    {
        this.StopClearBuffer(moduleName);

        string imagePath = this.GetAssetPath(moduleName, imageName);
        if (string.IsNullOrEmpty(imagePath))
        {
            string errorText = "Can't not find image from buffer";
            Debug.LogErrorFormat("<><SpriteHelper.LoadTextureByName>{0}", errorText);
            if (failure != null) failure(errorText);
        }

        if (!this.HasImage(moduleName, imagePath))
        {
            this.StartCoroutine(this.ResourceUtils.LoadTexture(imagePath,
                                           (sprite) =>
                                           {
                                               this.RegisterImage(moduleName, imagePath, sprite);
                                               if (success != null) success(sprite);
                                           },
                                           (failureInfo) => Debug.LogErrorFormat("<><SpriteHelper.LoadTextureByName>Unknown error: {0}", failureInfo.Message)));
        }
        else if (success != null) success(this.LoadTextureFromBuffer(imagePath));
    }

    public string GetAssetPath(ModuleViews moduleName, string assetName, string extension = ".png")
    {
        ModuleInfo moduleInfo = this.moduleInfos.Find(t => t.Name == moduleName.ToString("G"));
        if (moduleInfo != null)
        {
            ModuleFile file = moduleInfo.Files.Find(t => t.Name == assetName);
            if (file != null)
            {
                return file.Path + extension;
            }
            else
            {
                Debug.LogErrorFormat("<><SpriteHelper.GetAssetPath>Module file not exist, '{0}'", assetName);
                return "";
            }
        }
        else
        {
            Debug.LogErrorFormat("<><SpriteHelper.GetAssetPath>Module config is null, '{0}'", moduleName);
            return "";
        }
    }

    public void RegisterImage(ModuleViews moduleName, string imagePath, Sprite image)
    {
        //Debug.LogFormat("<><SpriteHelper.RegisterImage>ModuleName: {0}, ImagePath: {1}", moduleName, imagePath);
        if (this.buffer != null)
        {
            if (!this.buffer.ContainsKey(moduleName))
                this.buffer.Add(moduleName, new Dictionary<string, Sprite>());

            if (!this.buffer[moduleName].ContainsKey(imagePath))
                this.buffer[moduleName].Add(imagePath, image);
        }
    }

    public Sprite LoadTextureFromBuffer(string imagePath)
    {
        Sprite sprite = null;
        foreach (var kvp in this.buffer)
        {
            sprite = this.LoadTextureFromBuffer(kvp.Key, imagePath);
            if (sprite != null) return sprite;
        }
        return null;
    }

    public Sprite LoadTextureFromBuffer(ModuleViews moduleName, string imagePath, string extension = ".png")
    {
        if (!string.IsNullOrEmpty(imagePath) && !imagePath.ToLower().EndsWith(extension))
            imagePath += extension;

        if (this.buffer != null && this.buffer.ContainsKey(moduleName) &&
            this.buffer[moduleName].ContainsKey(imagePath))
            return this.buffer[moduleName][imagePath];
        else
            return null;
    }

    private bool HasImage(ModuleViews moduleName, string imagePath)
    {
        if (this.buffer != null && this.buffer.ContainsKey(moduleName) &&
            this.buffer[moduleName].ContainsKey(imagePath))
            return true;
        else
            return false;
    }

    public void ClearBuffer(ModuleViews moduleName)
    {
        if (this.clearBufferCoroutines != null && this.gameObject.activeInHierarchy)
        {
            if (this.clearBufferCoroutines.ContainsKey(moduleName))
            {
                if (this.clearBufferCoroutines[moduleName] != null)
                    this.StopCoroutine(this.clearBufferCoroutines[moduleName]);
                this.clearBufferCoroutines[moduleName] = this.StartCoroutine(this.DelayClearBuffer(moduleName));
            }
            else
            {
                this.clearBufferCoroutines.Add(moduleName, this.StartCoroutine(this.DelayClearBuffer(moduleName)));
            }
        }
    }

    private void StopClearBuffer(ModuleViews moduleName)
    {
        if (this.clearBufferCoroutines != null && this.clearBufferCoroutines.ContainsKey(moduleName) &&
            this.clearBufferCoroutines[moduleName] != null)
        {
            this.StopCoroutine(this.clearBufferCoroutines[moduleName]);
            this.clearBufferCoroutines.Remove(moduleName);
        }
    }

    private IEnumerator DelayClearBuffer(ModuleViews moduleName)
    {
        yield return new WaitForSeconds(this.GCDelaySeconds);
        if (this.buffer != null && this.buffer.ContainsKey(moduleName))
        {
            foreach (var moduleFile in this.buffer[moduleName])
            {
                GameObject.Destroy(moduleFile.Value);
            }
            this.buffer.Remove(moduleName);
            Resources.UnloadUnusedAssets();
        }
    }
}