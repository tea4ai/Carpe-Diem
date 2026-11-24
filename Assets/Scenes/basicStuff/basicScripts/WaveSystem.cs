using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
public class WaveSystem : MonoBehaviour
{
    public static WaveSystem instance;

    [Header("Wave system components")]
    [SerializeField] private GameObject wall_prefab;
    [SerializeField] private GameObject enemySpawner_prefab;
    [SerializeField] private AudioClip dramaticMusic;
    
    [Header("UI Transition")]
    [SerializeField] private CanvasGroup waveHintGroup;
    [SerializeField] private float fadeDuration = 0.5f;
    private Coroutine fadeCoroutine;

    private Player player;
    private GameObject wallL;
    private GameObject wallR;
    private bool waveStarted = false;
    private int activeSpawners;

    private void Awake()
    {
        if (instance == null) instance = this;
            else Destroy(gameObject);
        if (waveHintGroup != null) waveHintGroup.alpha = 0;
    }

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        activeSpawners = 2;
    }

    public void StartWave()
    {
        if (waveStarted || player == null) return;
        waveStarted = true;
        StartFade(1f);
        if (CameraManager.instance != null)
        {
            CameraManager.instance.FreezeCamera(true);
        }

        Vector3 wallPosL = new Vector3(player.transform.position.x - 20, player.transform.position.y, 0);
        wallL = Instantiate(wall_prefab, wallPosL, Quaternion.identity);
        Vector3 wallPosR = new Vector3(player.transform.position.x + 20, player.transform.position.y, 0);
        wallR = Instantiate(wall_prefab, wallPosR, Quaternion.identity);

        if (MusicManager.instance != null)
        {
            MusicManager.instance.PlayMusicWithFade(dramaticMusic, 0.7f, 0.7f);
        }

        Vector3 spawnerPosL = wallPosL + new Vector3(4, 0, 0);
        Instantiate(enemySpawner_prefab, spawnerPosL, Quaternion.identity);

        Vector3 spawnerPosR = wallPosR + new Vector3(-4, 0, 0);
        Instantiate(enemySpawner_prefab, spawnerPosR, Quaternion.identity);

        Debug.Log("Wave started: Camera locked at current position.");
    }

    public void OnSpawnerFinished()
    {
        activeSpawners--;
        if (activeSpawners <= 0) EndWaveLogic();
    }

    private void EndWaveLogic()
    {
        waveStarted = false;
        StartFade(0f);
        if (CameraManager.instance != null)
        {
            CameraManager.instance.FreezeCamera(false);
        }

        Destroy(wallL);
        Destroy(wallR);

        if (MusicManager.instance != null)
            MusicManager.instance.PlayMusicWithFade(MusicManager.instance.mainMusic, 1f, 1.5f);

        Destroy(gameObject, 1.5f);
    }
    // Helper to handle starting/stopping the coroutine safely
    private void StartFade(float targetAlpha)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeHint(targetAlpha));
    }

    private IEnumerator FadeHint(float targetAlpha)
    {
        float startAlpha = waveHintGroup.alpha;
        float time = 0;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            waveHintGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }
        waveHintGroup.alpha = targetAlpha;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player p))
        {
            if (TryGetComponent<Collider2D>(out Collider2D col)) col.enabled = false;
            StartWave();
        }
    }
}