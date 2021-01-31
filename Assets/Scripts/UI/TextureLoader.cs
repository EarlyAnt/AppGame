using AppGame.Util;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片加载框
/// </summary>
[RequireComponent(typeof(RawImage))]
public class TextureLoader : ImageLoader
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    private RawImage imageBox;
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
            Debug.LogErrorFormat("<><TextureLoader.LoadImage>Parameter 'imageName' is null or empty, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }
        else if (this.moduleView == ModuleViews.None)
        {
            Debug.LogErrorFormat("<><TextureLoader.LoadImage>Component 'baseView' is null, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }

        this.LoadImage(this.imagePath);
    }
    //加载图片
    public override void LoadImage(string imagePath, IResourceUtils resourceUtils = null)
    {
        //检查并设置组件
        if (this.imageBox == null)
            this.imageBox = this.GetComponent<RawImage>();

        //再次检查组件
        if (this.imageBox == null)
        {
            Debug.LogErrorFormat("<><TextureLoader.LoadImage2>Component 'image' is null");
            return;
        }

        //加载图片
        if (this.moduleView != ModuleViews.None)
        {
            //Debug.LogFormat("<><TextureLoader.SetImage>Object: {0}, Image: {1}", this.gameObject.name, imagePath);
            SpriteHelper.Instance.LoadTexture(this.moduleView, imagePath,
                                              (texture) =>
                                              {
                                                  this.imageBox.texture = texture;
                                                  if (this.autoLoad)
                                                      GameObject.Destroy(this);
                                              },
                                              (failureInfo) =>
                                              {
                                                  Debug.LogErrorFormat("<><TextureLoader.SetImage>Unknown error: {0}", failureInfo);
                                              });
        }
        else Debug.LogErrorFormat("<><TextureLoader.LoadImage2>Component 'baseView' is null");
    }
    [ContextMenu("设置RawImage组件")]
    private void SetImageBox()
    {
        if (this.imageBox == null)
            this.imageBox = this.GetComponent<RawImage>();
    }
    [ContextMenu("重新设置RawImage组件")]
    private void ResetImageBox()
    {
        this.imageBox = this.GetComponent<RawImage>();
    }
}