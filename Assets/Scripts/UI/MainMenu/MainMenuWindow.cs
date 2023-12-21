using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        [SerializeField] private string _sceneName;
        private Action _closeAction;

        protected override void Start()
        {
            base.Start();
            Animator.SetTrigger(Show);
        }
        
        public void OnShowSettings()
        {
            var window = Resources.Load<GameObject>("Ui/SettingsWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
        }

        public void OnStartGame()
        {
            _closeAction = () => { SceneManager.LoadScene(_sceneName); };
            Close();
        }

        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();
                
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                
            };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}
