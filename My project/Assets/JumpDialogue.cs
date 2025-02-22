using UnityEngine;

public class JumpDialogue : MonoBehaviour
{
    [SerializeField] private DialogueExposition dialogueExposition;
    [SerializeField] private DialogueObject triggerDialogue; // Assign specific dialogue

    private bool dialoguePlayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player 1"))
        {
            if (dialogueExposition != null && triggerDialogue != null && !dialoguePlayed)
            {
                dialogueExposition.ShowDialogue(triggerDialogue);
                dialoguePlayed=true;
            }
        }
    }
}

