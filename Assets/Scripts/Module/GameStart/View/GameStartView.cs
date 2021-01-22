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
            this.ReadConfig();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /************************************************�� �� �� �� ��************************************************/
        private void ReadConfig()
        {
            this.tweener = this.progressBar.DOFillAmount(1, Random.Range(this.durationMin, this.durationMax));
            this.tweener.onUpdate = () => { this.progressValue.text = string.Format("{0:f1}%", this.progressBar.fillAmount * 100); };
            this.tweener.onComplete = () => { SceneManager.LoadScene("TripMap", LoadSceneMode.Additive); };
        }
    }
}

