using AppGame.Util;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片加载框
/// </summary>
[RequireComponent(typeof(Image))]
public class SpriteLoader : ImageLoader
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    protected ModuleViews moduleView;
    [SerializeField]
    private Image imageBox;
    protected string imagePath
    {
        get
        {
            if (this.moduleView == ModuleViews.None)
            {
                Debug.LogErrorFormat("<><ImageLoader.imagePath>moduleView need to set up, object name: {0}", this.gameObject.name);
                return "";
            }
            return "Texture/" + SpriteHelper.Instance.GetAssetPath(this.moduleView, imageName, ".png");
        }
    }
    /************************************************Unity方法与事件***********************************************/
    protected override void Start()
    {
        base.Start();
        if (this.autoLoad && !string.IsNullOrEmpty(this.imageName) && this.moduleView != ModuleViews.None)
            this.LoadImage();
    }
    /************************************************自 定 义 方 法************************************************/
    //加载图片
    public override void LoadImage()
    {
        if (string.IsNullOrEmpty(this.imageName))
        {
            if (this.autoLoad)
                Debug.LogErrorFormat("<><SpriteLoader.LoadImage>Parameter 'imageName' is null or empty, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }
        else if (this.moduleView == ModuleViews.None)
        {
            if (this.autoLoad)
                Debug.LogErrorFormat("<><SpriteLoader.LoadImage>Component 'baseView' is null, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }

        //检查并设置组件
        if (this.imageBox == null)
            this.imageBox = this.GetComponent<Image>();

        //再次检查组件
        if (this.imageBox == null)
        {
            Debug.LogErrorFormat("<><SpriteLoader.LoadImage>Component 'image' is null");
            return;
        }

        //加载图片
        if (this.moduleView != ModuleViews.None)
        {
            //Debug.LogFormat("<><SpriteLoader.LoadImage>Object: {0}, Image: {1}", this.gameObject.name, this.imagePath);
            SpriteHelper.Instance.LoadSprite(this.moduleView, this.imagePath,
                                              (sprite) =>
                                              {
                                                  this.imageBox.sprite = sprite;
                                                  //if (this.autoLoad)
                                                  //    GameObject.Destroy(this);
                                              },
                                              (failureInfo) =>
                                              {
                                                  Debug.LogErrorFormat("<><SpriteLoader.SetImage>Unknown error: {0}", failureInfo);
                                              });
        }
        else Debug.LogErrorFormat("<><SpriteLoader.LoadImage>Component 'baseView' is null");
    }
    //加载图片
    public void LoadImage(string newImageName)
    {
        this.imageName = newImageName;
        this.LoadImage();
    }
    //设置图片所属模块
    public void SetModuleView(ModuleViews moduleView)
    {
        this.moduleView = moduleView;
    }
    [ContextMenu("0-设置Image组件和图片名字")]
    public void SetAll()
    {
        this.SetImageBox();
        this.SetImageName();
    }
    [ContextMenu("1-设置Image组件")]
    public void SetImageBox()
    {
        this.imageBox = this.GetComponent<Image>();
    }
    [ContextMenu("2-设置图片名字")]
    public void SetImageName()
    {
        if (this.imageBox != null && this.imageBox.sprite != null)
            this.imageName = this.imageBox.sprite.name;
        else
            Debug.LogError("<><SpriteLoader.SetImageName>Error: imageBox is null or sprite is null");
    }
}