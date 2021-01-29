using AppGame.Config;
using AppGame.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AppGame.Module.GameStart
{
    public class AudioPlayer : BaseView
    {
        /************************************************�������������************************************************/
        [Inject]
        public IAudioConfig AudioConfig { get; set; }
        [SerializeField]
        private List<AudioChannel> channels;
        public static AudioPlayer Instance { get; private set; }
        /************************************************Unity�������¼�***********************************************/
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
        /************************************************�� �� �� �� ��************************************************/
        //��ʼ��
        private void Initialize()
        {

        }
        [ContextMenu("�ռ���Ƶ����ͨ��")]
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
        //��������Ƭ��
        public void PlayAudioClip(string audioName, Action completeCallback, float delayStart = 1f, float delayEnd = 1f)
        {
            Audio audio = this.AudioConfig.GetAudio(audioName);
            if (audio == null)
            {
                Debug.LogErrorFormat("<><AudioPlayer.PlayAudioClip>Error: can not find audio named '{0}'", audioName);
                return;
            }

        }
        //���ű�������
        public void PlayBgm(string bgmName, bool loop = true, float delayStart = 1f, float delayEnd = 1f)
        {

        }
        //ֹͣ��Ƶ����
        public void StopAllAudios(AudioChannelTypes type)
        {
            if (type == AudioChannelTypes.Bgm)
            {
                AudioChannel channel = this.channels.Find(t => t.Type == AudioChannelTypes.Bgm);
                if (channel != null) channel.AudioSource.Stop();
            }
            else if (type == AudioChannelTypes.AudioClip)
            {
                AudioChannel channel = this.channels.Find(t => t.Type == AudioChannelTypes.AudioClip);
                if (channel != null) channel.AudioSource.Stop();
            }
            else if (type == (AudioChannelTypes.Bgm | AudioChannelTypes.AudioClip))
            {
                this.channels.ForEach(t => t.AudioSource.Stop());
            }
        }

        private string GetRandomAudioFile(Audio audio)
        {
            if (audio == null)
            {
                Debug.LogError("<><AudioPlayer.GetRandomAudioFile>Error: parameter 'audio' is null");
                return null;
            }
            else if (audio.Files == null || audio.Files.Count == 0)
            {
                Debug.LogError("<><AudioPlayer.GetRandomAudioFile>Error: parameter 'audio.Files' is null or empty");
                return null;
            }

            int index = UnityEngine.Random.Range(0, audio.Files.Count * 10) % audio.Files.Count;
            return audio.Files[index].Path;
        }
        private IEnumerator PlayAudio(AudioSource audioSource, AudioClip audioClip, bool loop, Action completeCallback, float delayStart, float delayEnd)
        {
            if (delayStart > 0)
                yield return new WaitForSeconds(delayStart);
            audioSource.clip = audioClip;
            audioSource.loop = loop;
            audioSource.Play();
            while (audioSource.isPlaying)
                yield return null;
            if (delayEnd > 0)
                yield return new WaitForSeconds(delayEnd);
            if (completeCallback != null)
                completeCallback();
        }
    }
}
