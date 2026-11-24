
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            string sceneName = SceneManager.GetActiveScene().name;
            int firstAlive = 5;

            for (int i = 1; i <= 4; i++)
            {
                if (GameObject.Find("p" + i) != null)
                {
                    firstAlive = i;
                    break;
                }
            }

            // Save string: LevelName#CheckpointName!FirstAliveIndex
            string saveString = $"{sceneName}#{gameObject.name}!{firstAlive}";
            PlayerPrefs.SetString("SavedGame", saveString);
            PlayerPrefs.Save();

            GetComponent<Collider2D>().enabled = false;
        }
    }
}
