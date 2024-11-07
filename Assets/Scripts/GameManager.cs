using UnityEngine;

[RequireComponent(typeof(BackgroundAudio))]
public class GameManager : MonoBehaviour
{
    private BackgroundAudio backgroundAudio;
    private PlayerState currentPlayerState;

    void Awake()
    {
        backgroundAudio = GetComponent<BackgroundAudio>();
    }

    void Start() {
        currentPlayerState = PlayerController.CurrentState;
        // Start background music
        backgroundAudio.PlayBackgroundMusic();
    }

    void Update()
    {
        if (currentPlayerState != PlayerController.CurrentState)
        {
            currentPlayerState = PlayerController.CurrentState;
            backgroundAudio.OnPlayerStateChange(currentPlayerState);
        }
    }
}
