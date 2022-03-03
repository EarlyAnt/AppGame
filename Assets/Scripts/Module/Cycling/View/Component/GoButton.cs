using AppGame.Data.Local;
using AppGame.UI;
using AppGame.Util;
using DG.Tweening;
using Spine;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    /// <summary>
    /// Go按钮动画
    /// </summary>
    public class GoButton : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public IAssetBundleUtil AssetBundleUtil { get; set; }
        [Inject]
        public IChildInfoManager ChildInfoManager { get; set; }
        [Inject]
        public IItemDataManager ItemDataManager { get; set; }
        #endregion
        #region 页面UI组件

        #endregion
        #region 其他变量  
        private string goButtonAB = "cycling/gobutton";
        protected AudioSource audioPlayer;
        private SkeletonGraphic goButton;
        private SkeletonGraphic particleEffect;
        public bool IsPlaying { get; set; }
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
            if (this.audioPlayer == null)
                this.audioPlayer = this.gameObject.AddComponent<AudioSource>();
        }
        protected override void Start()
        {
            this.LoadGoButton();
        }
        /************************************************自 定 义 方 法************************************************/
        //回收水壶资源
        public void DestroyAssetBundle()
        {
            if (this.goButton != null)
                this.AssetBundleUtil.UnloadAsset(this.goButtonAB);
        }
        //加载宝箱
        public void LoadGoButton()
        {
            this.AssetBundleUtil.LoadAssetBundleAsync(this.goButtonAB, (assetBundle) =>
            {
                Vector2 buttonSize = new Vector2(584, 232);

                Object spineObject = assetBundle.LoadAsset("GoButton_Prefab");
                Object materialObject = assetBundle.LoadAsset(SpineParameters.MATERIAL_NAME);
                Material material = materialObject as Material;
                Shader shader = Shader.Find(material.shader.name);
                material.shader = shader;
                SkeletonGraphic prefabSpine = (spineObject as GameObject).GetComponent<SkeletonGraphic>();
                prefabSpine.material = material;

                GameObject goButtonObject = GameObject.Instantiate(spineObject) as GameObject;
                goButtonObject.name = "GoButton";
                goButtonObject.transform.SetParent(this.transform);
                goButtonObject.transform.localPosition = Vector3.zero;
                goButtonObject.transform.localRotation = Quaternion.identity;
                goButtonObject.transform.localScale = Vector3.one;
                goButtonObject.GetComponent<RectTransform>().sizeDelta = buttonSize;
                this.goButton = goButtonObject.GetComponent<SkeletonGraphic>();
                CyclingView cyclingView = GameObject.FindObjectOfType<CyclingView>();
                if (cyclingView != null)
                {
                    this.goButton.gameObject.AddComponent<Button>().onClick.AddListener(cyclingView.Go);
                }

                GameObject particleEffectObject = GameObject.Instantiate(spineObject) as GameObject;
                particleEffectObject.name = "ParticleEffect";
                particleEffectObject.transform.SetParent(this.transform);
                particleEffectObject.transform.localPosition = Vector3.zero;
                particleEffectObject.transform.localRotation = Quaternion.identity;
                particleEffectObject.transform.localScale = Vector3.one;
                particleEffectObject.GetComponent<RectTransform>().sizeDelta = buttonSize;
                this.particleEffect = particleEffectObject.GetComponent<SkeletonGraphic>();
                this.particleEffect.raycastTarget = false;
                this.particleEffect.AnimationState.SetAnimation(0, "particle", true);
                this.particleEffect.gameObject.SetActive(false);

                this.dispatcher.Dispatch(GameEvent.GO_BUTTON_LOADED);
            },
            (errorText) =>
            {
                Debug.LogErrorFormat("<><GoButton.LoadGoButton>Error: {0}", errorText);
            });
        }
        //设置水位
        public void SetWaterLevel(int percent, WaterLevelActions waterLevelAction = WaterLevelActions.Unknown)
        {
            if (this.goButton == null)
                return;

            percent = Mathf.Clamp(percent, 0, 200);
            percent = percent / 20;
            string animationName = percent == 0 ? "animation10" : string.Format("animation{0}", percent * 10);
            //Debug.LogFormat("<><GoButton.SetWaterLevel>animation: {0}", animationName);
            this.goButton.AnimationState.SetAnimation(0, animationName, true).Complete += (trackEntry) =>
            {
                //Todo: 播放水位上升或下降的声音
            };

            this.particleEffect.gameObject.SetActive(percent > 0);
        }
        //淡入淡出
        public void DoFade(float endValue, float seconds)
        {
            this.goButton.DOFade(endValue, seconds);
            this.particleEffect.DOFade(endValue, seconds);
        }
    }
}

public enum WaterLevelActions
{
    Unknown,
    Rising,
    Drawdown
}
