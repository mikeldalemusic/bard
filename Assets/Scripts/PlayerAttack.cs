using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static bool CanAttack = true;
    public GameObject Weapon;
    [Range (0f, 1f)]
    public float attackCoolDownTime;

    private WeaponController weaponController;

    void Awake()
    {
        weaponController = Weapon.GetComponent<WeaponController>();
    }

    private IEnumerator AttackCooldown()
    {
        CanAttack = false;
        yield return new WaitForSeconds(attackCoolDownTime);
        CanAttack = true;
    }

    public void Attack()
    {
        // TODO: move weapon position
        weaponController.Attack();
        StartCoroutine(AttackCooldown());
    }

    public void TakeDamage()
    {
        // TODO: implement "stunned" state
        Debug.Log("Ouch!");
    }
}
