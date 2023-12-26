using Assets.Scripts.Models.Data;
using Assets.Scripts.UI.Widgets;
using UnityEngine;

namespace Assets.Scripts.UI.Settings
{
    public class SettingsWindow : AnimatedWindow
    {
        [SerializeField] private AudioSettingsWidget _music;
        [SerializeField] private AudioSettingsWidget _sfx;
        
        protected override void Start()
        {
            base.Start();
            Animator.SetTrigger(Show);
            _music.SetModel(GameSettings.I.Music);
            _sfx.SetModel(GameSettings.I.Sfx);
           
        }
    }
}
