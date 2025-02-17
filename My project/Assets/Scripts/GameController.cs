using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    private Vector2 startPos;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        startPos = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Death Plane"))
        {
            Die();
        }

        if (collision.CompareTag("x_plane"))
        {
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("x_plane"))
        {
            canvas.SetActive(false);
        }
    }

    void Die()
    {
        StartCoroutine(Respawn(0.5f));
    }

    IEnumerator Respawn(float duration)
    {
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(duration);
        transform.position = startPos;
        spriteRenderer.enabled = true;
    }
}
