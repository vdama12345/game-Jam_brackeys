using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class ResponseHandlerExposition : MonoBehaviour
{
    [SerializeField] private GameObject option1;
    [SerializeField] private GameObject option2;
    [SerializeField] private GameObject responseBox1;
    [SerializeField] private GameObject responseBox2;

    private DialogueExposition dialogueUI;
    private ResponseEvent[] responseEvents;

    private List<GameObject> tempResponseButtons = new List<GameObject>();

    private void Start()
    {
        dialogueUI = GetComponent<DialogueExposition>();
    }

    public void AddResponseEvents(ResponseEvent[] responseEvents)
    {
        this.responseEvents = responseEvents;
    }

    public void ShowResponses(Response[] responses)
    {
        responseBox1.SetActive(true);
        responseBox2.SetActive(true);

        option1.GetComponent<TMP_Text>().text = responses[0].ResponseText;
        option1.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(responses[0], 0));

        option2.GetComponent<TMP_Text>().text = responses[1].ResponseText;
        option2.GetComponent<Button>().onClick.AddListener(() => OnPickedResponse(responses[1], 1));
    }

    private void OnPickedResponse(Response response, int responseIndex)
    {
        responseBox1.gameObject.SetActive(false);
        responseBox2.gameObject.SetActive(false);

        if (responseEvents != null && responseIndex <= responseEvents.Length)
        {
            responseEvents[responseIndex].OnPickedResponse?.Invoke();
        }

        dialogueUI.ShowDialogue(response.DialogueObject);

        response.InvokePostDialogueEvent();
    }
}

