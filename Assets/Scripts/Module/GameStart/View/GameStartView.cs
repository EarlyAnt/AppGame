using AppGame.Config;
using AppGame.Global;
using AppGame.UI;
using AppGame.Util;
using DG.Tweening;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
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
        [Inject]
        public IHotUpdateUtil HotUpdateUtil { get; set; }
        [Inject]
        public IResourceUtil ResourceUtil { get; set; }
        [Inject]
        public ICommonResourceUtil CommonResourceUtil { get; set; }
        [Inject]
        public IAssetBundleUtil AssetBundleUtil { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private ProgressBar totalProgress;
        [SerializeField]
        private ProgressBar downloadProgress;
        [SerializeField, Range(3f, 30f)]
        private float durationMin = 3f;
        [SerializeField, Range(3f, 30f)]
        private float durationMax = 5f;
        [SerializeField, Range(0f, 1f)]
        private float speedRate = 1f;
        #endregion
        #region 其他变量
        private bool downloadComplete = false;
        private Tweener tweener;
        private Queue<System.Action> asyncActions = new Queue<System.Action>();
        private DownloadInfo downloadInfo = new DownloadInfo();//下载信息
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
        private void Update()
        {
            try
            {
                if (this.asyncActions.Count > 0)
                {
                    System.Action action = this.asyncActions.Dequeue();
                    if (action != null) action();
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogErrorFormat("<><GameStartView.Update>do async action error: {0}", ex.Message);
            }
        }
        /************************************************自 定 义 方 法************************************************/
        //初始化
        private IEnumerator Initialize()
        {
#if UNITY_IOS
                iOSNativeAPI.Instance.Initialize();
#endif
            float progress = UnityEngine.Random.Range(0.2f, 0.5f);
            yield return this.StartCoroutine(this.ReadConfig(progress));
            yield return this.StartCoroutine(this.LoadScene(progress, 1f));
        }
        //读取配置
        private IEnumerator ReadConfig(float endValue)
        {
            float startValue = 0f;
            float stepValue = endValue / 9f;

            //read font config
            this.FontConfig.Load();
            yield return new WaitUntil(() => this.FontConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            //read language config
            this.LanConfig.Load();
            yield return new WaitUntil(() => this.LanConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f));

            //read i18n config
            this.I18NConfig.Load();
            yield return new WaitUntil(() => this.I18NConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            //read audio config
            this.AudioConfig.Load();
            yield return new WaitUntil(() => this.AudioConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            //read module config
            this.ModuleConfig.Load();
            yield return new WaitUntil(() => this.ModuleConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            //read item config
            this.ItemConfig.Load();
            yield return new WaitUntil(() => this.ItemConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            //read map config
            this.MapConfig.Load();
            yield return new WaitUntil(() => this.MapConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            //read scenic config
            this.ScenicConfig.Load();
            yield return new WaitUntil(() => this.ScenicConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            //read card config
            this.CardConfig.Load();
            yield return new WaitUntil(() => this.CardConfig.IsLoaded());
            this.totalProgress.Value = startValue += stepValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            //load common images
            yield return this.StartCoroutine(this.Download());
            this.CommonImageUtils.Initialize();
            yield return new WaitForSeconds(1f);
            this.CommonImageUtils.LoadCommonImages();
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f) * this.speedRate);

            #region 测试代码
            //bool complete = false;
            //this.tweener = this.progressBar.DOFillAmount(endValue, UnityEngine.Random.Range(this.durationMin, this.durationMax));
            //this.tweener.onUpdate = () => { this.progressValue.text = string.Format("{0:f1}%", this.progressBar.fillAmount * 100); };
            //this.tweener.onComplete = () => { complete = true; };
            //yield return new WaitUntil(() => complete == true);
            //yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 1.5f));
            #endregion
            Debug.Log("<><GameStartView.ReadConfig>Read config complete...");
        }
        //下载资源文件
        private IEnumerator Download()
        {
            List<AssetFile> assetFiles = this.CommonResourceUtil.GetUpdateFileList();
            if (assetFiles == null || assetFiles.Count == 0)
            {
                Debug.Log("<><GameStartView.Download>No one file need to download");
                this.downloadComplete = true;
                yield break;
            }

            UpdateRecord updateRecord = this.HotUpdateUtil.ReadUpdateRecord();
            if (assetFiles == null || assetFiles.Count == 0)
            {
                Debug.Log("<><GameStartView.Download>Native update record is null");
                this.downloadComplete = true;
                yield break;
            }

            this.downloadInfo.CompleteCount = 0;
            this.downloadInfo.TotalCount = assetFiles.Count;
            bool error = false;
            this.InvokeRepeating("CheckOverTime", 1f, 1f);
            for (int i = 0; i < assetFiles.Count; i++)
            {//逐个下载缺失的资源文件
                Debug.LogFormat("<><GameStartView.Download>File: {0}", assetFiles[i]);
                yield return this.StartCoroutine(this.ResourceUtil.LoadAsset(assetFiles[i].FullPath, (obj) =>
                {
                    LocalFileInfo localFileInfo = updateRecord.FileList.Find(t => t.File + t.MD5 == assetFiles[i].FullPath);
                    if (localFileInfo != null)
                    {
                        localFileInfo.File = assetFiles[i].FullPath;
                        localFileInfo.MD5 = assetFiles[i].MD5;
                        localFileInfo.Type = assetFiles[i].GetFileType();
                    }
                    else
                    {
                        updateRecord.FileList.Add(new LocalFileInfo()
                        {
                            File = assetFiles[i].FullPath,
                            MD5 = assetFiles[i].MD5,
                            Type = assetFiles[i].GetFileType()
                        });
                    }
                },
                (failureInfo) =>
                {
                    if (failureInfo != null)//中断操作类错误才做处理
                    {
                        Debug.LogErrorFormat("<><GameStartView.Download>Error: {0}\n({1})", failureInfo.Message, assetFiles[i]);
                        if (failureInfo.Interrupt) error = true;
                    }
                }, true));
                if (error) break;//如果下载过程中遇到错误，退出下载页

                //更新进度条及进度文字
                float percent = (float)++this.downloadInfo.CompleteCount / this.downloadInfo.TotalCount;
                percent = Mathf.Clamp01(percent);
                this.downloadProgress.Value = percent;
            }
            System.GC.Collect();
            this.CancelInvoke("CheckOverTime");//结束下载时(无论是正常结束还是遇到错误时异常结束)停止检测下载超时
            AssetBundle.UnloadAllAssetBundles(true);
            this.downloadComplete = true;
            this.ResourceUtil.UnloadAllAssetBundles();
            yield return this.AssetBundleUtil.LoadManifest();
        }
        //加载场景
        private IEnumerator LoadScene(float startValue, float endValue)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync("TripMap", LoadSceneMode.Single);
            async.allowSceneActivation = false;
            while (async.progress < 0.9f)
            {
                this.totalProgress.Value = startValue + async.progress * (endValue - startValue);
                yield return new WaitForEndOfFrame();
            }
            Debug.Log("<><GameStartView.ReadConfig>Load scene async complete...");

            this.totalProgress.Value = endValue;
            yield return new WaitForSeconds(UnityEngine.Random.Range(1f, 2f) * this.speedRate);
            async.allowSceneActivation = true;
        }
        //定时检测是否超时
        private void CheckOverTime()
        {
            if (this.downloadInfo.OverTime)
            {
                AssetBundle.UnloadAllAssetBundles(true);
                System.GC.Collect();
                Debug.LogFormat("<><GameStartView.CheckOverTime>TotalCount: {0}, CompleteCount: {1}, Duration: {2}, LastTime: {3}",
                                this.downloadInfo.CompleteCount, this.downloadInfo.TotalCount, this.downloadInfo.Duration, this.downloadInfo.LastTime);
            }
        }
    }

    #region 自定义类
    [Serializable]
    public class ProgressBar
    {
        [SerializeField]
        private Image progressBar;
        [SerializeField]
        private Text progressText;
        private float progressValue;
        public float Value
        {
            get
            {
                return this.progressValue;
            }
            set
            {
                this.progressValue = Mathf.Clamp01(value);
                this.progressBar.fillAmount = this.progressValue;
                this.progressText.text = string.Format("{0:f1}%", this.progressValue * 100);
            }
        }
    }

    public class DownloadInfo
    {
        public DateTime LastTime { get; private set; }
        private bool updateOnce = false;
        private int completeCount = 0;
        public int CompleteCount
        {
            get
            {
                return this.completeCount;
            }
            set
            {
                this.completeCount = value;
                this.updateOnce = true;
                this.LastTime = DateTime.Now;
            }
        }
        public int TotalCount { get; set; }
        public int Duration { get { return (int)(DateTime.Now - this.LastTime).TotalSeconds; } }
        public bool OverTime
        {
            get
            {
                if (this.updateOnce)
                {//如果已经更新过一次下载完成数，才检测是否超时
                    return this.Duration > 60;//下载完成数的数值超过60秒没有改变，则视为超时(比如网络非常差的时候)
                }
                else return false;//否则，不视为超时
            }
        }
    }
    #endregion
}
