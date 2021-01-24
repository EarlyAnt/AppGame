using DG.Tweening;
using strange.extensions.mediation.impl;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class CyclingView : EventView
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private MapNode mapNode;
        [SerializeField]
        private Transform player;
        [SerializeField]
        private Camera camera;
        [SerializeField]
        private float step;
        private int nodeIndex;
        private Vector3 destination
        {
            get
            {
                return this.mapNode != null && this.mapNode.Points != null && this.mapNode.Points.Count > 0 ?
                       this.mapNode.Points[this.nodeIndex].position : Vector3.zero;
            }
        }
        /************************************************Unity方法与事件***********************************************/
        protected override void Start()
        {
            base.Start();
            this.Initialize();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        private void Update()
        {
            Vector3 playerPos = this.player.position;
            playerPos.z = this.camera.transform.position.z;
            this.camera.transform.position = playerPos;
        }
        /************************************************自 定 义 方 法************************************************/
        private void Initialize()
        {
            this.player.position = this.mapNode.Points[this.nodeIndex].position;
        }
        public void MoveForward()
        {
            this.StopAllCoroutines();
            this.StartCoroutine(this.MovePlayer());
        }
        private IEnumerator MovePlayer()
        {
            if (this.nodeIndex + 1 >= this.mapNode.Points.Count)
                yield break;

            MapPointNode pointNode = null;
            do
            {
                this.nodeIndex += 1;
                do
                {
                    this.player.position = Vector3.MoveTowards(this.player.position, this.destination, this.step);
                    yield return new WaitForEndOfFrame();
                }
                while (Vector3.Distance(this.player.position, this.destination) > 0.01f);
                pointNode = this.mapNode.Points[this.nodeIndex].GetComponent<MapPointNode>();
            }
            while (pointNode == null);
            Debug.Log("stop");
        }
    }
}
