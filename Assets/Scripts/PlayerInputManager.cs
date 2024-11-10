using UnityEngine;
using UnityEngine.InputSystem;

static class ActionMaps
{
    // PlayerInput action maps
    public const string Player = "Player";
    public const string Instrument = "Instrument";
}

static class Actions
{
    // Player actions
    public const string Move = "Move";
    public const string Attack = "Attack";
    // Instrument actions
    public const string ToggleInstrument = "ToggleInstrument";
    public const string NoteA = "NoteA";
    public const string NoteB = "NoteB";
    public const string NoteC = "NoteC";
    public const string NoteE = "NoteE";
    public const string NoteF = "NoteF";
}

[RequireComponent(typeof(InputActionAsset))]
public class PlayerInputManager : MonoBehaviour
{
    // Assigned in editor
    public InputActionAsset InputActionAsset;

    // Public properties for reading captured input
    public static Vector2 Movement;
    public static bool WasAttackPressed;
    public static bool WasToggleInstrumentPressed;
    public static string NotePressed;

    // Player actions
    private InputAction moveAction;
    private InputAction attackAction;
    // Instrument actions
    private InputAction toggleInstrumentAction;
    private InputAction noteAAction;
    private InputAction noteBAction;
    private InputAction noteCAction;
    private InputAction noteEAction;
    private InputAction noteFAction;

    void Awake()
    {
        // Player actions
        InputActionMap playerActionMap = InputActionAsset.FindActionMap(ActionMaps.Player);
        moveAction = playerActionMap.FindAction(Actions.Move);
        attackAction = playerActionMap.FindAction(Actions.Attack);
        // Instrument actions
        InputActionMap instrumentActionMap = InputActionAsset.FindActionMap(ActionMaps.Instrument);
        toggleInstrumentAction = instrumentActionMap.FindAction(Actions.ToggleInstrument);
        noteAAction = instrumentActionMap.FindAction(Actions.NoteA);
        noteBAction = instrumentActionMap.FindAction(Actions.NoteB);
        noteCAction = instrumentActionMap.FindAction(Actions.NoteC);
        noteEAction = instrumentActionMap.FindAction(Actions.NoteE);
        noteFAction = instrumentActionMap.FindAction(Actions.NoteF);
    }

    private void OnEnable()
    {
        // Enable actions to ensure they can read values
        moveAction.Enable();
        toggleInstrumentAction.Enable();
        noteAAction.Enable();
        noteBAction.Enable();
        noteCAction.Enable();
        noteEAction.Enable();
        noteFAction.Enable();
    }

    private void OnDisable()
    {
        // Disable actions to prevent unnecessary updates when the gameObject is inactive
        moveAction.Disable();
        toggleInstrumentAction.Disable();
        noteAAction.Disable();
        noteBAction.Disable();
        noteCAction.Disable();
        noteEAction.Disable();
        noteFAction.Disable();
    }

    void HandleNotePress()
    {
        if (noteAAction.WasPressedThisFrame())
        {
            NotePressed = Actions.NoteA;
        }
        else if (noteBAction.WasPressedThisFrame())
        {
            NotePressed = Actions.NoteB;
        }
        else if (noteCAction.WasPressedThisFrame())
        {
            NotePressed = Actions.NoteC;
        }
        else if (noteEAction.WasPressedThisFrame())
        {
            NotePressed = Actions.NoteE;
        }
        else if (noteFAction.WasPressedThisFrame())
        {
            NotePressed = Actions.NoteF;
        } else {
            NotePressed = null;
        }
    }

    void Update()
    {
        // Move action composite mode should be set to "digital" to prevent diagonal
        // movement magnitude from being less than 1
        Movement = moveAction.ReadValue<Vector2>();
        WasAttackPressed = attackAction.WasPressedThisFrame();
        WasToggleInstrumentPressed = toggleInstrumentAction.WasPressedThisFrame();
        HandleNotePress();
    }
}
