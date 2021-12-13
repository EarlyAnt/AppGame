using AppGame.Util;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 替换脚本
/// </summary>
public class SpriteLoaderToABSpriteLoader : MonoBehaviour
{
    /************************************************属性与变量命名************************************************/

    /************************************************Unity方法与事件***********************************************/
    private void Start()
    {
    }
    /************************************************自 定 义 方 法************************************************/
    [ContextMenu("添加TextureLoader脚本")]
    private void AddTextureLoader()
    {
        List<SpriteLoader> spriteLoaderList = this.FindSpriteLoaders(this.transform);
        for (int i = 0; i < spriteLoaderList.Count; i++)
        {
            ABSpriteLoader abspriteLoader = spriteLoaderList[i].GetComponent<ABSpriteLoader>();
            if (abspriteLoader == null)
            {
                abspriteLoader = spriteLoaderList[i].gameObject.AddComponent<ABSpriteLoader>();
                GameObject.DestroyImmediate(spriteLoaderList[i]);
            }

            abspriteLoader.SetAssetBundleName("cycling/view");
            abspriteLoader.SetImageName(spriteLoaderList[i].ImageName);
            abspriteLoader.SetImageBox();
        }
    }
    //递归搜集SpriteLoader组件
    private List<SpriteLoader> FindSpriteLoaders(Transform node)
    {
        List<SpriteLoader> spriteLoaders = new List<SpriteLoader>();

        SpriteLoader spriteLoader = node.GetComponent<SpriteLoader>();
        if (spriteLoader != null)
            spriteLoaders.Add(spriteLoader);

        if (node.childCount > 0)
        {
            for (int i = 0; i < node.childCount; i++)
            {
                List<SpriteLoader> subNodeSpriteLoaders = this.FindSpriteLoaders(node.GetChild(i));
                if (subNodeSpriteLoaders != null && subNodeSpriteLoaders.Count > 0)
                    spriteLoaders.AddRange(subNodeSpriteLoaders);
            }
        }

        return spriteLoaders;
    }
}