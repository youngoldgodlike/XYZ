using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Scripts.Hero
{
    public class HeroInputReader : MonoBehaviour
    {
        [SerializeField] private Creatures.Hero _hero;

        public void OnMovement(InputAction.CallbackContext context)
        {
            var direction =  context.ReadValue<Vector2>();
            _hero.SetDirection(direction);
        }

        public void OnSaySomething(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.Attack();
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.Attack();
        }

        public void OnThrow(InputAction.CallbackContext context)
        {
            if (context.started)
                _hero.StartThrowing();
            if (context.canceled)
                _hero.PerformThrowing();
        }
    
        public void OnNextItem(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.NextItem();
        }

        public void OnUseHeal(InputAction.CallbackContext context)
        {
            if (context.performed)
                _hero.UseHeal();
        }
        
    }
}



