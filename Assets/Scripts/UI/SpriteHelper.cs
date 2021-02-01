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
    public IResourceUtil ResourceUtils { get; set; }
    [SerializeField, Range(0f, 60f)]
    private float GCDelaySeconds = 10f;
    private List<ModuleInfo> moduleInfos = new List<ModuleInfo>();
    private Dictionary<ModuleViews, Dictionary<string, Sprite>> spriteBuffer = new Dictionary<ModuleViews, Dictionary<string, Sprite>>();
    private Dictionary<ModuleViews, Dictionary<string, Texture2D>> textureBuffer = new Dictionary<ModuleViews, Dictionary<string, Texture2D>>();
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
    public void LoadModuleImages(ModuleViews moduleName)
    {
        ModuleInfo moduleInfo = this.moduleInfos.Find(t => t.Name == moduleName.ToString("G"));
        if (moduleInfo != null)
        {
            foreach (ModuleFile file in moduleInfo.Files)
            {
                if (file.FileType == AppGame.Config.FileTypes.Sprite)
                    this.LoadSprite(moduleName, string.Format("Texture/{0}.png", file.Path));
                else if (file.FileType == AppGame.Config.FileTypes.Texture1)
                    this.LoadTexture(moduleName, string.Format("Texture/{0}.png", file.Path));
            }
        }
        else
        {
            Debug.LogErrorFormat("<><SpriteHelper.GetAssetPath>Module config is null, '{0}'", moduleName);
        }
    }
    private bool HasImage(ModuleViews moduleName, string imagePath)
    {
        if (this.spriteBuffer != null && this.spriteBuffer.ContainsKey(moduleName) &&
            this.spriteBuffer[moduleName].ContainsKey(imagePath))
            return true;
        else if (this.textureBuffer != null && this.textureBuffer.ContainsKey(moduleName) &&
            this.textureBuffer[moduleName].ContainsKey(imagePath))
            return true;
        else
            return false;
    }

    public void LoadSprite(ModuleViews moduleName, string imagePath, System.Action<Sprite> success = null, System.Action<string> failure = null)
    {
        this.StopClearBuffer(moduleName);
        if (!this.HasImage(moduleName, imagePath))
        {
            this.StartCoroutine(this.ResourceUtils.LoadSprite(imagePath,
                                           (sprite) =>
                                           {
                                               this.RegisterSprite(moduleName, imagePath, sprite);
                                               if (success != null) success(sprite);
                                           },
                                           (failureInfo) => Debug.LogErrorFormat("<><SpriteHelper.LoadSprite>Unknown error: {0}", failureInfo.Message)));
        }
        else if (success != null) success(this.LoadSpriteFromBuffer(imagePath));
    }
    public Sprite LoadSpriteFromBuffer(string imagePath)
    {
        Sprite sprite = null;
        foreach (var kvp in this.spriteBuffer)
        {
            sprite = this.LoadSpriteFromBuffer(kvp.Key, imagePath);
            if (sprite != null) return sprite;
        }
        return null;
    }
    public Sprite LoadSpriteFromBuffer(ModuleViews moduleName, string imagePath, string extension = ".png")
    {
        if (!string.IsNullOrEmpty(imagePath) && !imagePath.ToLower().EndsWith(extension))
            imagePath += extension;

        if (this.spriteBuffer != null && this.spriteBuffer.ContainsKey(moduleName) &&
            this.spriteBuffer[moduleName].ContainsKey(imagePath))
            return this.spriteBuffer[moduleName][imagePath];
        else
            return null;
    }
    public void RegisterSprite(ModuleViews moduleName, string imagePath, Sprite image)
    {
        //Debug.LogFormat("<><SpriteHelper.RegisterSprite>ModuleName: {0}, ImagePath: {1}", moduleName, imagePath);
        if (this.spriteBuffer != null)
        {
            if (!this.spriteBuffer.ContainsKey(moduleName))
                this.spriteBuffer.Add(moduleName, new Dictionary<string, Sprite>());

            if (!this.spriteBuffer[moduleName].ContainsKey(imagePath))
                this.spriteBuffer[moduleName].Add(imagePath, image);
        }
    }

    public void LoadTexture(ModuleViews moduleName, string imagePath, System.Action<Texture2D> success = null, System.Action<string> failure = null)
    {
        this.StopClearBuffer(moduleName);
        if (!this.HasImage(moduleName, imagePath))
        {
            this.StartCoroutine(this.ResourceUtils.LoadTexture(imagePath,
                                           (texture) =>
                                           {
                                               this.RegisterTexture(moduleName, imagePath, texture);
                                               if (success != null) success(texture);
                                           },
                                           (failureInfo) => Debug.LogErrorFormat("<><SpriteHelper.LoadTexture>Unknown error: {0}", failureInfo.Message)));
        }
        else if (success != null) success(this.LoadTextureFromBuffer(imagePath));
    }
    public Texture2D LoadTextureFromBuffer(string imagePath)
    {
        Texture2D texture = null;
        foreach (var kvp in this.textureBuffer)
        {
            texture = this.LoadTextureFromBuffer(kvp.Key, imagePath);
            if (texture != null) return texture;
        }
        return null;
    }
    public Texture2D LoadTextureFromBuffer(ModuleViews moduleName, string imagePath, string extension = ".png")
    {
        if (!string.IsNullOrEmpty(imagePath) && !imagePath.ToLower().EndsWith(extension))
            imagePath += extension;

        if (this.textureBuffer != null && this.textureBuffer.ContainsKey(moduleName) &&
            this.textureBuffer[moduleName].ContainsKey(imagePath))
            return this.textureBuffer[moduleName][imagePath];
        else
            return null;
    }
    public void RegisterTexture(ModuleViews moduleName, string imagePath, Texture2D image)
    {
        //Debug.LogFormat("<><SpriteHelper.RegisterTexture>ModuleName: {0}, ImagePath: {1}", moduleName, imagePath);
        if (this.textureBuffer != null)
        {
            if (!this.textureBuffer.ContainsKey(moduleName))
                this.textureBuffer.Add(moduleName, new Dictionary<string, Texture2D>());

            if (!this.textureBuffer[moduleName].ContainsKey(imagePath))
                this.textureBuffer[moduleName].Add(imagePath, image);
        }
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
        if (this.spriteBuffer != null && this.spriteBuffer.ContainsKey(moduleName))
        {
            foreach (var moduleFile in this.spriteBuffer[moduleName])
            {
                GameObject.Destroy(moduleFile.Value);
            }
            this.spriteBuffer.Remove(moduleName);
            Resources.UnloadUnusedAssets();
        }
        if (this.textureBuffer != null && this.textureBuffer.ContainsKey(moduleName))
        {
            foreach (var moduleFile in this.textureBuffer[moduleName])
            {
                GameObject.Destroy(moduleFile.Value);
            }
            this.textureBuffer.Remove(moduleName);
            Resources.UnloadUnusedAssets();
        }
    }
}