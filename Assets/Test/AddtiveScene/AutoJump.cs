using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Test.AddtiveScene
{
    public class AutoJump : MonoBehaviour
    {
        [SerializeField]
        private Vector3 end;
        [SerializeField]
        private float duration;
        private Vector3 start;
        private Tweener tweener;

        private void Start()
        {
            this.start = this.transform.localPosition;
            this.PlayAnimation(false);
        }

        private void OnEnable()
        {
            //DOTween.Restart(this.gameObject);
            if (this.tweener != null)
                this.tweener.Play();
        }

        private void OnDisable()
        {
            //DOTween.Pause(this.gameObject);
            if (this.tweener != null)
                this.tweener.Pause();
        }

        private void PlayAnimation(bool start)
        {
            this.tweener = this.transform.DOLocalMove(start ? this.start : this.end, this.duration);
            this.tweener.onComplete = () => { this.PlayAnimation(!start); };
        }
    }
}
