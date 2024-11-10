using UnityEngine;

[RequireComponent(typeof(LayerMask))]
public class WeaponController : MonoBehaviour
{
    public float AttackRange;
    public LayerMask enemyLayer;

    // TODO: make customizable in editor
    private float damage = 1f;

    public void Attack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, AttackRange, enemyLayer);
        foreach (Collider2D enemy in hitEnemies)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            enemyController.TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRange);
    }
}
