using UnityEngine;

public class PlayerAudio : AudioController
{
    public PlayerAudioData AudioData;

    // Store last played clip index to avoid playing it twice in a row
    private int lastWalkingClipIndex = 0;

    // TODO: refactor to use less audio sources and just change clips
    void Awake() {
        // TODO: improve this
        // Instantiate audio sources
        foreach (Sound sound in AudioData.Footsteps)
        {
            InitializeSound(sound);
        }
        foreach (Sound sound in AudioData.AttackChords)
        {
            InitializeSound(sound);
        }
        InitializeSound(AudioData.NoteA);
        InitializeSound(AudioData.NoteB);
        InitializeSound(AudioData.NoteC);
        InitializeSound(AudioData.NoteE);
        InitializeSound(AudioData.NoteF);
        InitializeSound(AudioData.Melody1);
        InitializeSound(AudioData.Melody2);
    }

    public void PlayWalkingAudio(Vector2 movement)
    {
        // Don't attempt to play walking audio if not moving
        // or if a walking clip is currently playing
        if(
            movement.sqrMagnitude == 0 ||
            AudioData.Footsteps[lastWalkingClipIndex].IsPlaying
        )
        {
            return;
        }

        // Prevent playing the same clip twice in a row
        int clipIndex = lastWalkingClipIndex;
        while (clipIndex == lastWalkingClipIndex)
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
        // Set lastWalkingClipIndex to the index just used
        lastWalkingClipIndex = clipIndex;
    }

    public void PlayNote(string noteName)
    {
        // Determine which Sound to play
        Sound sound;
        switch (noteName)
        {
            case MelodyData.NoteA:
                sound = AudioData.NoteA;
                break;
            case MelodyData.NoteB:
                sound = AudioData.NoteB;
                break;
            case MelodyData.NoteC:
                sound = AudioData.NoteC;
                break;
            case MelodyData.NoteE:
                sound = AudioData.NoteE;
                break;
            case MelodyData.NoteF:
                sound = AudioData.NoteF;
                break;
            default:
                return;
        }
        // If a valid note name was provided, play the related Sound
        if (sound != null)
        {
            sound.Play();
        }
    }

    public void PlayMelody(string melody)
    {
        switch (melody)
        {
            case MelodyData.Melody1:
                AudioData.Melody1.Play();
                break;
            case MelodyData.Melody2:
                AudioData.Melody2.Play();
                break;
            default:
                break;
        }
    }

    public void PlayAttackChord()
    {
        // Play a random attack chord clip
        int clipIndex = Random.Range(0, AudioData.AttackChords.Length);
        AudioData.AttackChords[clipIndex].Play();
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
