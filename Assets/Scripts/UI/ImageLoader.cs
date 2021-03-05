using AppGame.UI;
using AppGame.Util;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片加载框
/// </summary>
public abstract class ImageLoader : BaseView
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    protected string imageName;
    [SerializeField]
    protected bool autoLoad = true;
    public bool AutoLoad
    {
        get { return this.autoLoad; }
    }
    /************************************************Unity方法与事件***********************************************/

    /************************************************自 定 义 方 法************************************************/
    //加载图片
    public abstract void LoadImage();
    //加载图片
    public virtual void LoadImage(string newImageName)
    {
        this.imageName = newImageName;
        this.LoadImage();
    }
}