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

    // TODO: implement
    public override void OnPause()
    {

    }

    // TODO: implement
    public override void OnUnPause()
    {

    }
}
