using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerAnimation playerAnimation;
    private PlayerMovement playerMovement;
    private Vector2 movement;

    void Awake()
    {
        playerAnimation = GetComponent<PlayerAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        // TODO: handle pause state
        movement = PlayerInputManager.Movement;
        playerAnimation.SetAnimationParams(movement);
    }

    void FixedUpdate()
    {
        // Move player rigidbody during FixedUpdate so that movement
        // is independent of framerate
        playerMovement.Move(movement);
    }
}
