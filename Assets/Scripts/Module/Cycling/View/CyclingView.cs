using AppGame.UI;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class CyclingView : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Image mask;
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private MapNode mapNode;
        [SerializeField]
        private Transform player;
        [SerializeField]
        private Camera camera;
        [SerializeField]
        private float step;
        [SerializeField, Range(0f, 5f)]
        private float lerp = 0.5f;
        private int nodeIndex;
        private Vector3 top;
        private Vector3 bottom;
        private Vector3 left;
        private Vector3 right;

        private Vector3 topLeft;
        private Vector3 topRight;
        private Vector3 bottomLeft;
        private Vector3 bottomRight;
        private float frustumHeight;
        private float frustumWidth;
        private float fieldOfViewVertical = 60f;
        private float fieldOfViewHorizontal = 35.98339f;
        private float canvasScale = 0.05f;
        private bool inRange;
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
            if (!this.inRange)
            {
                Image image = this.mapNode.GetComponent<Image>();
                playerPos.x = Mathf.Clamp(playerPos.x, -image.rectTransform.sizeDelta.x * this.canvasScale / 2f + this.frustumWidth / 2f, image.rectTransform.sizeDelta.x * this.canvasScale / 2f - this.frustumWidth / 2f);
                playerPos.y = Mathf.Clamp(playerPos.y, -image.rectTransform.sizeDelta.y * this.canvasScale / 2f + this.frustumHeight / 2f, image.rectTransform.sizeDelta.y * this.canvasScale / 2f - this.frustumHeight / 2f);
            }
            this.camera.transform.DOMove(playerPos, this.lerp);
        }
        private void OnDrawGizmos()
        {
            Vector3 localPos = this.mapNode.transform.InverseTransformPoint(this.camera.transform.position);
            float dis = Vector3.Dot(localPos, Vector3.forward);
            Vector3 vectorNormal = Vector3.forward * dis;
            localPos = localPos - vectorNormal;
            Vector3 projection = this.mapNode.transform.TransformPoint(localPos);

            float distance = Vector3.Distance(this.camera.transform.position, projection);
            this.frustumHeight = 2.0f * distance * Mathf.Tan(this.fieldOfViewVertical * 0.5f * Mathf.Deg2Rad);
            this.frustumWidth = 2.0f * distance * Mathf.Tan(this.fieldOfViewHorizontal * 0.5f * Mathf.Deg2Rad);
            Debug.LogFormat("distance: {0}, height: {1}, width: {2}", distance, frustumHeight, frustumWidth);

            this.top = new Vector3(this.player.transform.position.x, this.player.position.y + frustumHeight / 2, this.mapNode.transform.position.z);
            this.bottom = new Vector3(this.player.transform.position.x, this.player.position.y - frustumHeight / 2, this.mapNode.transform.position.z);
            this.left = new Vector3(this.player.position.x + frustumWidth / 2, this.player.transform.position.y, this.mapNode.transform.position.z);
            this.right = new Vector3(this.player.position.x - frustumWidth / 2, this.player.transform.position.y, this.mapNode.transform.position.z);

            this.topLeft = new Vector3(this.left.x, this.top.y, this.top.z);
            this.topRight = new Vector3(this.right.x, this.top.y, this.top.z);
            this.bottomLeft = new Vector3(this.left.x, this.bottom.y, this.bottom.z);
            this.bottomRight = new Vector3(this.right.x, this.bottom.y, this.bottom.z);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.topLeft, this.topRight);
            Gizmos.DrawLine(this.topRight, this.bottomRight);
            Gizmos.DrawLine(this.bottomRight, this.bottomLeft);
            Gizmos.DrawLine(this.bottomLeft, this.topLeft);

            this.inRange = this.PointInEdge(this.topLeft) && this.PointInEdge(this.topRight) &&
                           this.PointInEdge(this.bottomLeft) && this.PointInEdge(this.bottomRight);

            Gizmos.color = inRange ? Color.cyan : Color.red;
            Gizmos.DrawSphere(projection, 9f);
        }
        /************************************************自 定 义 方 法************************************************/
        private void Initialize()
        {
            this.player.position = this.mapNode.Points[this.nodeIndex].position;
            Vector3 playerPos = this.player.position;
            playerPos.z = this.camera.transform.position.z;
            this.camera.transform.position = playerPos;

            this.DelayInvoke(() =>
            {
                this.mask.DOFade(0f, 1f);
                this.canvasGroup.DOFade(1f, 1f);
            }, 1f);
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
        private bool PointInEdge(Vector3 point)
        {
            Image image = this.mapNode.GetComponent<Image>();
            if (point.x >= -image.rectTransform.sizeDelta.x * this.canvasScale / 2f &&
                point.x <= image.rectTransform.sizeDelta.x * this.canvasScale / 2f &&
                point.y >= -image.rectTransform.sizeDelta.y * this.canvasScale / 2f &&
                point.y <= image.rectTransform.sizeDelta.y * this.canvasScale / 2f)
                return true;
            else
                return false;
        }
    }
}
