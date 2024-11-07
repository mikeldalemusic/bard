using UnityEngine;

public class PlayerAudio : AudioController
{
    public PlayerAudioData AudioData;

    // Store last played clip index to avoid playing it twice in a row
    private int lastClipIndex = 0;

    void Awake() {
        // Instantiate audio sources
        foreach (Sound sound in AudioData.Footsteps)
        {
            InitializeSound(sound);
        }
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

    // TODO: implement
    public override void OnPause()
    {

    }

    // TODO: implement
    public override void OnUnPause()
    {

    }
}
