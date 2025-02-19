using UnityEngine;
using System.Collections;

public class BreakWall : MonoBehaviour
{
    [SerializeField] private int wallHp = 100;
    [SerializeField] private ParticleSystem particleSysten;
    [SerializeField] private GameObject wall;
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 0.2f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Break Wall"))
        {
            wallHp -= 10;
            particleSysten.Play();
            StartCoroutine(ScreenShake());
        }

        if (wallHp <= 0)
        {
            wall.SetActive(false);
        }
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

