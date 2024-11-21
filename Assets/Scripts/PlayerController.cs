using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Default,
    Dialogue,
    Instrument,
    InstrumentMelody,
}

public enum FacingDirection {
    Up,
    Down,
    Left,
    Right,
}

[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerAudio))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAudioData))]
public class PlayerController : MonoBehaviour
{
    // Trigger custom event when CurrentState public variable is modified
    private static PlayerState _currentState = PlayerState.Default;
    public static PlayerState CurrentState
    {
        get { return _currentState; }
        set {
            _currentState = value;
            CustomEvents.OnPlayerStateChange?.Invoke(value);
        }
    }
    public static FacingDirection FacingDirection = FacingDirection.Down;
    // Set in editor
    public PlayerAudioData AudioData;
    public LayerMask DialogueLayer;

    private PlayerAnimation playerAnimation;
    private PlayerAttack playerAttack;
    private PlayerAudio playerAudio;
    private PlayerMovement playerMovement;
    private Vector2 movement;
    private string[] Melodies = new string[2]{
        MelodyData.Melody1,
        MelodyData.Melody2,
    };
    private Queue<string> lastPlayedNotes = new Queue<string>(new string[MelodyData.MelodyLength]{
        null,
        null,
        null,
        null,
    });

    void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAttack = GetComponent<PlayerAttack>();
        playerAudio = GetComponent<PlayerAudio>();
        playerMovement = GetComponent<PlayerMovement>();
        // Subscribe to custom event
        CustomEvents.OnDialogueEnd.AddListener(OnDialogueEnd);
    }

    void OnDestroy()
    {
        // Remove listener on destroy to prevent memory leaks
        CustomEvents.OnDialogueEnd.RemoveListener(OnDialogueEnd);
    }

    private void OnDialogueEnd(Dialogue dialogue)
    {
        CurrentState = PlayerState.Default;
        Debug.Log("dialogue completed");
    }

    private void ToggleInstrument()
    {
        if (CurrentState == PlayerState.Default)
        {
            CurrentState = PlayerState.Instrument;
            // reset movement when switching to Instrument state
            movement = Vector2.zero;
            // stop walking animation
            playerAnimation.SetAnimationParams(movement);
            // TODO: make player face down
            // TODO: camera zoom in
        }
        else if (CurrentState == PlayerState.Instrument)
        {
            CurrentState = PlayerState.Default;
        }
    }

    private string FindMelodyToPlay(Queue<string> lastPlayedNotes)
    {
        string lastPlayedNotesString = string.Join("", lastPlayedNotes);
        foreach (string melody in Melodies)
        {
            string[] melodyInputs = MelodyData.MelodyInputs[melody];
            string melodyInputsString = string.Join("", melodyInputs);
            if (lastPlayedNotesString == melodyInputsString)
            {
                return melody;
            }
        }
        return null;
    }

    private IEnumerator PlayMelodyAfterDelay(string melody)
    {
        CurrentState = PlayerState.InstrumentMelody;
        // After playing last note, wait before starting the melody audio
        yield return new WaitForSeconds(AudioData.TimeBeforeMelody);
        playerAudio.PlayMelody(melody);
        // After starting the melody audio, wait before giving control back to the player
        yield return new WaitForSeconds(AudioData.MelodyCooldownTime);
        CurrentState = PlayerState.Default;
    }

    public void TakeDamage()
    {
        playerAttack.TakeDamage();
    }

    private FacingDirection determineFacingDirection(Vector2 movement)
    {
        // If moving diagonal, use horizontal direction
        bool isMovingHorizontal = Math.Abs(movement.x) > 0;
        if (isMovingHorizontal)
        {
            return movement.x > 0 ? FacingDirection.Right : FacingDirection.Left;
        }
        else
        {
            return movement.y > 0 ? FacingDirection.Up : FacingDirection.Down;
        }
    }

    private Dialogue checkForDialogueCollision()
    {
        Vector3 interactPos = transform.position; // Player's center position

        // Define the size of the rectangle (adjust dynamically based on FacingDirection)
        Vector2 interactionZoneSize;

        switch (FacingDirection)
        {
            case FacingDirection.Up:
            case FacingDirection.Down:
                interactionZoneSize = new Vector2(0.5f, 1f); // Narrow width, taller height
                break;
            case FacingDirection.Left:
            case FacingDirection.Right:
                interactionZoneSize = new Vector2(1f, 0.5f); // Wider width, shorter height
                break;
            default:
                interactionZoneSize = new Vector2(0.5f, 0.5f); // Default size (failsafe)
                break;
        }

        // Offset the interaction rectangle based on FacingDirection
        switch (FacingDirection)
        {
            case FacingDirection.Up:
                interactPos += Vector3.up * interactionOffset;
                break;
            case FacingDirection.Down:
                interactPos += Vector3.down * interactionOffset;
                break;
            case FacingDirection.Left:
                interactPos += Vector3.left * interactionOffset;
                break;
            case FacingDirection.Right:
                interactPos += Vector3.right * interactionOffset;
                break;
        }

        // DEBUG: Make interaction zone visible in Scene view via red rectangle
        DebugDrawRectangle(interactPos, interactionZoneSize, Color.red);

        //Check for interactable objects in the interaction zone
        Collider2D hit = Physics2D.OverlapBox(interactPos, interactionZoneSize, 0, DialogueLayer);
        if (hit != null)
        {
            SignController signController = hit.GetComponent<SignController>();
            if (signController != null)
            {
                Debug.Log($"Found SignController on object: {hit.gameObject.name}");
                return signController.dialogue; // Return the assigned Dialogue ScriptableObject
            }
        }

        Debug.Log("No interactable object found in the interaction zone.");
        return null;
    }



    //DEBUG: Make interaction zone visible in Scene view via red rectangle
    private void DebugDrawRectangle(Vector3 center, Vector2 size, Color color, float duration = 0.1f)
    {
        // Calculate the corners of the rectangle
        Vector3 topLeft = center + new Vector3(-size.x / 2, size.y / 2, 0);
        Vector3 topRight = center + new Vector3(size.x / 2, size.y / 2, 0);
        Vector3 bottomLeft = center + new Vector3(-size.x / 2, -size.y / 2, 0);
        Vector3 bottomRight = center + new Vector3(size.x / 2, -size.y / 2, 0);

        // Draw the edges of the rectangle with a longer duration
        Debug.DrawLine(topLeft, topRight, color, duration);
        Debug.DrawLine(topRight, bottomRight, color, duration);
        Debug.DrawLine(bottomRight, bottomLeft, color, duration);
        Debug.DrawLine(bottomLeft, topLeft, color, duration);
    }

    //Interaction zone adjustable in Inspector
    [SerializeField] private Vector2 interactionZoneSize = new Vector2(1f, 0.5f); // Width and Height
    [SerializeField] private float interactionOffset = 0.5f; // Distance from the player's center


    void Update()
    {
        // Handle pause state
        if (PlayerInputManager.isPaused)
        {
            return;
        }
        // Check if instrument was toggled
        if (PlayerInputManager.WasToggleInstrumentPressed)
        {
            ToggleInstrument();
        }

        // Perform actions depending on player state
        if (CurrentState == PlayerState.Default)
        {
            if (PlayerInputManager.WasDialoguePressed)
            {
                Dialogue dialogue = checkForDialogueCollision();
                if (dialogue != null)
                {
                    CurrentState = PlayerState.Dialogue;
                    CustomEvents.OnDialogueStart?.Invoke(dialogue);

                    // Pass facing direction to the DialogueManager
                    DialogueManager.StartDialogue(dialogue, FacingDirection);
                }
            }
    

            movement = PlayerInputManager.Movement;
            // If moving, set FacingDirection
            if (movement != Vector2.zero)
            {
                FacingDirection = determineFacingDirection(movement);
            }
            playerAnimation.SetAnimationParams(movement);
            playerAudio.PlayWalkingAudio(movement);

            if (PlayerInputManager.WasAttackPressed && PlayerAttack.CanAttack)
            {
                playerAttack.Attack();
                playerAudio.PlayAttackChord();
            }
        }
        else if (CurrentState == PlayerState.Instrument)
        {
            string notePressed = PlayerInputManager.NotePressed;
            if (notePressed is not null)
            {
                playerAudio.PlayNote(notePressed);
                // Remove 1st note from queue, and add new note to end of queue,
                // so that the new 1st note is now the 4th-last note played
                lastPlayedNotes.Dequeue();
                lastPlayedNotes.Enqueue(notePressed);
                // Check if should play Melody
                string melodyToPlay = FindMelodyToPlay(lastPlayedNotes);
                if (melodyToPlay != null)
                {
                    // Start coroutine to change state, play song, and then return to default state
                    StartCoroutine(PlayMelodyAfterDelay(melodyToPlay));
                }
            }
        }
        else if (CurrentState == PlayerState.Dialogue)
        {
            if (PlayerInputManager.WasDialoguePressed)
            {
                DialogueManager.AdvanceCurrentDialogue();
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
