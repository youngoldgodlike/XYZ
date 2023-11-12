using UnityEngine;

public class GetWeaponComponent : MonoBehaviour
{
    private Hero _hero;

    private void Awake()
    {
        _hero = FindObjectOfType<Hero>();
    }
    
    public void AddWeapon()
    {
        _hero.AddWeapon();
    }
}
