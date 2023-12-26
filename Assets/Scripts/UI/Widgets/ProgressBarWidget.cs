using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.Widgets
{
    public class ProgressBarWidget : MonoBehaviour
    {
        [SerializeField] private Image _bar;
        
        public void SetProgress(float value, float maxProgress)
        {
            var progress = value / maxProgress;
            _bar.fillAmount = progress;
        }
    }
}