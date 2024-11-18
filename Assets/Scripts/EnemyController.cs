using System.Collections;
using UnityEngine;

public enum EnemyState {
    Default,
    Agro,
    Attacking,
    AttackCooldown,
}

public class EnemyController : MonoBehaviour
{
    public LayerMask PlayerLayer;
    public float AgroTimeBeforeAttack;
    public float AttackCooldownTime;
    public float MaxTravelDistance;
    public float MoveSpeed;

    // TODO: make customizable in editor
    private float health = 1f;
    private EnemyState currentState = EnemyState.Default;
    private Vector2 travelPoint;
    private GameObject target;
    private CircleCollider2D circleCollider2D;

    void Start()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0f)
        {
            CustomEvents.OnEnemyDeath?.Invoke(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (currentState == EnemyState.Attacking && Utils.HasTargetLayer(PlayerLayer, collision.gameObject))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage();
        }
    }

    private IEnumerator StartAttack(GameObject other)
    {
        Rigidbody2D targetRigidbody = other.GetComponent<Rigidbody2D>();
        currentState = EnemyState.Agro;
        yield return new WaitForSeconds(AgroTimeBeforeAttack);
        currentState = EnemyState.Attacking;

        // Calculate the direction towards the target
        Vector2 direction = (targetRigidbody.position - (Vector2)transform.position).normalized;
        // Calculate the exact travel point within the max distance
        travelPoint = (Vector2)transform.position + direction * MaxTravelDistance;
        currentState = EnemyState.Attacking;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (currentState == EnemyState.Default && Utils.HasTargetLayer(PlayerLayer, other.gameObject))
        {
            target = other.gameObject;
            StartCoroutine(StartAttack(other.gameObject));
        }
    }

    private IEnumerator AttackCooldown()
    {
        currentState = EnemyState.AttackCooldown;
        yield return new WaitForSeconds(AttackCooldownTime);
        currentState = EnemyState.Default;
        // Check if player is still in agro range. If so, attack again
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius, PlayerLayer);
        if (hitPlayers.Length > 0)
        {
            target = hitPlayers[0].gameObject;
            StartCoroutine(StartAttack(target));
        }
    }

    void FixedUpdate()
    {
        if (currentState == EnemyState.Attacking)
        {
            transform.position = Vector2.Lerp(transform.position, travelPoint, MoveSpeed * Time.fixedDeltaTime);
            // Stop once close enough to avoid overshooting
            if (Vector2.Distance(transform.position, travelPoint) < 0.1f)
            {
                StartCoroutine(AttackCooldown());
            }
        }
    }
}
