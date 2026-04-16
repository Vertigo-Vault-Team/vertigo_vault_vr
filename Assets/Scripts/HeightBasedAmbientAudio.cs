using UnityEngine;

public class HeightBasedAmbientAudio : MonoBehaviour
{
    [Header("Listener (player/camera)")]
    [SerializeField] private Transform listenerTransform;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip citySound;
    [SerializeField] private AudioClip winterWind;

    [Header("Height Switch (world Y)")]
    [SerializeField] private float midHeightY = 35f;
    [SerializeField] private float hysteresis = 2f;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 1.5f;

    private bool isHighZone;

    private void Start()
    {
        if (listenerTransform == null && Camera.main != null)
        {
            listenerTransform = Camera.main.transform;
        }

        if (citySound == null || winterWind == null)
        {
            Debug.LogWarning("HeightBasedAmbientAudio: citySound/winterWind clip is not assigned.");
            return;
        }

        EnsureSoundManager();
        SoundManager.Instance.PlayBgmImmediate(citySound, 1f);
    }

    private void Update()
    {
        if (listenerTransform == null || citySound == null || winterWind == null)
            return;

        float y = listenerTransform.position.y;

        if (!isHighZone && y >= midHeightY + hysteresis)
        {
            isHighZone = true;
            SoundManager.Instance.CrossfadeBgm(winterWind, fadeDuration, 1f);
        }
        else if (isHighZone && y <= midHeightY - hysteresis)
        {
            isHighZone = false;
            SoundManager.Instance.CrossfadeBgm(citySound, fadeDuration, 1f);
        }
    }

    private static void EnsureSoundManager()
    {
        if (SoundManager.Instance == null)
        {
            GameObject manager = new GameObject("SoundManager");
            manager.AddComponent<SoundManager>();
        }
    }
}
