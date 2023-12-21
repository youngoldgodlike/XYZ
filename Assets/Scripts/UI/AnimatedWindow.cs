using UnityEngine;

namespace Assets.Scripts.UI
{
    public class AnimatedWindow : MonoBehaviour
    {
        protected Animator Animator;
        protected static readonly int Show = Animator.StringToHash("Show");
        protected static readonly int Hide = Animator.StringToHash("Hide");

        protected virtual void Start()
        {
            Animator = GetComponent<Animator>();
        }

        public void Close()
        {
            Animator.SetTrigger(Hide);
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
        
    }
}
