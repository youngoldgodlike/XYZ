using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Component
{
    internal class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;

        public void Exit()
        {
            SceneManager.LoadScene(_sceneName);
        }
    }
}
