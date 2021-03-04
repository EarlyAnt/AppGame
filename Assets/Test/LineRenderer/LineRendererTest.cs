using System.Collections.Generic;
using UnityEngine;

public class LineRendererTest : MonoBehaviour
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
    [SerializeField, Range(0, 100)]
    private int vertices = 0;
    [SerializeField, Range(0, 20)]
    private int sortingOrder = 1;
    [SerializeField]
    private bool useWorldSpace;
    [SerializeField, Range(1, 100)]
    private int lineCount = 20;
    [SerializeField, Range(2, 100)]
    private int devide = 10;
    [SerializeField, Range(0.1f, 1f)]
    private float step = 0.1f;
    private int index;
    [SerializeField]
    private List<Vector3> points = new List<Vector3>();
    /************************************************Unity方法与事件***********************************************/
    private void Start()
    {
        //this.devide = (int)System.Math.Ceiling(this.step / (this.startWidth / 2));
        //Debug.LogFormat("<><LineRendererTest.Start>devide: {0}", this.devide);

        this.step = this.startWidth / 2 * this.devide;
        Debug.LogFormat("<><LineRendererTest.Start>step: {0}", this.step);

        float x = 0, y = 0;
        for (int i = 0; i < this.lineCount; i++)
        {
            this.points.Add(new Vector2(x, y));

            if (i % 2 == 0)
                x += this.step;
            else if (i % 2 == 1)
                y += this.step;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.index + 1 < this.points.Count)
        {
            LineRenderer lineRenderer = this.GetLineRenderer();
            lineRenderer.positionCount = this.devide + 1;
            this.index += 1;
            Vector3 offset = (this.points[this.index] - this.points[this.index - 1]) * (1f / this.devide);
            for (int i = 0; i < this.devide + 1; i++)
            {
                lineRenderer.SetPosition(i, this.points[this.index - 1] + offset * (i + 1));
            }
        }
    }
    /************************************************自 定 义 方 法************************************************/
    private LineRenderer GetLineRenderer()
    {
        GameObject gameObject = new GameObject();
        gameObject.name = "Pen_" + (this.index + 1);
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

#region 代码例子
//public class LineRendererTest : MonoBehaviour
//{
//    private LineRenderer curLine;//当前实例化出来的线
//    private int posIndex;//当前线的Positions数组下标
//    private Vector2 mouseEndPos;//鼠标结束的位置

//    //线的参数
//    public Color startColor = Color.black;//线开始的颜色
//    public Color endColor = Color.black;//线结束的颜色
//    public float width = 0.1f;//线宽度
//    public int vertices = 90;//顶点数

//    private void Update()
//    {
//        if (Input.GetMouseButtonDown(0))
//        {
//            curLine = new GameObject("line").AddComponent<LineRenderer>();
//            curLine.material = new Material(Shader.Find("Sprites/Default"));
//            InitLine();
//        }

//        if (Input.GetMouseButton(0))
//        {
//            Vector3 mouseStartPos = GetMousePosInWorldSpace();
//            //优化性能
//            if (Vector2.Distance(mouseStartPos, mouseEndPos) >= 0.1f)
//            {
//                AddPositions(mouseStartPos);
//            }
//        }
//    }

//    /// <summary>
//    /// 初始化线
//    /// </summary>
//    private void InitLine()
//    {
//        posIndex = 0;
//        curLine.startColor = startColor;
//        curLine.endColor = endColor;
//        curLine.startWidth = width;
//        curLine.endWidth = width;
//        curLine.numCapVertices = vertices;
//        curLine.numCornerVertices = vertices;
//    }

//    /// <summary>
//    /// 将线段添加到当前线的Positions数组中
//    /// </summary>
//    private void AddPositions(Vector2 pos)
//    {
//        curLine.positionCount = ++posIndex;
//        curLine.SetPosition(posIndex - 1, pos);
//        mouseEndPos = pos;
//    }

//    /// <summary>
//    /// 得到鼠标在世界空间下的坐标
//    /// </summary>
//    /// <returns></returns>
//    private Vector2 GetMousePosInWorldSpace()
//    {
//        Vector3 worldPos = Camera.main.ScreenToWorldPoint(
//            new Vector3(Input.mousePosition.x, Input.mousePosition.y, -Camera.main.transform.position.z));
//        return worldPos;
//    }
//}
#endregion
