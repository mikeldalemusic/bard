public class BackgroundAudio : AudioController
{
    public BackgroundAudioData AudioData;

    void Awake() {
        // Instantiate audio source
        InitializeSound(AudioData.BackgroundMusic);
        // Subscribe to custom event
        CustomEvents.OnPlayerStateChange.AddListener(OnPlayerStateChange);
    }

    void OnDestroy()
    {
        // Remove listener on destroy to prevent memory leaks
        CustomEvents.OnPlayerStateChange.RemoveListener(OnPlayerStateChange);
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
            case PlayerState.InstrumentMelody:
                AudioData.BackgroundMusic.SetVolume(AudioData.BackgroundMusicInstrumentMelodyVolume);
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
