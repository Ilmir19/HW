using UnityEngine;
using UnityEngine.UI;

public class ResolutionController : MonoBehaviour
{
    [System.Serializable]
    public class ResolutionItem
    {
        public int width;
        public int height;
        public string label;
    }

    [Header("—писок разрешений, которые можно выбрать")]
    public ResolutionItem[] resolutions =
    {
        new ResolutionItem{ width = 1280, height =  720, label = "HD  720p"},
        new ResolutionItem{ width = 1366, height =  768, label = "WXGA 768p"},
        new ResolutionItem{ width = 1920, height = 1080, label = "FullHD 1080p"},
        new ResolutionItem{ width = 2560, height = 1440, label = "QHD  1440p"},
        new ResolutionItem{ width = 3840, height = 2160, label = "4K   2160p"}
    };

    [Header("—тартовое разрешение (есого нет в save)")]
    public int defaultIndex = 2;

    private int currentIndex;

    void Start()
    {
        LoadResolution();
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= resolutions.Length) return;

        currentIndex = index;
        var res = resolutions[index];

        Screen.SetResolution(res.width, res.height, FullScreenMode.FullScreenWindow);

        SaveResolution();
    }

    public void ToggleFullscreen()
    {
        bool next = !Screen.fullScreen;
        Screen.fullScreen = next;

        PlayerPrefs.SetInt("Fullscreen", next ? 1 : 0);
    }

    public void NextResolution() => SetResolution((currentIndex + 1) % resolutions.Length);
    public void PrevResolution()
    {
        int i = currentIndex - 1;
        if (i < 0) i = resolutions.Length - 1;
        SetResolution(i);
    }

    private void SaveResolution()
    {
        PlayerPrefs.SetInt("ResIndex", currentIndex);
        PlayerPrefs.SetInt("Fullscreen", Screen.fullScreen ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadResolution()
    {
        currentIndex = PlayerPrefs.GetInt("ResIndex", defaultIndex);
        bool fs = PlayerPrefs.GetInt("Fullscreen", 1) == 1;

        var res = resolutions[currentIndex];
        Screen.SetResolution(res.width, res.height, fs ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed);
    }
}
