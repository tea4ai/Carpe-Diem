using System.Collections;
using UnityEngine;

public class TriggerVisuals : MonoBehaviour
{
    [SerializeField] private CanvasGroup tutorialUI;
    [SerializeField] private float fadeSpeed = 5f;

    private bool isPlayerInside = false;

    void Update()
    {
        float targetAlpha = isPlayerInside ? 1f : 0f;
        tutorialUI.alpha = Mathf.MoveTowards(tutorialUI.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            Debug.Log("In bounds:" + gameObject.name);
            isPlayerInside = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            isPlayerInside = false;
        }
    }
}