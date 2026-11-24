using System;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static bool isGameFinished = false;
    public static void gameEnded()
    {
        isGameFinished = true;
    }
    void Start()
    {
        isGameFinished = false;
        if (PlayerPrefs.HasKey("SavedGame"))
        {
            string data = PlayerPrefs.GetString("SavedGame");

            int hashPos = data.IndexOf('#');
            int bangPos = data.IndexOf('!');
 
            string cpName = data.Substring(hashPos + 1, bangPos - hashPos - 1);
            string enemyData = data.Substring(bangPos + 1);

            int firstAlive = 0;
            int.TryParse(enemyData, out firstAlive);

            for (int i = 1; i < firstAlive; i++)
            {
                GameObject enemy = GameObject.Find("p" + i);
                if (enemy != null) Destroy(enemy);
            }

            int cpIndex;
            if (int.TryParse(cpName, out cpIndex))
            {
                if (cpIndex >= 4)
                {
                    WaveSystem wave = GameObject.FindFirstObjectByType<WaveSystem>();
                    if (wave != null) Destroy(wave.gameObject);
                }
            }

            GameObject cp = GameObject.Find(cpName);
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            if (cp != null && player != null)
            {
                player.transform.position = cp.transform.position;
                Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
                if (rb != null) rb.linearVelocity = Vector2.zero;
            }
        }
    }
}
