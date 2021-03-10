using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class Vehicle : MonoBehaviour
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private int coin;//城市名字文字框
        [SerializeField]
        private int step;//景点名字文字框
        [SerializeField]
        private string vehicleName;//交通工具名称

        public int Coin { get { return this.coin; } }
        public int Step { get { return this.step; } }
        public string VehicleName { get { return this.vehicleName; } }
        /************************************************Unity方法与事件***********************************************/

        /************************************************自 定 义 方 法************************************************/

    }
}
