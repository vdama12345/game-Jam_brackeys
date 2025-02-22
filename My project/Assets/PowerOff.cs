using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PowerOff : MonoBehaviour
{

    public void powerOff()
    {
        SceneManager.LoadScene(4);
    }
}
