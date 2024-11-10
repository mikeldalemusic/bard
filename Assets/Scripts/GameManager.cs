using UnityEngine;

[RequireComponent(typeof(BackgroundAudio))]
public class GameManager : MonoBehaviour
{
    private BackgroundAudio backgroundAudio;
    private PlayerState currentPlayerState;

    void Awake()
    {
        backgroundAudio = GetComponent<BackgroundAudio>();

        // Subscribe to custom event
        CustomEvents.OnCombatEncounterCleared.AddListener(OnCombatEncounterCleared);
    }

    void OnDestroy()
    {
        // Remove listener on destroy to prevent memory leaks
        CustomEvents.OnCombatEncounterCleared.RemoveListener(OnCombatEncounterCleared);
    }

    void Start() {
        currentPlayerState = PlayerController.CurrentState;
        // Start background music
        backgroundAudio.PlayBackgroundMusic();
    }

    void Update()
    {
        // TODO: can use custom event for player state change instead of checking every frame
        if (currentPlayerState != PlayerController.CurrentState)
        {
            currentPlayerState = PlayerController.CurrentState;
            backgroundAudio.OnPlayerStateChange(currentPlayerState);
        }
    }

    void OnCombatEncounterCleared(GameObject combatEncounter)
    {
        Debug.Log("Combat cleared!");
    }
}
