using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.PauseMenu
{
    public class PauseMenuWindow : AnimatedWindow
    {
        private Action _closeAction;

        private bool _isOpen;
        
        public void OnUseMenu(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!_isOpen)
                {
                    _isOpen = true;
                    gameObject.SetActive(true);
                  //  Animator.SetTrigger(Show);
                }
                else
                {
                    _isOpen = false;
                    gameObject.SetActive(false);
                }
            }
        }
        
        public void OnRestart()
        {
            _closeAction = () => { SceneManager.LoadScene(SceneManager.GetActiveScene().name); };
            Close();
        }

        public void OnShowSettings()
        {
            var window = Resources.Load<GameObject>("Ui/SettingsWindow");
            var canvas = FindObjectOfType<Canvas>();
            Instantiate(window, canvas.transform);
        }

        public void OnBackMainMenu()
        {
            _closeAction = () => { SceneManager.LoadScene("MainMenu"); };
            Close();
        }

        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
        }
    }
}
