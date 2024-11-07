public class BackgroundAudio : AudioController
{
    public BackgroundAudioData AudioData;

    void Awake() {
        // Instantiate audio source
        InitializeSound(AudioData.BackgroundMusic);
    }

    public void PlayBackgroundMusic()
    {
        AudioData.BackgroundMusic.Play();
    }

    public void OnPlayerStateChange(PlayerState playerState)
    {
        switch (playerState)
        {
            case PlayerState.Default:
                AudioData.BackgroundMusic.SetVolume(AudioData.BackgroundMusic.DefaultVolume);
                break;
            case PlayerState.Instrument:
                AudioData.BackgroundMusic.SetVolume(AudioData.BackgroundMusicInstrumentVolume);
                break;
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
