using UnityEngine;

namespace AppGame.Module.Test
{
    public class ComingSoon : MonoBehaviour
    {
        /************************************************�������������************************************************/
        #region ע��ӿ�

        #endregion
        #region ҳ��UI���
        [SerializeField]
        private Transform cubeObject;
        [SerializeField]
        private Vector3 angle;
        #endregion
        #region ��������

        #endregion
        /************************************************Unity�������¼�***********************************************/
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
        /************************************************�� �� �� �� ��************************************************/
        public void GoBack()
        {
            //Todo: �˴����֪ͨflutterж����Ϸ�Ĵ���
        }
    }
}
