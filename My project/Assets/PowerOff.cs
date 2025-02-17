using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PowerOff : MonoBehaviour
{

    public void powerOff()
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
