﻿using System;
using UnityEngine;

namespace Assets.Scripts.Component.Audio
{
    public class PlaySoundsComponent : MonoBehaviour
    {
        [SerializeField] private AudioData[] _sounds;
        private AudioSource _source;

        public void Play(string id)
        {
            if (_source == null)
                _source = GameObject.FindWithTag("SfxAudioSource").GetComponent<AudioSource>();
            
            foreach (var audioData in _sounds)
            {
                if (audioData.Id != id) continue;
                
                _source.PlayOneShot(audioData.Clip);
                break;
            }
        }

        [Serializable]
        public class AudioData
        {
            [SerializeField] private string _id;
            [SerializeField] private AudioClip _clip;

            public string Id => _id;
            public AudioClip Clip => _clip;

        }
    }

    
}