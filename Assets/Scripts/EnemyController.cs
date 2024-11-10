using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // TODO: make customizable in editor
    private float health = 1f;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            CustomEvents.OnEnemyDeath?.Invoke(gameObject);
        }
    }
}
