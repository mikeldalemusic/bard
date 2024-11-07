using UnityEngine;

[RequireComponent(typeof(BackgroundAudio))]
public class GameManager : MonoBehaviour
{
    private BackgroundAudio backgroundAudio;

    void Awake()
    {
        backgroundAudio = GetComponent<BackgroundAudio>();
    }

    void Start() {
        // Start background music
        backgroundAudio.PlayBackgroundMusic();
    }
}
