using UnityEngine;

namespace Assets.Scripts.Component
{
     class ArmHeroComponent : MonoBehaviour
     {
        
        public void ArmHero(GameObject go)
        {
            var hero = go.GetComponent<global::Hero>();

            if (hero != null)
            {
                hero.ArmHero();
            }

        }

     }
}
