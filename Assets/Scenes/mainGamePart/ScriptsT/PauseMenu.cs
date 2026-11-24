using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject Container;
    [SerializeField] private GameObject player;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (LevelManager.isGameFinished) return;

            if (Container.activeSelf)
            {
                Container.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                Container.SetActive(true);
                Time.timeScale = 0;
            }
        }
    }
    public void ResumeBtn()
    {
        Container.SetActive(false);
        Time.timeScale = 1;
    }
    public void MainMenuBtn()
    {
        Time.timeScale = 1;
        UnityEngine.SceneManagement.SceneManager.LoadScene("mainMenu");
    }
}
