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
        [SerializeField]
        private RectTransform scenicCanvas;
        [SerializeField, Range(0f, 1f)]
        private float canvasScale = 0.04f;
        [SerializeField]
        private RectTransform mapRectTransform;
        [SerializeField]
        private Transform camera;
        [SerializeField, Range(0f, 1f)]
        private float lerp = 0.1f;
        [SerializeField]
        private bool showGizmos;
        [SerializeField]
        private RoadRenderer roadRenderer;
        #endregion
        #region 其他变量
        private CameraEdge cameraEdge;
        private bool inRange;
        private Vector3 lastPos;
        private int directon = 0;
        private int lastDirection = 0;
        public MapPointNode CurrentNode
        {
            get
            {
                if (this.nodeIndex >= 0 && this.nodeIndex < this.mapNode.Points.Count)
                {
                    return this.mapNode.Points[this.nodeIndex].GetComponent<MapPointNode>();
                }
                else
                {
                    Debug.LogError("<><Player.CurrentNode>Error: nodeIndex is out of range");
                    return null;
                }
            }
        }
        #endregion
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
            this.mapCanvas.localScale = new Vector3(this.canvasScale, this.canvasScale, 1);
            this.scenicCanvas.localScale = new Vector3(this.canvasScale, this.canvasScale, 1);
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
            this.camera.transform.position = Vector3.Lerp(this.camera.transform.position, this.GetCameraPosition(), this.lerp);
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
            if (this.IsMoving)
                return;

            if (this.nodeIndex + 1 < this.mapNode.Points.Count)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.MovePlayer(true));
            }
            else
            {
                this.OnStopped();
            }
        }
        //向后移动
        public void MoveBack()
        {
            if (!this.IsMoving && this.nodeIndex > 0)
            {
                this.StopAllCoroutines();
                this.StartCoroutine(this.MovePlayer(false));
            }
        }
        //玩家移动
        private IEnumerator MovePlayer(bool forward)
        {
            if (this.nodeIndex < 0 || this.nodeIndex >= this.mapNode.Points.Count)
                yield break;

            this.IsMoving = true;
            MapPointNode pointNode = null;
            do
            {//移动玩家至下一个节点
                this.nodeIndex += forward ? 1 : -1;
                do
                {
                    this.lastPos = this.player.position;
                    this.player.position = Vector3.MoveTowards(this.player.position, this.destination, this.step);
                    this.roadRenderer.DrawPoint(this.player.position, this.directon, this.DirectionChanged(this.player.position, this.lastPos));
                    yield return new WaitForEndOfFrame();
                }
                while (Vector3.Distance(this.player.position, this.destination) > 0.01f);
                pointNode = this.mapNode.Points[this.nodeIndex].GetComponent<MapPointNode>();
            }
            while (pointNode == null || pointNode.NodeType == NodeTypes.EmptyNode);

            this.SetPointIcon(true);
            this.OnStopped();
            Debug.Log("<><Player.MovePlayer>Stop + + + + +");
        }
        //移动到指定位置
        public override void MoveToNode(string nodeID, bool lerp = false)
        {
            this.SetPointIcon(false);
            this.roadRenderer.Clear();
            int targetNodeIndex = this.mapNode.Points.FindIndex(t => t.GetComponent<MapPointNode>().ID == nodeID);
            if (targetNodeIndex >= 0 && targetNodeIndex < this.mapNode.Points.Count)
            {
                this.nodeIndex = targetNodeIndex;
                this.player.position = this.mapNode.Points[this.nodeIndex].position;
                this.camera.position = this.GetCameraPosition();
                this.SetPointIcon(true);
            }
            else
            {
                Debug.LogErrorFormat("<><Player.MoveToNode>Error: can not find the node named '{0}'", nodeID);
            }

            int index = 0;
            Vector3 lastNode = this.mapNode.Points[0].position;
            Vector3 currentNode = this.mapNode.Points[0].position;
            while (index <= targetNodeIndex)
            {
                currentNode = this.mapNode.Points[index].position;
                if (this.DirectionChanged(currentNode, lastNode))
                    this.roadRenderer.DrawPoint(lastNode, this.directon, true, false);
                this.roadRenderer.DrawPoint(currentNode, this.directon, false);

                lastNode = this.mapNode.Points[index].position;
                index += 1;
            }
        }
        //获取相机的合理位置(不超出地图)
        private Vector3 GetCameraPosition()
        {
            Vector3 playerPosition = this.player.position;
            playerPosition.z = this.camera.position.z;
            if (!this.inRange && this.cameraEdge != null)
            {
                playerPosition.x = Mathf.Clamp(playerPosition.x, -this.mapRectTransform.sizeDelta.x * this.canvasScale / 2f + this.cameraEdge.Width / 2f, this.mapRectTransform.sizeDelta.x * this.canvasScale / 2f - this.cameraEdge.Width / 2f);
                playerPosition.y = Mathf.Clamp(playerPosition.y, -this.mapRectTransform.sizeDelta.y * this.canvasScale / 2f + this.cameraEdge.Height / 2f, this.mapRectTransform.sizeDelta.y * this.canvasScale / 2f - this.cameraEdge.Height / 2f);
            }
            return playerPosition;
        }
        //判断是否已改变方向
        private bool DirectionChanged(Vector3 pos1, Vector3 pos2)
        {
            if (pos1.x == pos2.x)
                this.directon = 0;
            else if (pos1.y == pos2.y)
                this.directon = 1;

            if (this.lastDirection != this.directon)
            {
                this.lastDirection = this.directon;
                return true;
            }
            else return false;
        }
        //点亮已经过的点的图标
        private void SetPointIcon(bool light)
        {
            if (light)
            {
                int index = 0;
                while (index <= this.nodeIndex)
                {
                    MapPointNode mapPointNode = this.mapNode.Points[index].GetComponent<MapPointNode>();
                    if (mapPointNode != null && mapPointNode.NodeType != NodeTypes.EmptyNode)
                        mapPointNode.SetIcon(true);
                    index += 1;
                }
            }
            else
            {
                this.mapNode.Points.ForEach(t =>
                {
                    MapPointNode mapPointNode = t.GetComponent<MapPointNode>();
                    if (mapPointNode != null && mapPointNode.NodeType != NodeTypes.EmptyNode)
                        mapPointNode.SetIcon(false);
                });
            }
        }
        //当玩家移动停止时
        private void OnStopped()
        {
            this.IsMoving = false;

            MapPointNode mapPointNode = this.mapNode.Points[this.nodeIndex].GetComponent<MapPointNode>();
            if (mapPointNode != null)
                this.dispatcher.Dispatch(GameEvent.INTERACTION, mapPointNode);
            else
                Debug.LogError("<><Player.OnStopped>Error: can not find the component 'MapPointNode'");
        }
    }
}
