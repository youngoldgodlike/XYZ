using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Models
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;

        public PlayerData Data => _data;

        private void Awake()
        {
            LoadHud();
            if (IsSessionExit())
            { 
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(gameObject);
            }
        }

        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
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
            thisSession.Data.Hp.Value = gameSession.Data.Hp.Value;
        }
    }
}
