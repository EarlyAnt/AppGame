using AppGame.Util;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片加载框
/// </summary>
public abstract class ImageLoader : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/
    [SerializeField]
    protected ModuleViews moduleView;
    [SerializeField]
    protected string imageName;
    [SerializeField]
    protected bool autoLoad;
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
    public bool AutoLoad
    {
        get { return this.autoLoad; }
    }
    /************************************************Unity方法与事件***********************************************/

    /************************************************自 定 义 方 法************************************************/
    //加载图片
    public abstract void LoadImage();
    //加载图片
    public abstract void LoadImage(string imagePath, IResourceUtils resourceUtils = null);
}