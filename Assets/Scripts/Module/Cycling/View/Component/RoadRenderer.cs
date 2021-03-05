using UnityEngine;

namespace AppGame.Module.Cycling
{
    public class RoadRenderer : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Color startColor = Color.white;
        [SerializeField]
        private Color endColor = Color.white;
        [SerializeField, Range(0.1f, 10f)]
        private float startWidth = 0.1f;
        [SerializeField, Range(0.1f, 10f)]
        private float endWidth = 0.1f;
        [SerializeField, Range(0.1f, 10f)]
        private float offset = 0.1f;
        [SerializeField, Range(0, 100)]
        private int vertices = 0;
        [SerializeField, Range(0, 20)]
        private int sortingOrder = 1;
        [SerializeField]
        private bool useWorldSpace;
        private int index;
        private int lineCount;
        private Vector3 lastPoint;
        private LineRenderer lineRenderer;
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/
        public void DrawPoint(Vector3 point, int axis, bool newLine, bool cornerOffset = true)
        {
            if (newLine || this.lineRenderer == null)
            {
                int flag = point[axis] >= this.lastPoint[axis] ? 1 : -1;
                this.lastPoint = point;
                this.DrawCorner(axis, flag, cornerOffset);
                this.index = 0;
                this.lineCount += 1;
                this.lineRenderer = this.GetLineRenderer();
            }
            else
            {
                this.lastPoint = point;
                this.index += 1;
            }

            this.DrawPoint(this.index, point);
        }
        public void Clear()
        {
            while (this.transform.childCount > 0)
                GameObject.DestroyImmediate(this.transform.GetChild(0));
        }
        private void DrawCorner(int axis, int flag, bool cornerPlus)
        {
            if (this.lineRenderer != null)
            {
                Vector3 corner;
                int index = 0;

                if (!cornerPlus)
                {//拐角方块不使用偏移
                    flag = 0;
                }

                if (axis == 0)
                {
                    //横向补齐
                    index = 0;
                    this.lineCount += 1;
                    this.lineRenderer = this.GetLineRenderer(true);
                    corner = new Vector3(this.lastPoint.x - this.startWidth / 2f - this.offset * flag, this.lastPoint.y, 0);
                    this.DrawPoint(index, corner);

                    corner = new Vector3(this.lastPoint.x + this.startWidth / 2f - this.offset * flag, this.lastPoint.y, 0);
                    this.DrawPoint(++index, corner);
                }
                else if (axis == 1)
                {
                    //纵向补齐
                    index = 0;
                    this.lineCount += 1;
                    this.lineRenderer = this.GetLineRenderer(true);
                    corner = new Vector3(this.lastPoint.x, this.lastPoint.y - this.startWidth / 2f - this.offset * flag, 0);
                    this.DrawPoint(index, corner);

                    corner = new Vector3(this.lastPoint.x, this.lastPoint.y + this.startWidth / 2f - this.offset * flag, 0);
                    this.DrawPoint(++index, corner);
                }
            }
        }
        private void DrawPoint(int index, Vector3 point)
        {
            if (this.lineRenderer != null)
            {
                this.lineRenderer.positionCount = index + 1;
                this.lineRenderer.SetPosition(index, point);
            }
        }
        private LineRenderer GetLineRenderer(bool corner = false)
        {
            GameObject gameObject = new GameObject();
            gameObject.name = corner ? string.Format("Pen_{0}_Corner", this.lineCount) : string.Format("Pen_{0}", this.lineCount);
            gameObject.transform.SetParent(this.transform);
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localScale = Vector3.one;
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.startColor = this.startColor;
            lineRenderer.endColor = this.endColor;
            lineRenderer.startWidth = this.startWidth;
            lineRenderer.endWidth = this.endWidth;
            lineRenderer.numCapVertices = this.vertices;
            lineRenderer.numCornerVertices = this.vertices;
            lineRenderer.sortingOrder = this.sortingOrder;
            lineRenderer.useWorldSpace = this.useWorldSpace;
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            return lineRenderer;
        }
    }
}
