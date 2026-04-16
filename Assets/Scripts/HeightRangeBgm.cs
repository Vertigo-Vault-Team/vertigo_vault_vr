using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HeightRangeBgm : MonoBehaviour
{
    [Header("Listener (player/camera)")]
    [SerializeField] private Transform listenerTransform;

    [Header("Height Range (world Y)")]
    [SerializeField] private float minY = 20f;
    [SerializeField] private float maxY = 40f;
    [SerializeField] private float hysteresis = 1f;

    [Header("Audio")]
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private float targetVolume = 1f;
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private bool stopWhenOutOfRange = true;

    private AudioSource audioSource;
    private bool isInsideRange;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 0f;

        if (bgmClip != null)
            audioSource.clip = bgmClip;
    }

    private void Start()
    {
        if (listenerTransform == null && Camera.main != null)
            listenerTransform = Camera.main.transform;
    }

    private void Update()
    {
        if (listenerTransform == null || bgmClip == null || maxY <= minY)
            return;

        float y = listenerTransform.position.y;
        bool nextInside = isInsideRange
            ? y >= (minY - hysteresis) && y <= (maxY + hysteresis)
            : y >= (minY + hysteresis) && y <= (maxY - hysteresis);

        isInsideRange = nextInside;
        float desiredVolume = isInsideRange ? Mathf.Clamp01(targetVolume) : 0f;

        if (desiredVolume > 0f && !audioSource.isPlaying)
            audioSource.Play();

        float speed = fadeDuration > 0f ? Time.deltaTime / fadeDuration : 1f;
        audioSource.volume = Mathf.MoveTowards(audioSource.volume, desiredVolume, speed);

        if (!isInsideRange && stopWhenOutOfRange && audioSource.isPlaying && audioSource.volume <= 0.001f)
            audioSource.Stop();
    }
}
