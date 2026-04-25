using UnityEngine;

public class ResolutionControllerBT : MonoBehaviour
{
    private struct ResolutionInfo
    {
        public int width;
        public int height;
        public string label;

        public ResolutionInfo(int w, int h, string label)
        {
            width = w;
            height = h;
            this.label = label;
        }
    }

    private ResolutionInfo[] resolutions = {
        new ResolutionInfo(1280, 720, "720p"),
        new ResolutionInfo(1366, 768, "768p"),
        new ResolutionInfo(1920, 1080, "1080p"),
        new ResolutionInfo(2560, 1440, "1440p"),
        new ResolutionInfo(3840, 2160, "2160p")
    };

    void Start()
    {
        ApplyResolution(2);
    }

    public void ApplyResolution(int index)
    {
        if (index < 0 || index >= resolutions.Length)
        {
            Debug.LogWarning($"ResolutionController: Index {index} is out of range.");
            return;
        }

        var target = resolutions[index];
        Screen.SetResolution(target.width, target.height, false);
        Debug.Log($"Resolution changed to: {target.label} ({target.width}x{target.height})");
    }

    public bool IsResolutionSupported(int width, int height)
    {
        foreach (var res in Screen.resolutions)
        {
            if (res.width == width && res.height == height)
                return true;
        }
        return false;
    }

    public string GetCurrentResolutionLabel()
    {
        int currentWidth = Screen.width;
        int currentHeight = Screen.height;

        foreach (var res in resolutions)
        {
            if (res.width == currentWidth && res.height == currentHeight)
                return res.label;
        }

        return $"{currentWidth}x{currentHeight} (custom)";
    }
}
