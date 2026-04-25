using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class VolumeControlUI : MonoBehaviour
{
    [Header("AudioMixer")]
    public AudioMixer mixer;

    [Header("UI-элементы (можно не тянуть, найдём по имени)")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Image masterIcon; 
    public Image musicIcon;  
    public TMP_Text masterLabel;
    public TMP_Text musicLabel;

    public Sprite[] iconLevels;

    readonly string PREF_MASTER = "MasterVol";
    readonly string PREF_MUSIC = "MusicVol";

    void Start()
    {
        FindUI();
        InitSliders();
        SetCallbacks();
    }

    void FindUI()
    {
        if (masterSlider == null) masterSlider = GameObject.Find("MasterSlider")?.GetComponent<Slider>();
        if (musicSlider == null) musicSlider = GameObject.Find("MusicSlider")?.GetComponent<Slider>();
        if (masterIcon == null) masterIcon = GameObject.Find("MasterIcon")?.GetComponent<Image>();
        if (musicIcon == null) musicIcon = GameObject.Find("MusicIcon")?.GetComponent<Image>();
        if (masterLabel == null) masterLabel = GameObject.Find("MasterLabel")?.GetComponent<TMP_Text>();
        if (musicLabel == null) musicLabel = GameObject.Find("MusicLabel")?.GetComponent<TMP_Text>();
    }

    void InitSliders()
    {
        float mVol = PlayerPrefs.GetFloat(PREF_MASTER, 0.75f);
        float muVol = PlayerPrefs.GetFloat(PREF_MUSIC, 0.60f);

        masterSlider.value = mVol;
        musicSlider.value = muVol;

        ApplyVolume(mVol, true);
        ApplyVolume(muVol, false);
    }

    void SetCallbacks()
    {
        masterSlider.onValueChanged.AddListener(v => ApplyVolume(v, true));
        musicSlider.onValueChanged.AddListener(v => ApplyVolume(v, false));
    }

    void ApplyVolume(float value01, bool isMaster)
    {
        float dB = value01 <= 0.01f ? -80f : 20f * Mathf.Log10(Mathf.Clamp(value01, 0.0001f, 1f));
        string param = isMaster ? "MasterVol" : "MusicVol";
        mixer.SetFloat(param, dB);

        if (isMaster)
        {
            masterIcon.sprite = IconForValue(value01);
            masterLabel?.SetText($"{Mathf.RoundToInt(value01 * 100)}");
        }
        else
        {
            musicIcon.sprite = IconForValue(value01);
            musicLabel?.SetText($"{Mathf.RoundToInt(value01 * 100)}");
        }

        PlayerPrefs.SetFloat(isMaster ? PREF_MASTER : PREF_MUSIC, value01);
    }

    Sprite IconForValue(float v)
    {
        if (v <= 0.01f) return iconLevels[0];
        if (v < 0.33f) return iconLevels[1];
        if (v < 0.66f) return iconLevels[2];
        return iconLevels[3];
    }
}
