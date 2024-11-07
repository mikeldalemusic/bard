using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAudioData", menuName = "Scriptable Objects/PlayerAudioData")]
public class PlayerAudioData : ScriptableObject
{
    [Header("Footsteps")]
    public Sound[] Footsteps;
    [Range (0f, 1f)]
    public float MaxFootstepPitchVariation;
    [Range (0f, 1f)]
    public float MaxFootstepVolumeVariation;

    [Header("Instrument")]
    public Sound NoteA;
    public Sound NoteB;
    public Sound NoteC;
    public Sound NoteE;
    public Sound NoteF;
    [Range (0f, 0.5f)]
    public float NoteCooldownTime;
}
