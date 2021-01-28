using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class Player : BasePlayer
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口
        [Inject]
        public ICameraUtil CameraUtil { get; set; }
        #endregion
        #region 页面UI组件
        [SerializeField]
        private RectTransform mapCanvas;
        [SerializeField, Range(0f, 1f)]
        private float canvasScale = 0.04f;
        [SerializeField]
        private Transform camera;
        [SerializeField, Range(0f, 5f)]
        private float lerp = 0.5f;
        [SerializeField]
        private bool showGizmos;
        #endregion
        #region 其他变量
        private CameraEdge cameraEdge;
        private RectTransform mapRectTransform;
        private bool inRange;
        private ScenicNode scenicNode;
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
            this.mapCanvas.localScale = new Vector3(this.canvasScale, this.canvasScale, 1);
            this.mapRectTransform = this.mapNode.GetComponent<RectTransform>();
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
            //计算相机可视区域的大小
            this.cameraEdge = this.CameraUtil.GetCameraEdge(this.mapNode.transform, this.camera.position, this.player.position);
            //计算相机可视区域是否超过地图
            this.inRange = this.CameraUtil.PointInEdge(this.mapRectTransform, this.cameraEdge.TopLeft, this.canvasScale) &&
                           this.CameraUtil.PointInEdge(this.mapRectTransform, this.cameraEdge.TopRight, this.canvasScale) &&
                           this.CameraUtil.PointInEdge(this.mapRectTransform, this.cameraEdge.BottomRight, this.canvasScale) &&
                           this.CameraUtil.PointInEdge(this.mapRectTransform, this.cameraEdge.BottomLeft, this.canvasScale);
            //根据玩家的位置调整相机的位置，使相机可视区域不超出地图
            this.camera.DOMove(this.GetCameraPosition(), this.lerp);
        }
        private void OnDrawGizmos()
        {
            if (!this.showGizmos) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(this.cameraEdge.TopLeft, this.cameraEdge.TopRight);
            Gizmos.DrawLine(this.cameraEdge.TopRight, this.cameraEdge.BottomRight);
            Gizmos.DrawLine(this.cameraEdge.BottomRight, this.cameraEdge.BottomLeft);
            Gizmos.DrawLine(this.cameraEdge.BottomLeft, this.cameraEdge.TopLeft);
        }
        /************************************************自 定 义 方 法************************************************/
        //初始化
        protected override void Initialize()
        {
            base.Initialize();
            Vector3 playerPos = this.player.position;
            playerPos.z = this.camera.position.z;
            this.camera.position = playerPos;
        }
        //向前移动
        public void MoveForward()
        {
            if (this.nodeIndex + 1 < this.mapNode.Points.Count)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.MovePlayer(true));
            }
        }
        //向后移动
        public void MoveBack()
        {
            if (this.nodeIndex > 0)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.MovePlayer(false));
            }
        }
        //玩家移动
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
            Debug.Log("<><Player.MovePlayer>Stop + + + + +");
        }
        //移动到指定位置
        public override void MoveToNode(string nodeID, bool lerp = false)
        {
            int index = this.mapNode.Points.FindIndex(t => t.name == nodeID);
            if (index >= 0 && index < this.mapNode.Points.Count)
            {
                this.nodeIndex = index;
                this.player.position = this.mapNode.Points[this.nodeIndex].position;
                this.camera.position = this.GetCameraPosition();
            }
            else
            {
                Debug.LogErrorFormat("<><Player.MoveToNode>Error: can not find the node named '{0}'", nodeID);
            }
        }
        //获取相机的合理位置(不超出地图)
        private Vector3 GetCameraPosition()
        {
            Vector3 playerPosition = this.player.position;
            playerPosition.z = this.camera.position.z;
            if (!this.inRange)
            {
                Image image = this.mapNode.GetComponent<Image>();
                playerPosition.x = Mathf.Clamp(playerPosition.x, -image.rectTransform.sizeDelta.x * this.canvasScale / 2f + this.cameraEdge.Width / 2f, image.rectTransform.sizeDelta.x * this.canvasScale / 2f - this.cameraEdge.Width / 2f);
                playerPosition.y = Mathf.Clamp(playerPosition.y, -image.rectTransform.sizeDelta.y * this.canvasScale / 2f + this.cameraEdge.Height / 2f, image.rectTransform.sizeDelta.y * this.canvasScale / 2f - this.cameraEdge.Height / 2f);
            }
            return playerPosition;
        }
    }
}
