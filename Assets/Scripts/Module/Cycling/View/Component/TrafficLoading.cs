using AppGame.Config;
using AppGame.UI;
using AppGame.Util;
using Spine.Unity;
using UnityEngine;
using UnityEngine.UI;

namespace AppGame.Module.Cycling
{
    public class TrafficLoading : BaseView
    {
        /************************************************�������������************************************************/
        #region ע��ӿ�
        [Inject]
        public IMapConfig MapConfig { get; set; }
        [Inject]
        public IAssetBundleUtil AssetBundleUtil { get; set; }
        #endregion
        #region ҳ��UI���
        [SerializeField]
        private GameObject root;
        [SerializeField]
        private GameObject loadingRoot;
        [SerializeField]
        private Text fromCityBox;//�����������ֿ�
        [SerializeField]
        private Text fromCityPinYinBox;//�����������ֿ�
        [SerializeField]
        private Text toCityBox;//�����������ֿ�
        [SerializeField]
        private Text toCityPinYinBox;//�����������ֿ�
        #endregion
        #region ��������
        private string trafficAB = "cycling/traffic";
        private SkeletonGraphic traffic;
        #endregion
        /************************************************Unity�������¼�***********************************************/
        protected override void Start()
        {
            this.LoadTraffic();
        }
        /************************************************�� �� �� �� ��************************************************/
        //��ʾҳ��
        public void Show(Ticket ticket)
        {
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, false);
            if (ticket == null)
            {
                Debug.LogError("<><TrafficLoading.Show>Error: parameter 'ticket' is null");
                return;
            }

            this.fromCityBox.text = ticket.FromCityName;
            this.toCityBox.text = ticket.ToCityName;
            this.fromCityPinYinBox.text = ticket.FromCityPinYin;
            this.toCityPinYinBox.text = ticket.ToCityPinYin;
            this.root.SetActive(true);
            this.traffic.AnimationState.SetAnimation(0, ticket.Vehicle, true);
        }
        //����ҳ��
        public void Hide()
        {
            this.root.SetActive(false);
            this.dispatcher.Dispatch(GameEvent.SET_TOUCH_PAD_ENABLE, true);
        }
        //����Traffic����
        private void LoadTraffic()
        {
            this.AssetBundleUtil.LoadAssetBundleAsync(this.trafficAB, (assetBundle) =>
            {
                Object spineObject = assetBundle.LoadAsset("Traffic_Prefab");
                Object materialObject = assetBundle.LoadAsset(SpineParameters.MATERIAL_NAME);
                Material material = materialObject as Material;
                Shader shader = Shader.Find(material.shader.name);
                material.shader = shader;
                SkeletonGraphic prefabSpine = (spineObject as GameObject).GetComponent<SkeletonGraphic>();
                prefabSpine.material = material;
                GameObject trafficObject = GameObject.Instantiate(spineObject) as GameObject;
                trafficObject.name = "Traffic";
                trafficObject.transform.SetParent(this.loadingRoot.transform);
                trafficObject.transform.localPosition = Vector3.zero;
                trafficObject.transform.localRotation = Quaternion.identity;
                trafficObject.transform.localScale = Vector3.one * 1.25f;
                this.traffic = trafficObject.GetComponent<SkeletonGraphic>();
            },
            (errorText) =>
            {
                Debug.LogErrorFormat("<><TrafficLoading.LoadTraffic>Error: {0}", errorText);
            });
        }
    }
}
