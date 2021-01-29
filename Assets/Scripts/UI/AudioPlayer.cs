using AppGame.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AppGame.Module.GameStart
{
    public class AudioPlayer : BaseView
    {
        /************************************************属性与变量命名************************************************/
        [SerializeField]
        private List<AudioChannel> channels;
        public static AudioPlayer Instance { get; private set; }
        /************************************************Unity方法与事件***********************************************/
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        protected override void Start()
        {
            base.Start();
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        /************************************************自 定 义 方 法************************************************/
        //初始化
        private void Initialize()
        {

        }
        [ContextMenu("收集音频播放通道")]
        private void CollectAudioChannel()
        {
            AudioChannel[] channels = GameObject.FindObjectsOfType<AudioChannel>();
            if (channels != null && channels.Length > 0)
            {
                this.channels = new List<AudioChannel>();
                this.channels.AddRange(channels.ToList().OrderBy(t => t.name).ToArray());
            }
            else
            {
                Debug.LogError("<><>Error: can not find objects of type 'AudioChannel'");
            }
        }
    }
}
