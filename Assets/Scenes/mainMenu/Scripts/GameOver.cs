using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameOver : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private Image backGround;
    [SerializeField] private TMP_Text deadText;
    [SerializeField] private float alphaFloat = 0.5f;
    private bool active = false;
    public static GameOver instance;

    private void Start()
    {
        if( instance == null)
        {
            instance = this;
        }
    }

    public void EnableGameOver()
    {
        LevelManager.gameEnded();
        container.SetActive(true);
        active = true;
        
    }
    private void Update()
    {
        Color color = backGround.color;
        if (active)
        {

            if (color.a >= 1f)
            {
                active = false;
                StartCoroutine(waitBeforeRestart());
                backGround.color = Color.black;
                return;
            }
            else
            {
                color.a += alphaFloat * Time.deltaTime;
                backGround.color = color;
            }
        }
        else
            return;
    }
    private IEnumerator waitBeforeRestart()
    {
        yield return new WaitForSecondsRealtime(1.5f);
        RestartLevel();
    }
    public void RestartLevel()
    {
        string savedData = PlayerPrefs.GetString("SavedGame", "TutorialLVL#None!0");
        if (string.IsNullOrEmpty(savedData))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        int hashPos = savedData.IndexOf('#');
        int bangPos = savedData.IndexOf('!');
        string checkpointName = savedData.Substring(hashPos + 1, bangPos - hashPos - 1);

        GameObject checkpoint = GameObject.Find(checkpointName);
        Player playerScript = FindFirstObjectByType<Player>();

        if (checkpoint != null && playerScript != null)
        {
            Rigidbody2D rb = playerScript.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.simulated = false;
                rb.linearVelocity = Vector2.zero;
            }

            playerScript.transform.position = checkpoint.transform.position;
            if (rb != null) rb.position = checkpoint.transform.position;

            playerScript.ResetPlayer();
            if (rb != null) rb.simulated = true;

            container.SetActive(false);
            active = false;
            Color c = backGround.color;
            c.a = 0;
            backGround.color = c;
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

}
