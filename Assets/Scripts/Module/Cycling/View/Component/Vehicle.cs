using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class Vehicle : MonoBehaviour
    {
        /************************************************�������������************************************************/
        [SerializeField]
        private int coin;//�����������ֿ�
        [SerializeField]
        private int step;//�����������ֿ�
        [SerializeField]
        private string vehicleName;//��ͨ��������

        public int Coin { get { return this.coin; } }
        public int Step { get { return this.step; } }
        public string VehicleName { get { return this.vehicleName; } }
        /************************************************Unity�������¼�***********************************************/

        /************************************************�� �� �� �� ��************************************************/

    }
}
