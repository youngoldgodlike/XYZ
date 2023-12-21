using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Widgets
{
    public class ProgressBarWidget : MonoBehaviour
    {
        [SerializeField] private Image _bar;

        public void SetProgress(float progress)
        {
            _bar.fillAmount = progress;
        }

        public void SetProgress(float progress, float maxProgress)
        {
            var value = progress / maxProgress;
            SetProgress(value);
        }
    }
}