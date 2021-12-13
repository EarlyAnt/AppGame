using AppGame.Util;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 图片加载框
/// </summary>
[RequireComponent(typeof(Image))]
public class ABSpriteLoader : ImageLoader
{
    /************************************************属性与变量命名************************************************/
    [Inject]
    public IAssetBundleUtil AssetBundleUtil { get; set; }
    [SerializeField]
    private string assetbundleName;
    [SerializeField]
    private Image imageBox;
    /************************************************Unity方法与事件***********************************************/
    protected override void Start()
    {
        base.Start();
        if (this.autoLoad && !string.IsNullOrEmpty(this.assetbundleName) && !string.IsNullOrEmpty(this.imageName))
            this.LoadImage();
    }
    /************************************************自 定 义 方 法************************************************/
    //加载图片
    public override void LoadImage()
    {
        if (string.IsNullOrEmpty(this.assetbundleName))
        {
            if (this.autoLoad)
                Debug.LogErrorFormat("<><ABSpriteLoader.LoadImage>Parameter 'assetbundleName' is null or empty, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }
        else if (string.IsNullOrEmpty(this.imageName))
        {
            if (this.autoLoad)
                Debug.LogErrorFormat("<><ABSpriteLoader.LoadImage>Parameter 'imageName' is null or empty, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }

        //检查并设置组件
        if (this.imageBox == null)
            this.imageBox = this.GetComponent<Image>();

        //再次检查组件
        if (this.imageBox == null)
        {
            Debug.LogErrorFormat("<><ABSpriteLoader.LoadImage>Component 'image' is null");
            return;
        }

        //加载图片
        //Debug.LogFormat("<><ABSpriteLoader.LoadImage>Object: {0}, Image: {1}", this.gameObject.name, imagePath);
        this.AssetBundleUtil.LoadAssetBundleAsync(this.assetbundleName, (assetbundle) =>
        {
            Sprite sprite = assetbundle.LoadAsset<Sprite>(this.imageName);
            if (sprite != null)
            {
                this.imageBox.sprite = sprite;
                //if (this.autoLoad)
                //    GameObject.Destroy(this);
            }
            else
            {
                Debug.LogErrorFormat("<><ABSpriteLoader.LoadImage>Error: can not find sprite, assetbundle: {0}, texture: {1}", this.assetbundleName, this.imageName);
            }
        }, (failureInfo) =>
        {
            Debug.LogErrorFormat("<><ABSpriteLoader.LoadImage>Error: can not find assetbundle, assetbundle: {0}", this.assetbundleName);
        });
    }
    //加载图片
    public void LoadImage(string newImageName)
    {
        this.imageName = newImageName;
        this.LoadImage();
    }
    //设置AB包名称
    public void SetAssetBundleName(string name)
    {
        this.assetbundleName = name;
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
            Debug.LogError("<><ABSpriteLoader.SetImageName>Error: imageBox is null or sprite is null");
    }
}