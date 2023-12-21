using System;
using UnityEngine;

namespace Assets.Scripts.Models.Data.Properties
{
    [Serializable]
    public class FloatPersistentleProperty : PrefsPersistentleProperty<float>
    {
        public FloatPersistentleProperty(float defaultValue, string key) : base(defaultValue, key)
        {
            Init();
        }

        protected override void Write(float value)
        {
            PlayerPrefs.SetFloat(Key, Value);
            PlayerPrefs.Save();
        }

        protected override float Read(float defaultValue)
        {
            return PlayerPrefs.GetFloat(Key, defaultValue);
        }

        
    }
}