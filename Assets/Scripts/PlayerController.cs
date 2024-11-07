using UnityEngine;

public enum PlayerState {
    Default,
    Dialogue,
    Instrument,
}

[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAudio))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    public static PlayerState CurrentState = PlayerState.Default;

    private PlayerAnimation playerAnimation;
    private PlayerAudio playerAudio;
    private PlayerMovement playerMovement;
    private Vector2 movement;

    void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAudio = GetComponent<PlayerAudio>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void ToggleInstrument()
    {
        if (CurrentState == PlayerState.Default)
        {
            CurrentState = PlayerState.Instrument;
            // reset movement when switching to Instrument state
            movement = Vector2.zero;
            // stop walking animation
            playerAnimation.SetAnimationParams(movement);
        }
        else if (CurrentState == PlayerState.Instrument)
        {
            CurrentState = PlayerState.Default;
        }
    }

    void Update()
    {
        // TODO: handle pause state
        // Check if instrument was toggled
        if (PlayerInputManager.WasToggleInstrumentPressed)
        {
            ToggleInstrument();
        }

        // Perform actions depending on player state
        if (CurrentState == PlayerState.Default)
        {
            movement = PlayerInputManager.Movement;
            playerAnimation.SetAnimationParams(movement);
            playerAudio.PlayWalkingAudio(movement);
        }
        else if (CurrentState == PlayerState.Instrument)
        {
            string notePressed = PlayerInputManager.NotePressed;
            if (notePressed is not null)
            {
                playerAudio.PlayNote(notePressed);
            }
        }
    }

    void FixedUpdate()
    {
        // Move player rigidbody during FixedUpdate so that movement
        // is independent of framerate
        playerMovement.Move(movement);
    }
}
