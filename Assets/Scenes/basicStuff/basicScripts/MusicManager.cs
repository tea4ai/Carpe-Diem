using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    public AudioClip mainMusic;
    public static MusicManager instance;
    private Coroutine fadeCoroutine;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            if (musicSource != null)
            {
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }
        }
        else Destroy(gameObject);
    }
    public void PlayMusicWithFade(AudioClip newClip, float targetVolume = 1f, float duration = 1.2f)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeMusicRoutine(newClip, targetVolume, duration));
    }

    private IEnumerator FadeMusicRoutine(AudioClip newClip, float targetVolume, float duration)
    {
        float startVolume = musicSource.volume;

        if (musicSource.isPlaying)
        {
            while (musicSource.volume > 0)
            {
                musicSource.volume -= startVolume * (Time.deltaTime / (duration / 2));
                yield return null;
            }
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        while (musicSource.volume < targetVolume)
        {
            musicSource.volume += targetVolume * (Time.deltaTime / (duration / 2));
            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}
