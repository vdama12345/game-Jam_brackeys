using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DialogueEntry
{
    [TextArea] public string dialogueText;
}

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] private DialogueEntry[] dialogueEntries;
    [SerializeField] private Response[] responses;

    public DialogueEntry[] DialogueEntries => dialogueEntries;

    public bool HasResponses => Responses != null && Responses.Length > 0;

    public Response[] Responses => responses;
}

