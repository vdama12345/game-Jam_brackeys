using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class DialogueExposition : MonoBehaviour
{

    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject exposition;

    public bool IsOpen { get; private set; }

    private ResponseHandlerExposition responseHandler;
    private TypewriterEffect typewriterEffect;

    private void Start()
    {
        typewriterEffect = GetComponent<TypewriterEffect>();
        responseHandler = GetComponent<ResponseHandlerExposition>();
        CloseDialogueBox();
        ShowDialogue(exposition);
    }
    public void ShowDialogue(DialogueObject dialogueObject)
    {
        Debug.LogError("1");
        IsOpen = true;
        dialogueBox.SetActive(true);
        StartCoroutine(StepThroughDialogue(dialogueObject));
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        responseHandler.AddResponseEvents(responseEvents);
    }
    private IEnumerator StepThroughDialogue(DialogueObject dialogueObject)
    {
        Debug.LogError("2");
        foreach (var entry in dialogueObject.DialogueEntries)
        {
            string dialogue = entry.dialogueText;

            yield return RunTypingEffect(dialogue);
        }

        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogueBox();
        }
    }

    private IEnumerator RunTypingEffect(string dialogue)
    {
        Debug.LogError("3");
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.isRunning)
        {
            yield return null;
        }
    }

    private void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;


    }
}