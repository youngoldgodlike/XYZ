using Assets.Scripts.Models;
using Assets.Scripts.Models.Difinitions;
using Assets.Scripts.UI.Widgets;
using UnityEngine;

namespace Assets.Scripts.UI.HUD
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private ProgressBarWidget _healthBar;

        private GameSession _session;
        
        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.OnChanged += OnHealthChanged;
            
            OnHealthChanged(_session.Data.Hp.Value, 0);
        }

        private void OnHealthChanged(int newValue, int oldValue)
        {
            var maxHealth = DefsFacade.I.Player.MaxHealth;
            _healthBar.SetProgress( newValue, maxHealth);
        }
        
        
        private void OnDestroy()
        {
            _session.Data.Hp.OnChanged -= OnHealthChanged;
        }
    }
}