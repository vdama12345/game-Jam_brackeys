using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Response
{
    [SerializeField] private string responseText;
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private UnityEvent postDialogueEvent = new();  // Optional script

    public string ResponseText => responseText;
    public DialogueObject DialogueObject => dialogueObject;
    public void InvokePostDialogueEvent()
    {
        postDialogueEvent?.Invoke();
    }
}