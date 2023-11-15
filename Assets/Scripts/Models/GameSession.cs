using UnityEngine;

namespace Assets.Scripts.Models
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;

        public PlayerData Data => _data;

        private void Awake()
        {
            if (IsSessionExit())
            { 
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                {
                    SafeSettings(gameSession);
                    return true;
                }
            }
            return false;
        }

        private void SafeSettings(GameSession gameSession)
        {
            var thisSession = GetComponent<GameSession>();
            thisSession.Data.Hp = gameSession.Data.Hp;
            thisSession.Data.Coins = gameSession.Data.Coins;
            thisSession.Data.AmountWeapon = gameSession.Data.AmountWeapon;
        }
    }
}
