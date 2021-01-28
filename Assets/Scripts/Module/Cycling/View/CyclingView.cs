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
        [Inject]
        public ICameraUtil CameraUtil { get; set; }
        [SerializeField]
        private bool showGizmos;
        [SerializeField]
        private Image mask;
        [SerializeField]
        private RectTransform mapCanvas;
        [SerializeField, Range(0f, 1f)]
        private float canvasScale = 0.04f;
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
        private CameraEdge cameraEdge;
        private bool inRange;
        private Vector3 destination
        {
            get
            {
                return this.mapNode != null && this.mapNode.Points != null && this.mapNode.Points.Count > 0 ?
                       this.mapNode.Points[this.nodeIndex].position : Vector3.zero;
            }
        }
        private ScenicNode scenicNode;
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
            this.mapCanvas.localScale = new Vector3(this.canvasScale, this.canvasScale, 1);
        }
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
            if (this.cameraEdge == null) return;

            Vector3 playerPos = this.player.position;
            playerPos.z = this.camera.transform.position.z;
            if (!this.inRange)
            {
                Image image = this.mapNode.GetComponent<Image>();
                playerPos.x = Mathf.Clamp(playerPos.x, -image.rectTransform.sizeDelta.x * this.canvasScale / 2f + this.cameraEdge.Width / 2f, image.rectTransform.sizeDelta.x * this.canvasScale / 2f - this.cameraEdge.Width / 2f);
                playerPos.y = Mathf.Clamp(playerPos.y, -image.rectTransform.sizeDelta.y * this.canvasScale / 2f + this.cameraEdge.Height / 2f, image.rectTransform.sizeDelta.y * this.canvasScale / 2f - this.cameraEdge.Height / 2f);
            }
            this.camera.transform.DOMove(playerPos, this.lerp);
        }
        private void OnDrawGizmos()
        {
            if (!this.showGizmos) return;
            this.cameraEdge = this.CameraUtil.GetCameraEdge(this.mapNode.transform, this.camera.transform.position, this.player.position);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.cameraEdge.TopLeft, this.cameraEdge.TopRight);
            Gizmos.DrawLine(this.cameraEdge.TopRight, this.cameraEdge.BottomRight);
            Gizmos.DrawLine(this.cameraEdge.BottomRight, this.cameraEdge.BottomLeft);
            Gizmos.DrawLine(this.cameraEdge.BottomLeft, this.cameraEdge.TopLeft);

            RectTransform mapRectTransform = this.mapNode.GetComponent<RectTransform>();
            this.inRange = this.CameraUtil.PointInEdge(mapRectTransform, this.cameraEdge.TopLeft, this.canvasScale) &&
                           this.CameraUtil.PointInEdge(mapRectTransform, this.cameraEdge.TopRight, this.canvasScale) &&
                           this.CameraUtil.PointInEdge(mapRectTransform, this.cameraEdge.BottomRight, this.canvasScale) &&
                           this.CameraUtil.PointInEdge(mapRectTransform, this.cameraEdge.BottomLeft, this.canvasScale);
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
            if (this.nodeIndex + 1 < this.mapNode.Points.Count)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.MovePlayer(true));
            }
        }
        public void MoveBack()
        {
            if (this.nodeIndex > 0)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.MovePlayer(false));
            }
        }
        private IEnumerator MovePlayer(bool forward)
        {
            if (this.scenicNode != null) this.scenicNode.Hide();

            if (this.nodeIndex < 0 || this.nodeIndex >= this.mapNode.Points.Count)
                yield break;

            MapPointNode pointNode = null;
            do
            {
                this.nodeIndex += forward ? 1 : -1;
                do
                {
                    this.player.position = Vector3.MoveTowards(this.player.position, this.destination, this.step);
                    yield return new WaitForEndOfFrame();
                }
                while (Vector3.Distance(this.player.position, this.destination) > 0.01f);
                pointNode = this.mapNode.Points[this.nodeIndex].GetComponent<MapPointNode>();
            }
            while (pointNode == null);
            yield return new WaitForSeconds(0.5f);

            this.scenicNode = this.mapNode.Points[this.nodeIndex].GetComponent<ScenicNode>();
            if (this.scenicNode != null) this.scenicNode.Show();
            Debug.Log("stop");
        }
    }
}
