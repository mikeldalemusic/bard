using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static Vector2 Movement;

    private InputAction moveAction;

    void Awake()
    {
        PlayerInput playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    void Update()
    {
        // Input composite mode should be set to "digital" to prevent diagonal
        // movement magnitude from being less than 1
        Movement = moveAction.ReadValue<Vector2>();
    }
}
