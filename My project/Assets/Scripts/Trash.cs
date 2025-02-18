using UnityEngine;
using UnityEngine.SceneManagement;

public class Trash : MonoBehaviour
{
    private bool isPlayerInTrash = false; // Track if player is inside trigger

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered Trigger");
        if (collision.CompareTag("Trash"))
        {
            isPlayerInTrash = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Exited Trigger");
        if (collision.CompareTag("Trash"))
        {
            isPlayerInTrash = false;
        }
    }

    private void Update()
    {
        if (isPlayerInTrash && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Updated");
            if (CollectorScript.collectedItem == "System32")
            {
                int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
                if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextSceneIndex);
                }
                else
                {
                    Debug.Log("No more scenes to load. End of build order.");
                }
            }
        }
    }
}
