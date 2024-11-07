using UnityEngine;

[CreateAssetMenu(fileName = "PlayerAudioData", menuName = "Scriptable Objects/PlayerAudioData")]
public class PlayerAudioData : ScriptableObject
{
    public Sound[] Footsteps;
    [Range (0f, 1f)]
    public float MaxFootstepPitchVariation;
    [Range (0f, 1f)]
    public float MaxFootstepVolumeVariation;
}
