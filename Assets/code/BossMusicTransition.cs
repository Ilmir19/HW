using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BossMusicTransition : MonoBehaviour
{
    [Header("Battle Music (Before Victory)")]
    public AudioClip battleMusic;
    public float battleStartLoopTime = 3f;
    public float battleEndLoopTime = 50f; 

    [Header("Victory Music (After Victory)")]
    public AudioClip victoryMusic;
    public float victoryStartLoopTime = 3f; 
    public float victoryEndLoopTime = 45f; 

    [Header("Transition")]
    public float delayAfterVictory = 6f; 
    public bool autoPlayOnClipChange = true; 

    private AudioSource src;
    private bool battleIntroPassed = false;
    private bool victoryIntroPassed = false;
    private bool isPlayingBattle = true;  

    private void Awake()
    {
        src = GetComponent<AudioSource>();
        src.loop = false; 

        ValidateLoopTimes(battleMusic, ref battleStartLoopTime, ref battleEndLoopTime);
        ValidateLoopTimes(victoryMusic, ref victoryStartLoopTime, ref victoryEndLoopTime);

        PlayBattleMusic();
    }

    private void OnValidate()
    {
        ValidateLoopTimes(battleMusic, ref battleStartLoopTime, ref battleEndLoopTime);
        ValidateLoopTimes(victoryMusic, ref victoryStartLoopTime, ref victoryEndLoopTime);
    }

    private void Update()
    {
        if (!src.isPlaying || src.clip == null) return;

        float currentTime = (float)src.timeSamples / src.clip.frequency;

        if (isPlayingBattle)
        {
            if (!battleIntroPassed && currentTime >= battleStartLoopTime)
            {
                JumpTo(battleStartLoopTime);
                battleIntroPassed = true;
                return;
            }

            if (battleIntroPassed && currentTime >= battleEndLoopTime)
            {
                JumpTo(battleStartLoopTime);
            }
        }
        else 
        {
            if (!victoryIntroPassed && currentTime >= victoryStartLoopTime)
            {
                JumpTo(victoryStartLoopTime);
                victoryIntroPassed = true;
                return;
            }

            if (victoryIntroPassed && currentTime >= victoryEndLoopTime)
            {
                JumpTo(victoryStartLoopTime);
            }
        }
    }



    public void PlayBattleMusic()
    {
        if (battleMusic == null)
        {
            Debug.LogError("Battle music clip not assigned in BossMusicManager!");
            return;
        }

        src.clip = battleMusic;
        src.timeSamples = 0;
        battleIntroPassed = false;
        victoryIntroPassed = false;
        isPlayingBattle = true;

        if (autoPlayOnClipChange) src.Play();
    }

    public void PlayVictoryMusic()
    {
        if (victoryMusic == null)
        {
            Debug.LogWarning("Victory music clip not assigned. Skipping playback.");
            return;
        }

        src.clip = victoryMusic;
        src.timeSamples = 0;
        battleIntroPassed = false;
        victoryIntroPassed = false;
        isPlayingBattle = false;

        if (autoPlayOnClipChange) src.Play();
    }


    public void OnBossDefeated()
    {
        if (src == null) return;

        src.Stop(); 
        Invoke("PlayVictoryMusic", delayAfterVictory);
    }



    void JumpTo(float seconds)
    {
        if (src.clip == null) return;
        int sample = Mathf.RoundToInt(seconds * src.clip.frequency);
        sample = Mathf.Clamp(sample, 0, src.clip.samples - 1);
        src.timeSamples = sample;
    }

    void ValidateLoopTimes(AudioClip clip, ref float start, ref float end)
    {
        if (clip == null) return;

        if (start < 0) start = 0;
        if (end > clip.length) end = clip.length;
        if (start >= end)
        {
            start = Mathf.Max(0, end - 1f);
            Debug.LogWarning($"Loop times adjusted: start={start}s, end={end}s");
        }
    }

    public void Pause() => src.Pause();
    public void Stop() => src.Stop();
    public bool IsPlaying => src.isPlaying;

    public void SetBattleMusic(AudioClip newClip)
    {
        battleMusic = newClip;
        ValidateLoopTimes(battleMusic, ref battleStartLoopTime, ref battleEndLoopTime);
        if (isPlayingBattle && autoPlayOnClipChange) PlayBattleMusic();
    }

    public void SetVictoryMusic(AudioClip newClip)
    {
        victoryMusic = newClip;
        ValidateLoopTimes(victoryMusic, ref victoryStartLoopTime, ref victoryEndLoopTime);
    }
}
