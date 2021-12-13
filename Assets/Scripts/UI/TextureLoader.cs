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
    [Inject]
    public IAssetBundleUtil AssetBundleUtil { get; set; }
    [SerializeField]
    private string assetbundleName;
    [SerializeField]
    private RawImage imageBox;
    [SerializeField]
    private bool setNativeSize;
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
                Debug.LogErrorFormat("<><TextureLoader.LoadImage>Parameter 'assetbundleName' is null or empty, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }
        else if (string.IsNullOrEmpty(this.imageName))
        {
            if (this.autoLoad)
                Debug.LogErrorFormat("<><TextureLoader.LoadImage>Parameter 'imageName' is null or empty, object: {0}", this.gameObject != null ? this.gameObject.name : "");
            return;
        }

        //检查并设置组件
        if (this.imageBox == null)
            this.imageBox = this.GetComponent<RawImage>();

        //再次检查组件
        if (this.imageBox == null)
        {
            Debug.LogErrorFormat("<><TextureLoader.LoadImage>Component 'image' is null");
            return;
        }

        //加载图片
        //Debug.LogFormat("<><TextureLoader.LoadImage>Object: {0}, Image: {1}", this.gameObject.name, imagePath);
        this.AssetBundleUtil.LoadAssetBundleAsync(this.assetbundleName, (assetbundle) =>
        {
            Texture2D texture = assetbundle.LoadAsset<Texture2D>(this.imageName);
            if (texture != null)
            {
                this.imageBox.texture = texture;
                if (this.setNativeSize)
                    this.imageBox.SetNativeSize();
                if (this.autoLoad)
                    GameObject.Destroy(this);
            }
            else
            {
                Debug.LogErrorFormat("<><TextureLoader.LoadImage>Error: can not find texture, assetbundle: {0}, texture: {1}", this.assetbundleName, this.imageName);
            }
        }, (failureInfo) =>
        {
            Debug.LogErrorFormat("<><TextureLoader.LoadImage>Error: can not find assetbundle, assetbundle: {0}", this.assetbundleName);
        });
    }
    //加载图片
    public void LoadImage(string assetbundleName, string newImageName)
    {
        this.assetbundleName = assetbundleName;
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
    [ContextMenu("1-设置RawImage组件")]
    private void SetImageBox()
    {
        this.imageBox = this.GetComponent<RawImage>();
    }
    [ContextMenu("2-设置图片名字")]
    public void SetImageName()
    {
        if (this.imageBox != null && this.imageBox.texture != null)
            this.imageName = this.imageBox.texture.name;
        else
            Debug.LogError("<><TextureLoader.SetImageName>Error: imageBox is null or sprite is null");
    }
}