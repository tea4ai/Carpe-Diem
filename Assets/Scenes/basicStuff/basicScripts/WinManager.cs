using UnityEngine;
using TMPro;
using System.Collections;

public class WinTrigger : MonoBehaviour
{
    [Header("UI Settings")]
    public CanvasGroup warningGroup;
    public TextMeshProUGUI warningText;
    
    [Header("Follow Settings")]
    public Transform playerTransform;     
    public Vector3 offset = new Vector3(4, 9f, 0); 

    private bool isShowing = false;

    private void Start()
    {
        if (warningGroup) warningGroup.alpha = 0;
    }

    private void Update()
    {
        if (isShowing && playerTransform != null)
        {
            warningGroup.transform.position = playerTransform.position + offset;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            int count = GameObject.FindGameObjectsWithTag("Patroller").Length;

            if (count == 0)
            {
                LevelManager.gameEnded();

                if (WinUI.instance != null)
                {
                    WinUI.instance.EnableWin();
                }
            }
            else if (!isShowing)
            {
                if(playerTransform == null) playerTransform = collision.transform;
                StartCoroutine(ShowWarning(count));
            }
        }
    }

    IEnumerator ShowWarning(int count)
    {
        isShowing = true;
        warningText.text = count.ToString();

        yield return StartCoroutine(FadeWarning(0, 1, 1f));
        yield return new WaitForSeconds(5f);
        yield return StartCoroutine(FadeWarning(1, 0, 1f));

        isShowing = false;
    }

    IEnumerator FadeWarning(float startAlpha, float endAlpha, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            warningGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            yield return null;
        }
        warningGroup.alpha = endAlpha;
    }
}