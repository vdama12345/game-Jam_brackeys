using UnityEngine;
using UnityEngine.SceneManagement;

public class RestarGame : MonoBehaviour
{
    public KeyCode restarKey;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(restarKey))
        {
            SceneManager.LoadScene(0);
            CollectorScript.collectedItem = "";
        }
        
    }
}
