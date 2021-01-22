using System.Collections;
using UnityEngine;

namespace Test.AddtiveScene
{
    public class Bubble : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private string mapName;
        private float time;
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {
            this.canvas.worldCamera = GameObject.FindObjectOfType<Camera>();
            CanvasManager.RegisterCanvas(this.canvas);
        }
        private void Start()
        {
            //this.StartCoroutine(this.PlayAnimation());
        }
        private void OnDestroy()
        {
            CanvasManager.UnregisterCanvas(GameObject.FindObjectOfType<Canvas>());
        }
        private void Update()
        {
            this.time += Time.deltaTime;
            if (this.time >= 1)
            {
                this.time = 0;
                Debug.LogWarningFormat("<><Bubble.Update>* * * * *{0}, {1}", this.mapName, System.DateTime.Now.ToString("HH:mm:ss:fff"));
            }
        }
        /************************************************自 定 义 方 法************************************************/
        private IEnumerator PlayAnimation()
        {
            while (this.gameObject.activeInHierarchy)
            {
                yield return new WaitForSeconds(2f);
                Debug.LogFormat("<><Bubble.PlayAnimation>+ + + + + {0}, {1}", this.mapName, System.DateTime.Now.ToString("HH:mm:ss:fff"));
            }
        }
    }
}