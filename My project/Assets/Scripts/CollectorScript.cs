using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectorScript : MonoBehaviour
{
    [SerializeField] private GameObject collectable; // Assigned in Inspector
    [HideInInspector] public static string collectedItem = "";
    private GameObject currentCollectable; // Store the collided object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(collectable.tag)) // Check if the collided object matches the Inspector-set collectable tag
        {
            currentCollectable = collision.gameObject; // Store the object
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == currentCollectable)
        {
            currentCollectable = null; // Reset when leaving
        }
    }

    private void Update()
    {
        if (currentCollectable != null && Input.GetKeyDown(KeyCode.E))
        {

            if (currentCollectable.CompareTag("System 32")) // Check if it's "System 32"
            {
                collectedItem = "System32";
                currentCollectable.SetActive(false);
            }


            //if (currentCollectable.CompareTag("Ball Game")) // Check if it's "System 32"
            //{
            //    int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            //    if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            //    {
            //        SceneManager.LoadScene(nextSceneIndex);
            //    }
            //    else
            //    {
            //        Debug.Log("No more scenes to load. End of build order.");
            //    }
            //}

            if (currentCollectable.CompareTag("Game Name")) // Check if it's "System 32"
            {
                SceneManager.LoadScene(0);
            }

            currentCollectable = null; // Reset after collection
        }
    }
}
