using System;
using Assets.Scripts.Models.Data;
using Assets.Scripts.Models.Data.Properties;
using UnityEngine;

namespace Assets.Scripts.Component.Audio
{
    [RequireComponent(typeof(AudioSource))]
    
    public class AudioSettingComponent : MonoBehaviour
    {
        [SerializeField] private SoundsSetting _mode;
        private AudioSource _source;
        private FloatPersistentleProperty _model;
        
        private void Start()
        {
            _source = GetComponent<AudioSource>();
            
           _model = FindProperty();
           _model.OnChanged += OnSoundSettingChanged;
           OnSoundSettingChanged(_model.Value, _model.Value);
        }

        private void OnSoundSettingChanged(float newValue, float oldValue)
        {
            _source.volume = newValue;
        }

        private FloatPersistentleProperty FindProperty()
        {
            switch (_mode)
            {
                case  SoundsSetting.Music:
                    return GameSettings.Instance.Music;
                case  SoundsSetting.Sfx:
                    return GameSettings.Instance.Sfx;
            }
            
            throw  new ArgumentException("Undefined mode");
        }

        private void OnDestroy()
        {
            _model.OnChanged -= OnSoundSettingChanged;
        }
        
    }
}