using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState {
    Default,
    Dialogue,
    Instrument,
    InstrumentMelody,
}

[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAttack))]
[RequireComponent(typeof(PlayerAudio))]
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerAudioData))]
public class PlayerController : MonoBehaviour
{
    public static PlayerState CurrentState = PlayerState.Default;

    public PlayerAudioData AudioData;

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

            if (PlayerInputManager.WasAttackPressed)
            {
                playerAttack.Attack();
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
    }

    void FixedUpdate()
    {
        // Move player rigidbody during FixedUpdate so that movement
        // is independent of framerate
        playerMovement.Move(movement);
    }
}
