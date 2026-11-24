using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject _enemySpawner;
    [SerializeField] private Transform[] positions;
    [SerializeField] private float coolDown;
    [SerializeField] private int countToGo;
    [Space]
    [SerializeField] private float decreaseCD;
    [SerializeField] private float maxCD;
    private float timer;
    private Transform player;
    
    private Vector3 spawnPos;
    private int posIndex;

    private void Awake()
    {
        player = FindFirstObjectByType<Player>().transform;

        positions = GetComponentsInChildren<Transform>();
    }
    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0 && countToGo > 0)
        {
            CreateNewEnemy();
            countToGo--;
            timer = coolDown;
            coolDown = Mathf.Max(maxCD, coolDown - decreaseCD);
        }
        if (countToGo <= 0)
        {
            if (timer <= -0.5f)
            {
                var allActiveEnemies = FindObjectsByType<Enemy>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

                int waveEnemyCount = 0;
                foreach (var enemy in allActiveEnemies)
                {
                    if (!enemy.CompareTag("Patroller"))
                    {
                        waveEnemyCount++;
                    }
                }

                if (waveEnemyCount == 0)
                {
                    if (WaveSystem.instance != null)
                        WaveSystem.instance.OnSpawnerFinished();

                    Destroy(gameObject);
                }

                timer = 0;
            }
        }
    }

    private void CreateNewEnemy()
    {
        if (positions == null || positions.Length <= 1)
        {
            SpawnAtPosition(transform.position);
            return;
        }
            posIndex = Random.Range(1, positions.Length);
            SpawnAtPosition(positions[posIndex].position);
    }
    private void SpawnAtPosition(Vector3 position)
    {
        GameObject newEnemy = Instantiate(_enemySpawner, position, Quaternion.identity);

        newEnemy.SetActive(true);
        bool onTheRight = newEnemy.transform.position.x > player.position.x;
        if (onTheRight)
            newEnemy.GetComponent<Enemy>().Flip();


    }
}
