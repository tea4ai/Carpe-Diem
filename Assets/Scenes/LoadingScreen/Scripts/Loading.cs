using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{

    [SerializeField] private Slider progressBar;
    [SerializeField] private Text loadingText;
    [SerializeField] private float fakeLoadDelay = 5f;
    [SerializeField]private RectTransform loader;
    public float speed = 200f;

    void Update()
    {
        loader.Rotate(0f, 0f, speed * Time.deltaTime);
    }
    void Start()
    {
       StartCoroutine (LoadMainSceneAsync());
    }


    IEnumerator LoadMainSceneAsync()
    {
        yield return new WaitForSeconds(fakeLoadDelay);

        AsyncOperation operation = SceneManager.LoadSceneAsync("mainMenu");
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;
            loadingText.text = $"Loading...{Mathf.Floor(progress * 100)}%";
            yield return null;
        }

        progressBar.value = 1f;
        loadingText.text = "Loading...100%";

        yield return new WaitForSeconds(0.5f);

        operation.allowSceneActivation = true;
    }

}