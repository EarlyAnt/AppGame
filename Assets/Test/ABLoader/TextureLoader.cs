using UnityEngine;
using UnityEngine.UI;

namespace ABLoader
{
    public class TextureLoader : BaseLoader
    {
        [SerializeField]
        private string spriteName;
        [SerializeField]
        private RawImage imageBox;

        public override void LoadAB()
        {
            this.assetBundle = this.GetAssetBundle(this.assetBundleName);
            Object textureObject = this.assetBundle.LoadAsset(this.spriteName);
            Texture2D texture = textureObject as Texture2D;
            this.imageBox.texture = texture;
            Debug.LogFormat("<><TextureLoader.LoadAB>assetbundle: {0}, sprite: {1}", this.assetBundleName, this.spriteName);
            base.LoadAB();
        }

        [ContextMenu("0-设置Image组件")]
        private void CollectImageBox()
        {
            this.imageBox = this.GetComponent<RawImage>();
        }
    }
}
