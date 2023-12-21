using Assets.Scripts.Models.Data.Properties;
using UnityEngine;

namespace Assets.Scripts.Models.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
    
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private FloatPersistentleProperty _music; 
        [SerializeField] private FloatPersistentleProperty _sfx;

        public FloatPersistentleProperty Music => _music;
        public FloatPersistentleProperty Sfx => _sfx;
        
        private static GameSettings _instance;
        public static GameSettings Instance => _instance == null ? LoadGameSettings() : _instance;

        private static GameSettings LoadGameSettings()
        {
            return Resources.Load<GameSettings>("GameSettings");
        }

        private void OnEnable()
        {
            _music = new FloatPersistentleProperty(1, SoundsSetting.Music.ToString());
            _sfx = new FloatPersistentleProperty(1, SoundsSetting.Sfx.ToString());
        }

        private void OnValidate()
        {
            _music.Validate();
            _sfx.Validate();
        }
    }

    public enum SoundsSetting
    {
        Music,
        Sfx
    }
}
