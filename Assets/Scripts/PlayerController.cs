using UnityEngine;

[RequireComponent(typeof(PlayerAnimation))]
[RequireComponent(typeof(PlayerAudio))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerController : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private PlayerAudio playerAudio;
    private PlayerMovement playerMovement;
    private Vector2 movement;

    void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAudio = GetComponent<PlayerAudio>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // TODO: handle pause state
        movement = PlayerInputManager.Movement;
        playerAnimation.SetAnimationParams(movement);
        playerAudio.PlayWalkingAudio(movement);
    }

    void FixedUpdate()
    {
        // Move player rigidbody during FixedUpdate so that movement
        // is independent of framerate
        playerMovement.Move(movement);
    }
}
