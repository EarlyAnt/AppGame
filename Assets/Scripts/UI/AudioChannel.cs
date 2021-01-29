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
        private AudioChannelTypes type;
        [SerializeField]
        private AudioSource audioSource;

        public AudioChannelTypes Type { get { return this.type; } }
        public AudioSource AudioSource { get { return this.audioSource; } }
    }

    public enum AudioChannelTypes
    {
        Bgm = 0,
        AudioClip = 1
    }
}

