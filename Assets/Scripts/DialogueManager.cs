using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    private static Dialogue currentDialogue;

    void Awake() {
        // Subscribe to custom event
        CustomEvents.OnDialogueStart.AddListener(OnDialogueStart);
    }

    void OnDestroy()
    {
        // Remove listener on destroy to prevent memory leaks
        CustomEvents.OnDialogueStart.RemoveListener(OnDialogueStart);
    }

    private void OnDialogueStart(Dialogue dialogue)
    {
        currentDialogue = dialogue;
        Debug.Log("starting dialogue");
    }

    public static void AdvanceCurrentDialogue()
    {
        // TODO: @Andrew - check if should end current dialogue
        // (is currently set to `true` for testing)
        bool shouldEndDialogue = true;
        if (shouldEndDialogue)
        {
            CustomEvents.OnDialogueEnd?.Invoke(currentDialogue);
            currentDialogue = null;
        }
        else
        {
            // TODO: @Andrew - advance current dialogue
        }
    }
}
