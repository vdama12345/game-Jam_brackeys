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
    [SerializeField] private GameObject[] panels;

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
        int panelIndex = 0;

        // Show the default panel immediately
        if (panels.Length > 0)
        {
            panels[panelIndex].SetActive(true);
            panelIndex++;
        }

        foreach (var entry in dialogueObject.DialogueEntries)
        {
            string dialogue = entry.dialogueText;
            yield return RunTypingEffect(dialogue);

            // Show the next panel if available
            if (panelIndex < panels.Length)
            {
                panels[panelIndex].SetActive(true);
                panelIndex++;
            }

            yield return new WaitForSeconds(2f); // Automatically wait before continuing

            textLabel.text = string.Empty; // Clear text before implementing wait time
            yield return new WaitForSeconds(entry.waitTime); // Use waitTime from DialogueEntry
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
        typewriterEffect.Run(dialogue, textLabel);

        while (typewriterEffect.isRunning)
        {
            yield return null;
        }
    }

    public void CloseDialogueBox()
    {
        IsOpen = false;
        dialogueBox.SetActive(false);
        textLabel.text = string.Empty;
    }
}

