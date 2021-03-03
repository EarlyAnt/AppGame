using AppGame.Config;
using AppGame.Global;
using AppGame.UI;
using DG.Tweening;
using strange.extensions.mediation.impl;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppGame.Module.GameStart
{
    public class GameStartView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public IFontConfig FontConfig { get; set; }
        [Inject]
        public ILanConfig LanConfig { get; set; }
        [Inject]
        public II18NConfig I18NConfig { get; set; }
        [Inject]
        public IAudioConfig AudioConfig { get; set; }
        [Inject]
        public IModuleConfig ModuleConfig { get; set; }
        [Inject]
        public IItemConfig ItemConfig { get; set; }
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [Inject]
        public IScenicConfig ScenicConfig { get; set; }
        [Inject]
        public ICardConfig CardConfig { get; set; }
        [Inject]
        public ICommonImageUtils CommonImageUtils { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private Image progressBar;
        [SerializeField]
        private Text progressValue;
        [SerializeField, Range(3f, 30f)]
        private float durationMin = 3f;
        [SerializeField, Range(3f, 30f)]
        private float durationMax = 5f;
        [SerializeField, Range(0f, 1f)]
        private float speedRate = 1f;
        #endregion
        #region 其他变量
        private Tweener tweener;
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
            this.StartCoroutine(this.Initialize());
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        //初始化
        private IEnumerator Initialize()
        {
            float progress = Random.Range(0.2f, 0.5f);
            yield return this.StartCoroutine(this.ReadConfig(progress));
            yield return this.StartCoroutine(this.LoadScene(progress, 1f));
        }
        //读取配置
        private IEnumerator ReadConfig(float endValue)
        {
            float startValue = 0f;
            float stepValue = endValue / 10f;

            //read font config
            this.FontConfig.Load();
            yield return new WaitUntil(() => this.FontConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            //read language config
            this.LanConfig.Load();
            yield return new WaitUntil(() => this.LanConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));

            //read i18n config
            this.I18NConfig.Load();
            yield return new WaitUntil(() => this.I18NConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            //read audio config
            this.AudioConfig.Load();
            yield return new WaitUntil(() => this.AudioConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            //read module config
            this.ModuleConfig.Load();
            yield return new WaitUntil(() => this.ModuleConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            //read item config
            this.ItemConfig.Load();
            yield return new WaitUntil(() => this.ItemConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            //read map config
            this.MapConfig.Load();
            yield return new WaitUntil(() => this.MapConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            //read scenic config
            this.ScenicConfig.Load();
            yield return new WaitUntil(() => this.ScenicConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            //read card config
            this.CardConfig.Load();
            yield return new WaitUntil(() => this.CardConfig.IsLoaded());
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            //load common images
            this.CommonImageUtils.Initialize();
            yield return new WaitForSeconds(1f);
            this.CommonImageUtils.LoadCommonImages();
            this.SetProgress(startValue += stepValue);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f) * this.speedRate);

            #region 测试代码
            //bool complete = false;
            //this.tweener = this.progressBar.DOFillAmount(endValue, Random.Range(this.durationMin, this.durationMax));
            //this.tweener.onUpdate = () => { this.progressValue.text = string.Format("{0:f1}%", this.progressBar.fillAmount * 100); };
            //this.tweener.onComplete = () => { complete = true; };
            //yield return new WaitUntil(() => complete == true);
            //yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            #endregion
            Debug.Log("<><GameStartView.ReadConfig>Read config complete...");
        }
        //设置进度条
        private void SetProgress(float progressValue)
        {
            this.progressBar.fillAmount = progressValue;
            this.progressValue.text = string.Format("{0:f1}%", this.progressBar.fillAmount * 100);
        }
        //加载场景
        private IEnumerator LoadScene(float startValue, float endValue)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync("TripMap", LoadSceneMode.Single);
            async.allowSceneActivation = false;
            while (async.progress < 0.9f)
            {
                this.progressBar.fillAmount = startValue + async.progress * (endValue - startValue);
                this.progressValue.text = string.Format("{0:f1}%", startValue * 100 + async.progress * (endValue - startValue) * 100);
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("<><GameStartView.ReadConfig>Load scene async complete...");

            this.progressBar.fillAmount = endValue;
            this.progressValue.text = endValue * 100 + "%";
            yield return new WaitForSeconds(Random.Range(1f, 2f) * this.speedRate);
            async.allowSceneActivation = true;
        }
    }
}
