using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class NextSceneLoader : MonoBehaviour
{
    public float closeAnimationDuration = 1f; // Animation time
    public CanvasGroup fadeCanvasGroup;  // Assign UI Canvas Group for fading
    public RectTransform windowTransform; // Assign UI window object for scaling
    public Vector2 targetAnchor = new Vector2(1f, 0f); // Bottom-right anchor

    public void LoadNextScene()
    {
        StartCoroutine(CloseWindowEffect());
    }

    private IEnumerator CloseWindowEffect()
    {
        float elapsedTime = 0f;
        Vector3 originalScale = windowTransform.localScale;
        Vector2 originalAnchor = windowTransform.anchorMin; // Original anchor

        // Move anchor point to bottom-right
        windowTransform.anchorMin = targetAnchor;
        windowTransform.anchorMax = targetAnchor;
        windowTransform.pivot = targetAnchor;

        while (elapsedTime < closeAnimationDuration)
        {
            elapsedTime += Time.deltaTime;
            float progress = elapsedTime / closeAnimationDuration;

            // Fade out
            if (fadeCanvasGroup != null)
            {
                fadeCanvasGroup.alpha = 1f - progress;
            }

            // Shrink towards bottom-right
            windowTransform.localScale = Vector3.Lerp(originalScale, Vector3.zero, progress);

            yield return null;
        }

        // Load the next scene after animation
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
