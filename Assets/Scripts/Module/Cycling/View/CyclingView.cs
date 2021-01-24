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
        private Vector3 top;
        private Vector3 bottom;
        private Vector3 left;
        private Vector3 right;

        private Vector3 topLeft;
        private Vector3 topRight;
        private Vector3 bottomLeft;
        private Vector3 bottomRight;
        private float fieldOfViewVertical = 60f;
        private float fieldOfViewHorizontal = 35.98339f;
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
        private void OnDrawGizmos()
        {
            Vector3 localPos = this.mapNode.transform.InverseTransformPoint(this.camera.transform.position);
            float dis = Vector3.Dot(localPos, Vector3.forward);
            Vector3 vectorNormal = Vector3.forward * dis;
            localPos = localPos - vectorNormal;
            Vector3 projection = this.mapNode.transform.TransformPoint(localPos);

            float distance = Vector3.Distance(this.camera.transform.position, projection);
            float frustumHeight = 2.0f * distance * Mathf.Tan(this.fieldOfViewVertical * 0.5f * Mathf.Deg2Rad);
            float frustumWidth = 2.0f * distance * Mathf.Tan(this.fieldOfViewHorizontal * 0.5f * Mathf.Deg2Rad);
            Debug.LogFormat("height: {0}, width: {1}", frustumHeight, frustumWidth);

            this.top = new Vector3(this.camera.transform.position.x, projection.y + frustumHeight / 2, this.mapNode.transform.position.z);
            this.bottom = new Vector3(this.camera.transform.position.x, projection.y - frustumHeight / 2, this.mapNode.transform.position.z);
            this.left = new Vector3(projection.x + frustumWidth / 2, this.camera.transform.position.y, this.mapNode.transform.position.z);
            this.right = new Vector3(projection.x - frustumWidth / 2, this.camera.transform.position.y, this.mapNode.transform.position.z);

            this.topLeft = new Vector3(this.left.x, this.top.y, this.top.z);
            this.topRight = new Vector3(this.right.x, this.top.y, this.top.z);
            this.bottomLeft = new Vector3(this.left.x, this.bottom.y, this.bottom.z);
            this.bottomRight = new Vector3(this.right.x, this.bottom.y, this.bottom.z);

            Gizmos.color = Color.green;
            Gizmos.DrawLine(this.camera.transform.position, projection);
            Gizmos.DrawLine(this.camera.transform.position, this.top);
            Gizmos.DrawLine(this.camera.transform.position, this.bottom);
            Gizmos.DrawLine(this.camera.transform.position, this.left);
            Gizmos.DrawLine(this.camera.transform.position, this.right);

            Gizmos.DrawLine(this.topLeft, this.topRight);
            Gizmos.DrawLine(this.topRight, this.bottomRight);
            Gizmos.DrawLine(this.bottomRight, this.bottomLeft);
            Gizmos.DrawLine(this.bottomLeft, this.topLeft);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(projection, 2f);
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
