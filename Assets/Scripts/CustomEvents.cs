using UnityEngine;
using UnityEngine.Events;

public static class CustomEvents
{
    // Combat events
    public static UnityEvent<GameObject> OnEnemyDeath = new UnityEvent<GameObject>();
    public static UnityEvent<GameObject> OnCombatEncounterCleared = new UnityEvent<GameObject>();
}
