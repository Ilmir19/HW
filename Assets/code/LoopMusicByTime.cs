using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class LoopMusicByTime : MonoBehaviour
{
    [Header("Loop Settings")]
    [Tooltip("Момент, с которого начинается зацикливание (после интро)")]
    public float startLoopTime = 3f;

    [Tooltip("Момент, на котором зацикливание заканчивается (возвращаемся к startLoopTime)")]
    public float endLoopTime = 50f;

    [Header("Audio")]
    [Tooltip("Аудиоклип, который будет проигрываться и зацикливаться")]
    public AudioClip audioClip;

    [Tooltip("Если true — при смене аудиоклипа автоматически запускается воспроизведение")]
    public bool autoPlayOnClipChange = true;

    AudioSource src;

    bool introPassed = false;

    void Awake()
    {
        src = GetComponent<AudioSource>();
        src.loop = false;
        ValidateLoopTimes();
        Play();
    }

    void OnValidate()
    {
        ValidateLoopTimes();
    }

    void Update()
    {
        if (!src.isPlaying || src.clip == null) return;

        float currentTime = (float)src.timeSamples / src.clip.frequency;

        if (!introPassed && currentTime >= startLoopTime)
        {
            JumpTo(startLoopTime);
            introPassed = true;
            return;
        }

        if (introPassed && currentTime >= endLoopTime)
        {
            JumpTo(startLoopTime);
        }
    }

    public void SetAudioClip(AudioClip newClip)
    {
        if (newClip == null)
        {
            Debug.LogWarning("DynamicLoopMusic: Attempted to set null AudioClip.");
            return;
        }

        audioClip = newClip;
        src.clip = newClip;
        ValidateLoopTimes();

        if (autoPlayOnClipChange)
        {
            Play();
        }
    }

    public void Play()
    {
        if (src.clip == null) return;

        src.timeSamples = 0;
        introPassed = false;
        src.Play();
    }

    void JumpTo(float seconds)
    {
        int sample = Mathf.RoundToInt(seconds * src.clip.frequency);
        sample = Mathf.Clamp(sample, 0, src.clip.samples - 1);
        src.timeSamples = sample;
    }

    void ValidateLoopTimes()
    {
        if (audioClip != null)
        {
            if (startLoopTime < 0) startLoopTime = 0;
            if (endLoopTime > audioClip.length) endLoopTime = audioClip.length;
            if (startLoopTime >= endLoopTime)
            {
                startLoopTime = Mathf.Max(0, endLoopTime - 1f);
                Debug.LogWarning($"DynamicLoopMusic: startLoopTime must be < endLoopTime. Adjusted to {startLoopTime}s.");
            }
        }
    }

    public void Pause() => src.Pause();
    public void Stop() => src.Stop();
    public bool IsPlaying => src.isPlaying;
}
