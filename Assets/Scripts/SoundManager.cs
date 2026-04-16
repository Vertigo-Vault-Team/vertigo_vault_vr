using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Audio Routing")]
    [SerializeField] private AudioSource bgmSourceA;
    [SerializeField] private AudioSource bgmSourceB;
    [SerializeField] private AudioSource sfxSource;

    private bool useAAsCurrent = true;
    private Coroutine bgmFadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EnsureAudioSources();
        ConfigureBgmSource(bgmSourceA);
        ConfigureBgmSource(bgmSourceB);
        ConfigureSfxSource(sfxSource);
    }

    public void PlayBgmImmediate(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;

        AudioSource current = useAAsCurrent ? bgmSourceA : bgmSourceB;
        AudioSource next = useAAsCurrent ? bgmSourceB : bgmSourceA;

        if (bgmFadeRoutine != null)
            StopCoroutine(bgmFadeRoutine);

        next.Stop();
        next.clip = null;
        next.volume = 0f;

        current.clip = clip;
        current.volume = Mathf.Clamp01(volume);
        if (!current.isPlaying)
            current.Play();
    }

    public void CrossfadeBgm(AudioClip targetClip, float fadeDuration = 1.5f, float targetVolume = 1f)
    {
        if (targetClip == null)
            return;

        AudioSource current = useAAsCurrent ? bgmSourceA : bgmSourceB;
        AudioSource next = useAAsCurrent ? bgmSourceB : bgmSourceA;

        if (current.clip == targetClip && current.isPlaying)
            return;

        if (next.clip != targetClip)
            next.clip = targetClip;

        if (!next.isPlaying)
            next.Play();

        if (bgmFadeRoutine != null)
            StopCoroutine(bgmFadeRoutine);

        bgmFadeRoutine = StartCoroutine(CrossfadeRoutine(current, next, Mathf.Max(0.01f, fadeDuration), Mathf.Clamp01(targetVolume)));
        useAAsCurrent = !useAAsCurrent;
    }

    public void PlaySfx(AudioClip clip, float volume = 1f)
    {
        if (clip == null)
            return;

        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    private IEnumerator CrossfadeRoutine(AudioSource from, AudioSource to, float duration, float targetVolume)
    {
        float time = 0f;
        float fromStart = from.volume;
        float toStart = to.volume;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Clamp01(time / duration);
            from.volume = Mathf.Lerp(fromStart, 0f, t);
            to.volume = Mathf.Lerp(toStart, targetVolume, t);
            yield return null;
        }

        from.volume = 0f;
        to.volume = targetVolume;
    }

    private void EnsureAudioSources()
    {
        if (bgmSourceA == null)
            bgmSourceA = gameObject.AddComponent<AudioSource>();

        if (bgmSourceB == null)
            bgmSourceB = gameObject.AddComponent<AudioSource>();

        if (sfxSource == null)
            sfxSource = gameObject.AddComponent<AudioSource>();
    }

    private static void ConfigureBgmSource(AudioSource source)
    {
        source.loop = true;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.volume = 0f;
    }

    private static void ConfigureSfxSource(AudioSource source)
    {
        source.loop = false;
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.volume = 1f;
    }
}
