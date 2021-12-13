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
    public string ImageName
    {
        get { return this.imageName; }
    }
    /************************************************Unity方法与事件***********************************************/

    /************************************************自 定 义 方 法************************************************/
    //加载图片
    public abstract void LoadImage();
    //设置图片名称
    public void SetImageName(string name)
    {
        this.imageName = name;
    }
}