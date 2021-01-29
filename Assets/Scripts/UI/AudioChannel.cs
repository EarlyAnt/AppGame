using DG.Tweening;
using strange.extensions.mediation.impl;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppGame.UI
{
    public class AudioChannel : MonoBehaviour
    {
        [SerializeField]
        private AudioPlayerTypes type;
        [SerializeField]
        private AudioSource audioSource;

        public AudioPlayerTypes Type { get { return this.type; } }
        public AudioSource Player { get { return this.audioSource; } }
    }

    public enum AudioPlayerTypes
    {
        Bgm = 0,
        AudioClip = 1
    }
}

