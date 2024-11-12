using UnityEngine;
using UnityEngine.Events;

public static class CustomEvents
{
    // Player events
    public static UnityEvent<PlayerState> OnPlayerStateChange = new UnityEvent<PlayerState>();

    // Combat events
    public static UnityEvent<GameObject> OnEnemyDeath = new UnityEvent<GameObject>();
    public static UnityEvent<GameObject> OnCombatEncounterCleared = new UnityEvent<GameObject>();
}
