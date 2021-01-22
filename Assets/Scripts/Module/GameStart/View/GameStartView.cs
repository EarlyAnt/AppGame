using DG.Tweening;
using strange.extensions.mediation.impl;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppGame.Module.GameStart
{
    public class GameStartView : EventView
    {
        /************************************************�������������************************************************/
        [SerializeField]
        private Image progressBar;
        [SerializeField]
        private Text progressValue;
        [SerializeField, Range(3f, 30f)]
        private float durationMin = 3f;
        [SerializeField, Range(3f, 30f)]
        private float durationMax = 5f;
        private Tweener tweener;
        /************************************************Unity�������¼�***********************************************/
        protected override void Start()
        {
            base.Start();
            this.StartCoroutine(this.Initialize());
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /************************************************�� �� �� �� ��************************************************/
        //��ʼ��
        private IEnumerator Initialize()
        {
            float progress = Random.Range(0.2f, 0.5f);
            yield return this.StartCoroutine(this.ReadConfig(progress));
            yield return this.StartCoroutine(this.LoadScene(progress, 1f));
        }
        //��ȡ����
        private IEnumerator ReadConfig(float endValue)
        {
            bool complete = false;
            this.tweener = this.progressBar.DOFillAmount(endValue, Random.Range(this.durationMin, this.durationMax));
            this.tweener.onUpdate = () => { this.progressValue.text = string.Format("{0:f1}%", this.progressBar.fillAmount * 100); };
            this.tweener.onComplete = () => { complete = true; };
            yield return new WaitUntil(() => complete == true);
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            Debug.Log("<><GameStartView.ReadConfig>Read config complete...");
        }
        //���س���
        private IEnumerator LoadScene(float startValue, float endValue)
        {
            AsyncOperation async = SceneManager.LoadSceneAsync("TripMap", LoadSceneMode.Additive);
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
            yield return new WaitForSeconds(Random.Range(1f, 2f));
            async.allowSceneActivation = true;
        }
    }
}
