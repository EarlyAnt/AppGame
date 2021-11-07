using UnityEngine;

namespace AppGame.Module.Test
{
    public class ComingSoon : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        #region 注入接口

        #endregion
        #region 页面UI组件
        [SerializeField]
        private Transform cubeObject;
        [SerializeField]
        private Vector3 angle;
        #endregion
        #region 其他变量

        #endregion
        /************************************************Unity方法与事件***********************************************/
        private void Awake()
        {

        }
        private void Start()
        {

        }
        private void Update()
        {
            this.cubeObject.Rotate(this.angle);
        }
        private void OnDestroy()
        {

        }
        /************************************************自 定 义 方 法************************************************/
        public void GoBack()
        {
            //Todo: 此处添加通知flutter卸载游戏的代码
        }
    }
}
