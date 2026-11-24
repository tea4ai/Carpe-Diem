using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class WinUI : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Image backGround;
    [SerializeField] private TMP_Text winText;
    [SerializeField] private float fadeDuration = 2.0f;

    public static WinUI instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        container.SetActive(false);
    }
    public void EnableWin()
    {
        Time.timeScale = 0f;
        container.SetActive(true);
        StartCoroutine(FadeInSequence());
    }
    private IEnumerator FadeInSequence()
{
    float elapsed = 0;
    Color color = backGround.color;
    color.a = 0;
    backGround.color = color;
    while (elapsed < fadeDuration)
    {
        elapsed += Time.unscaledDeltaTime; 
        float newAlpha = Mathf.Clamp01(elapsed / fadeDuration);
        color.a = newAlpha;
        backGround.color = color;
        yield return null;
    }
    color.a = 1f;
    backGround.color = Color.black;
    yield return new WaitForSecondsRealtime(1.5f);
    RestartLevel();
}
    public void RestartLevel()
    {
        PlayerPrefs.SetString("LastCheckpoint", "");
        PlayerPrefs.SetString("SavedGame", "    ");
        Time.timeScale = 1f;
        SceneManager.LoadScene("mainMenu");
    }
}