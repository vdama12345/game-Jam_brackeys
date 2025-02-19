using UnityEngine;
using System.Collections;
using TMPro;

public class BreakWall : MonoBehaviour
{
    [SerializeField] private int wallHp = 100;
    [SerializeField] private ParticleSystem particleSysten;
    [SerializeField] private GameObject wall;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 0.2f;
    [SerializeField] private AudioSource smash;
    [SerializeField] private DialogueObject wallDialogue;
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueExposition dialogueExposition;

    private int dialogueIndex = 0;
    private Coroutine currentDialogueCoroutine;
    private bool isDialogueRunning = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Break Wall"))
        {
            wallHp -= 10;
            particleSysten.Play();
            StartCoroutine(ScreenShake());
            smash.Play();
            smash.loop = false;

            if (wallDialogue != null && dialogueIndex < wallDialogue.DialogueEntries.Length)
            {
                if (!isDialogueRunning)
                {
                    currentDialogueCoroutine = StartCoroutine(ShowDialogueCoroutine(wallDialogue.DialogueEntries[dialogueIndex]));
                    dialogueIndex++;
                }
            }
        }

        if (wallHp <= 0)
        {
            wall.SetActive(false);
        }
    }

    private IEnumerator ShowDialogueCoroutine(DialogueEntry entry)
    {
        isDialogueRunning = true;
        DialogueObject tempDialogue = ScriptableObject.CreateInstance<DialogueObject>();
        tempDialogue.GetType().GetField("dialogueEntries", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(tempDialogue, new DialogueEntry[] { entry });
        dialogueExposition.ShowDialogue(tempDialogue);
        
        yield return new WaitForSeconds(entry.waitTime);

        isDialogueRunning = false;
        currentDialogueCoroutine = null;
    }

    private IEnumerator ScreenShake()
    {
        Vector3 originalPosition = Camera.main.transform.position;
        float elapsedTime = 0f;
        while (elapsedTime < shakeDuration)
        {
            float xOffset = Random.Range(-shakeMagnitude, shakeMagnitude);
            float yOffset = Random.Range(-shakeMagnitude, shakeMagnitude);

            Camera.main.transform.position = originalPosition + new Vector3(xOffset, yOffset, 0);
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        Camera.main.transform.position = originalPosition;
    }
}
