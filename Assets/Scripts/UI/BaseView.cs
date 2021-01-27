using DG.Tweening;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.UI
{
    public class BaseView : EventView
    {
        private List<Tweener> tweeners = new List<Tweener>();

        protected override void Start()
        {
            base.Start();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

        protected void DelayInvoke(System.Action action, float delaySeconds)
        {
            float timer = 0;
            Tweener tweener = DOTween.To(() => timer, (x) => timer = x, 1, delaySeconds);
            tweener.onComplete = () =>
            {
                if (this.tweeners.Contains(tweener))
                    this.tweeners.Remove(tweener);
                if (action != null) action();
            };
            if (!this.tweeners.Contains(tweener))
                this.tweeners.Add(tweener);
        }

        protected void CancelAllDelayInvoke()
        {
            if (this.tweeners != null && this.tweeners.Count > 0)
            {
                this.tweeners.ForEach(t => { if (t != null) t.Kill(); });
            }
        }

        protected void WaitUntil(Func<bool> condition, System.Action callback)
        {
            this.StopCoroutine(this.WaitForCondition(condition, callback));
            this.StartCoroutine(this.WaitForCondition(condition, callback));
        }

        private IEnumerator WaitForCondition(Func<bool> condition, System.Action callback)
        {
            yield return new WaitUntil(condition);
            if (callback != null) callback();
        }
    }
}

