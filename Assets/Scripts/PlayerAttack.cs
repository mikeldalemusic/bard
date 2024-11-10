using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Weapon;

    private WeaponController weaponController;

    void Awake()
    {
        weaponController = Weapon.GetComponent<WeaponController>();
    }

    public void Attack()
    {
        // TODO: add cooldown
        // TODO: move weapon position
        weaponController.Attack();
    }
}
