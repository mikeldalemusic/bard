using UnityEngine;

public class PlayerAudio : AudioController
{
    public PlayerAudioData AudioData;

    // Store last played clip index to avoid playing it twice in a row
    private int lastClipIndex = 0;
    private float lastNotePlayedTime;

    void Awake() {
        // Instantiate audio sources
        foreach (Sound sound in AudioData.Footsteps)
        {
            InitializeSound(sound);
        }
        InitializeSound(AudioData.NoteA);
        InitializeSound(AudioData.NoteB);
        InitializeSound(AudioData.NoteC);
        InitializeSound(AudioData.NoteE);
        InitializeSound(AudioData.NoteF);
        // Initialize lastNotePlayedTime so that a note can be played immediately
        lastNotePlayedTime = -AudioData.NoteCooldownTime;
    }

    // Returns true if noteCooldownTime time has passed since lastNotePlayedTime
    private bool CanPlayNote()
    {
        return Time.time >= lastNotePlayedTime + AudioData.NoteCooldownTime;
    }

    public void PlayWalkingAudio(Vector2 movement)
    {
        // Don't attempt to play walking audio if not moving
        // or if a walking clip is currently playing
        if(
            movement.sqrMagnitude == 0 ||
            AudioData.Footsteps[lastClipIndex].IsPlaying
        )
        {
            return;
        }

        // Prevent playing the same clip twice in a row
        int clipIndex = lastClipIndex;
        while (clipIndex == lastClipIndex)
        {
            clipIndex = Random.Range(0, AudioData.Footsteps.Length);
        }
        Sound sound = AudioData.Footsteps[clipIndex];
        // Randomize pitch in either direction of the default pitch
        float pitch = Random.Range(sound.DefaultPitch - AudioData.MaxFootstepPitchVariation, sound.DefaultPitch + AudioData.MaxFootstepPitchVariation);
        // Randomize volume, but don't exceed the default volume
        float volume = Random.Range(sound.DefaultVolume - AudioData.MaxFootstepVolumeVariation, sound.DefaultVolume);
        // Play the sound with new pitch and volume
        AudioData.Footsteps[clipIndex].Play(pitch, volume);
        // Set lastClipIndex to the index just used
        lastClipIndex = clipIndex;
    }

    public void PlayNote(string noteName)
    {
        // Return early if player can't currently play a note
        if (!CanPlayNote())
        {
            return;
        }
        // Determine which Sound to play
        Sound sound;
        switch (noteName)
        {
            case Actions.NoteA:
                sound = AudioData.NoteA;
                break;
            case Actions.NoteB:
                sound = AudioData.NoteB;
                break;
            case Actions.NoteC:
                sound = AudioData.NoteC;
                break;
            case Actions.NoteE:
                sound = AudioData.NoteE;
                break;
            case Actions.NoteF:
                sound = AudioData.NoteF;
                break;
            default:
                return;
        }
        // If a valid note name was provided, play the related Sound
        if (sound != null)
        {
            sound.Play();
            lastNotePlayedTime = Time.time;
        }
    }

    // TODO: implement
    public override void OnPause()
    {

    }

    // TODO: implement
    public override void OnUnPause()
    {

    }
}
