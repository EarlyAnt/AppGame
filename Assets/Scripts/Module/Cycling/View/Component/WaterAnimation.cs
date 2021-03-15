using AppGame.Data.Local;
using AppGame.UI;
using AppGame.Util;
using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    /// <summary>
    /// 浇水动画
    /// </summary>
    public class WaterAnimation : BaseView
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
        [SerializeField]
        protected AudioSource audioPlayer;
        [SerializeField]
        private Transform landTransform;
        [SerializeField]
        private Balloon balloonPrefab;
        [SerializeField]
        private Image arrowPrefab;
        [SerializeField]
        private Image mask;
        [SerializeField]
        private GameObject guideSpine;
        [SerializeField]
        private AnimationCurve curve;
        #endregion
        #region 其他变量  
        private List<TreasureBoxInfo> treasureBoxList = new List<TreasureBoxInfo>();
        private List<Balloon> balloonList = new List<Balloon>();
        private Color transparentColor = new Color(1, 1, 1, 0);
        private Vector3 waterPotOffset = new Vector3(30f, 50f, 0f);
        private string treasureBoxAB = "garden/treasurebox";
        private OpenTreasureBoxSteps currentStep;
        private PageCounter pageCounter = null;
        private System.Action callback = null;
        private bool manual = false;
        private List<float> boxOffset = new List<float>() { 0f, -70f, 70f };
        private List<RewardTypes> rewardTypes = null;
        public bool IsPlaying { get; set; }
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
            if (this.audioPlayer == null)
                this.audioPlayer = this.gameObject.AddComponent<AudioSource>();

            this.pageCounter = new PageCounter();
            this.curve = new AnimationCurve();
            this.curve.AddKey(0f, 2f);
            this.curve.AddKey(0.2f, 1f);
            this.curve.AddKey(0.3f, 0f);
            this.curve.AddKey(0.5f, 0f);
            this.curve.AddKey(0.6f, 0f);
            this.curve.AddKey(0.8f, 1f);
            this.curve.AddKey(1f, 2f);
        }
        protected override void Start()
        {
            this.LoadTreasureBox();
        }
        private void Update()
        {
        }
        /************************************************自 定 义 方 法************************************************/
        //显示动画
        public void Play(System.Action callback)
        {
            this.manual = false;
            this.callback = callback;
            this.pageCounter.Reset(3, 3);
            this.currentStep = OpenTreasureBoxSteps.Empty;
            this.treasureBoxList.ForEach(t => { t.Spine.DOFade(0f, 0f); t.Arrow.enabled = false; });
            this.balloonList.ForEach(t => t.Reset());
            this.rewardTypes = new List<RewardTypes>() { RewardTypes.Low, RewardTypes.Medium, RewardTypes.High };
            this.ChangeBox(false);
            this.TreasureBoxDropDown(0);
        }
        //回收水壶资源
        public void DestroyAssetBundle()
        {
            if (this.treasureBoxList != null && this.treasureBoxList.Count > 0)
                this.AssetBundleUtil.UnloadAsset(this.treasureBoxAB);
        }
        //点击操作
        public void Click()
        {
            //switch (this.currentStep)
            //{
            //    case WaterAnimationSteps.DropDown:
            //        this.manual = true;
            //        SoundPlayer.GetInstance().PlaySoundInChannal("garden_open_chest", this.audioPlayer, 0.2f);
            //        this.OpenBox(this.pageCounter.ItemIndex);
            //        break;
            //    case WaterAnimationSteps.OpenedBox:
            //        this.GetReward();
            //        break;
            //}
        }
        //选择宝箱
        public void SelectTreasureBox(int index)
        {
            this.manual = true;
            this.guideSpine.SetActive(false);
            this.pageCounter.Locate(index);
            this.ChangeBox();
        }
        //加载宝箱
        private void LoadTreasureBox()
        {
            this.AssetBundleUtil.LoadAssetBundleAsync(this.treasureBoxAB, (assetBundle) =>
            {
                Object spineObject = assetBundle.LoadAsset("TreasureBox_Prefab");
                Object materialObject = assetBundle.LoadAsset(SpineParameters.MATERIAL_NAME);
                Material material = materialObject as Material;
                Shader shader = Shader.Find(material.shader.name);
                material.shader = shader;
                SkeletonGraphic prefabSpine = (spineObject as GameObject).GetComponent<SkeletonGraphic>();
                prefabSpine.material = material;

                for (int i = 0; i < 3; i++)
                {
                    GameObject treasureBoxObject = GameObject.Instantiate(spineObject) as GameObject;
                    treasureBoxObject.name = "TreasureBox";
                    treasureBoxObject.transform.SetParent(this.transform);
                    treasureBoxObject.transform.localPosition = Vector3.zero + new Vector3(this.boxOffset[i], 0f, 0f);
                    treasureBoxObject.transform.localRotation = Quaternion.identity;
                    treasureBoxObject.transform.localScale = Vector3.one * 0.25f;
                    SkeletonGraphic treasureBox = treasureBoxObject.GetComponent<SkeletonGraphic>();
                    treasureBox.color = this.transparentColor;

                    Balloon balloon = GameObject.Instantiate<Balloon>(this.balloonPrefab);
                    balloon.transform.SetParent(treasureBoxObject.transform);
                    balloon.transform.localPosition = new Vector3(13f, 0f, 0f);
                    balloon.transform.localRotation = Quaternion.identity;
                    balloon.transform.localScale = Vector3.one * 4f;
                    this.balloonList.Add(balloon);

                    Image arrow = GameObject.Instantiate(this.arrowPrefab, treasureBox.transform);
                    arrow.transform.localPosition = new Vector3(0f, 294f, 0f);
                    arrow.transform.localRotation = Quaternion.identity;
                    arrow.transform.localScale = Vector3.one;
                    arrow.enabled = false;
                    this.treasureBoxList.Add(new TreasureBoxInfo() { Spine = treasureBox, Arrow = arrow });
                }
                this.ChangeBox(false);
            },
            (errorText) =>
            {
                Debug.LogErrorFormat("<><WaterAnimation.LoadTreasureBox>Error: {0}", errorText);
            });
        }
        //掉落宝箱
        private void TreasureBoxDropDown(int index)
        {
            if (index >= 0 && index < this.treasureBoxList.Count)
            {
                this.mask.DOFade(0.8f, 0.5f);
                this.treasureBoxList[index].Spine.DOFade(1f, 0.5f);
                this.treasureBoxList[index].Spine.AnimationState.ClearTracks();
                this.treasureBoxList[index].Spine.AnimationState.SetAnimation(0, "box01", false).Complete += (trackEntry) =>
                {
                    index += 1;
                    if (index < this.treasureBoxList.Count)
                    {
                        this.TreasureBoxDropDown(index);
                    }
                    else
                    {
                        this.currentStep = OpenTreasureBoxSteps.DropDown;                        
                        this.treasureBoxList.ForEach(t => t.Arrow.enabled = true);
                        this.ChangeBox();
                        this.DelayInvoke(() =>
                        {
                            if (!this.manual)
                                this.guideSpine.SetActive(true);
                            this.DelayInvoke(() =>
                            {
                                if (!this.manual)
                                {
                                    this.guideSpine.SetActive(false);
                                    this.StopAllCoroutines();
                                    this.StartCoroutine(this.AutoChangeBox());
                                }
                            }, 5f);
                        }, 5f);
                    }
                };
            }
        }
        //自动切换宝箱
        private IEnumerator AutoChangeBox()
        {
            float time = 0;
            bool forward = Random.Range(0, 10) % 2 == 1;
            while (time <= 3)
            {
                if (forward)
                    this.pageCounter.PreItem();
                else
                    this.pageCounter.NextItem();
                this.ChangeBox();
                yield return new WaitForSeconds(this.curve.Evaluate(time / 3f) / 10f);
                time += Time.deltaTime;
            }
            //SoundPlayer.GetInstance().PlaySoundInChannal("garden_open_chest", this.audioPlayer, 0.2f);
            this.OpenBox(this.pageCounter.ItemIndex);
        }
        //切换宝箱
        private void ChangeBox(bool show = true)
        {
            Color whiteTransparentColor = new Color(1f, 1f, 1f, 0f);
            Color grayTransparentColor = new Color(0.5f, 0.5f, 0.5f, 0f);

            for (int i = 0; i < this.treasureBoxList.Count; i++)
            {
                this.treasureBoxList[i].Spine.transform.DOScale(Vector3.one * 0.15f * (i == this.pageCounter.ItemIndex ? 1.5f : 1f), show ? 0f : 0.5f);
                if (show)
                    this.treasureBoxList[i].Spine.DOColor(i == this.pageCounter.ItemIndex ? Color.white : Color.gray, 0.5f);
                else
                    this.treasureBoxList[i].Spine.DOColor(i == this.pageCounter.ItemIndex ? whiteTransparentColor : grayTransparentColor, 0f);

                this.treasureBoxList[i].Arrow.DOFade(i == this.pageCounter.ItemIndex && show ? 1f : 0f, i == this.pageCounter.ItemIndex && show ? 0.2f : 0f);
            }
            this.treasureBoxList[this.pageCounter.ItemIndex].Spine.transform.SetSiblingIndex(1);
        }
        //打开宝箱
        private void OpenBox(int index, bool recursion = true)
        {
            this.currentStep = OpenTreasureBoxSteps.OpeningBox;
            this.treasureBoxList.ForEach(t => t.Arrow.enabled = false);
            if (this.treasureBoxList != null && this.treasureBoxList.Count > 0 &&
                index >= 0 && index < this.treasureBoxList.Count)
            {
                this.treasureBoxList[index].Spine.AnimationState.ClearTracks();
                this.treasureBoxList[index].Spine.AnimationState.SetAnimation(0, "box02", false).Complete += (trackEntry) =>
                {//打开当前选中的宝箱
                    TreasureBoxReward reward = this.CalculateReward();
                    this.balloonList[index].SetValue(reward.Exp, reward.Coin);
                    this.balloonList[index].Appear(index == this.pageCounter.ItemIndex);
                    if (recursion)
                    {
                        this.DelayInvoke(() =>
                        {
                            for (int i = 0; i < this.treasureBoxList.Count; i++)
                            {//打开其他未选中的宝箱
                                if (i != index) this.OpenBox(i, false);
                            }
                        }, index == 0 ? 1f : 0f);
                    }
                    else if (this.currentStep == OpenTreasureBoxSteps.OpeningBox)
                    {
                        this.currentStep = OpenTreasureBoxSteps.OpenedBox;
                    }
                };
            }
        }
        //获得奖励
        private void GetReward()
        {
            this.currentStep = OpenTreasureBoxSteps.GettingReward;
            if (this.balloonList != null && this.balloonList.Count > 0 &&
                this.pageCounter.ItemIndex >= 0 && this.pageCounter.ItemIndex < this.balloonList.Count)
            {
                //SoundPlayer.GetInstance().PlaySoundInChannal("garden_reward", this.audioPlayer, 0.2f);
                this.balloonList[this.pageCounter.ItemIndex].Raise(() =>
                {
                    this.ItemDataManager.AddItem(Items.COIN, this.balloonList[this.pageCounter.ItemIndex].Coin);
                    this.balloonList.ForEach(t => t.Disappear());
                    this.DelayInvoke(() =>
                    {
                        this.treasureBoxList.ForEach(t => t.Spine.DOFade(0f, 0f));
                        this.mask.DOFade(0f, 0f);
                        this.currentStep = OpenTreasureBoxSteps.GotReward;
                        this.IsPlaying = false;
                        this.OnCallback();
                    }, 0.5f);
                });
            }
        }
        //获取随机奖励方案
        private TreasureBoxReward CalculateReward()
        {
            RewardTypes rewardType = RewardTypes.Low;
            if (this.rewardTypes.Count == 3)
            {
                int solution = UnityEngine.Random.Range(0, 1000) % 101;
                if (solution <= 5)
                {//5%的高产出几率：经验9，金币99
                    rewardType = RewardTypes.High;
                }
                else if (solution > 5 && solution <= 30)
                {//25%的中产出几率：经验5，金币20～50之间随机
                    rewardType = RewardTypes.Medium;
                }
                else if (solution > 30)
                {//70%的低产出几率：经验2，金币10～20之间随机
                    rewardType = RewardTypes.Low;
                }
            }
            else if (this.rewardTypes.Count == 2)
            {
                int solution = UnityEngine.Random.Range(0, 10) % 2;
                rewardType = this.rewardTypes[solution];
            }
            else if (this.rewardTypes.Count == 1)
            {
                rewardType = this.rewardTypes[0];
            }
            this.rewardTypes.Remove(rewardType);
            return this.GetReward(rewardType);
        }
        //获取随机经验和金币奖励
        private TreasureBoxReward GetReward(RewardTypes rewardType)
        {
            int exp = 0;
            int coin = 0;
            switch (rewardType)
            {
                case RewardTypes.High://5%的高产出几率：经验9，金币99
                    exp = 9;
                    coin = 99;
                    break;
                case RewardTypes.Medium://25%的中产出几率：经验5，金币20～50之间随机
                    exp = 5;
                    coin = 20 + UnityEngine.Random.Range(0, 100) % 31;
                    break;
                case RewardTypes.Low://70%的低产出几率：经验2，金币10～20之间随机
                    exp = 2;
                    coin = 10 + UnityEngine.Random.Range(0, 100) % 11;
                    break;
            }
            return new TreasureBoxReward() { Exp = exp, Coin = coin };
        }
        //当执行回调时
        private void OnCallback()
        {
            if (this.callback != null)
                this.callback();
        }
    }
}

