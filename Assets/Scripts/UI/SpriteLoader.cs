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
    private Image imageBox;
    /************************************************Unity方法与事件***********************************************/
    private void Start()
    {
        if (this.autoLoad && !string.IsNullOrEmpty(this.imageName) && this.moduleView != ModuleViews.None)
            this.LoadImage();
    }
    /************************************************自 定 义 方 法************************************************/
    //加载图片
    public override void LoadImage()
    {
        if (string.IsNullOrEmpty(this.imageName))
        {
            Debug.LogErrorFormat("<><SpriteLoader.LoadImage>Parameter 'imageName' is null or empty, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }
        else if (this.moduleView == ModuleViews.None)
        {
            Debug.LogErrorFormat("<><SpriteLoader.LoadImage>Component 'baseView' is null, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }

        this.LoadImage(this.imagePath);
    }
    //加载图片
    public override void LoadImage(string imagePath, IResourceUtils resourceUtils = null)
    {
        //检查并设置组件
        if (this.imageBox == null)
            this.imageBox = this.GetComponent<Image>();

        //再次检查组件
        if (this.imageBox == null)
        {
            Debug.LogErrorFormat("<><SpriteLoader.LoadImage2>Component 'image' is null");
            return;
        }

        //加载图片
        if (this.moduleView != ModuleViews.None)
        {
            //Debug.LogFormat("<><SpriteLoader.SetImage>Object: {0}, Image: {1}", this.gameObject.name, imagePath);
            SpriteHelper.Instance.LoadSprite(this.moduleView, imagePath,
                                              (sprite) =>
                                              {
                                                  this.imageBox.sprite = sprite;
                                                  if (this.autoLoad)
                                                      GameObject.Destroy(this);
                                              },
                                              (failureInfo) =>
                                              {
                                                  Debug.LogErrorFormat("<><SpriteLoader.SetImage>Unknown error: {0}", failureInfo);
                                              });
        }
        else Debug.LogErrorFormat("<><SpriteLoader.LoadImage2>Component 'baseView' is null");
    }
    [ContextMenu("设置Image组件")]
    private void SetImageBox()
    {
        if (this.imageBox == null)
            this.imageBox = this.GetComponent<Image>();
    }
    [ContextMenu("重新设置Image组件")]
    private void ResetImageBox()
    {
        this.imageBox = this.GetComponent<Image>();
    }
}