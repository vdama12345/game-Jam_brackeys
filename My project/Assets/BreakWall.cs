using UnityEngine;

public class BreakWall : MonoBehaviour
{
    [SerializeField] private int wallHp = 100;
    [SerializeField] ParticleSystem particleSysten;
    [SerializeField] GameObject wall;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Break Wall"))
        {
            wallHp -= 10;
            particleSysten.Play();
        }

        if (wallHp <= 0)
        {
            wall.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //particleSysten.Stop();
    }
}
