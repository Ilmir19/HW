using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    [System.Serializable]
    public struct ResolutionOption
    {
        public int width;
        public int height;
        public string label;
    }

    public ResolutionOption[] resolutions = {
        new ResolutionOption { width = 1920, height = 1080, label = "Full HD" },
        new ResolutionOption { width = 1280, height = 720, label = "HD" },
        new ResolutionOption { width = 1366, height = 768, label = "HD+" },
        new ResolutionOption { width = 2560, height = 1440, label = "QHD" },
        new ResolutionOption { width = 3840, height = 2160, label = "4K" }
    };

    public GameObject[] resolutionButtons;
    private int currentResolutionIndex = 0;

    private void Start()
    {
        // Проверяем, какие разрешения реально доступны
        Resolution[] availableResolutions = Screen.resolutions;
        Debug.Log($"Доступные разрешения: {availableResolutions.Length}");

        // Автоматически выбираем ближайшее доступное
        AutoSetBestResolution();
        HighlightActiveButton(currentResolutionIndex);
    }

    public void SetResolution(int index)
    {
        if (index < 0 || index >= resolutions.Length) return;

        var target = resolutions[index];

        // 🔥 КРИТИЧНО: Используем ExclusiveFullScreen — это НЕОБХОДИМО для смены разрешения!
        Screen.SetResolution(target.width, target.height, FullScreenMode.ExclusiveFullScreen);

        // Обязательно обновляем Canvas!
        Canvas.ForceUpdateCanvases();

        currentResolutionIndex = index;
        Debug.Log($"✅ Разрешение изменено: {target.label} ({target.width}x{target.height})");

        HighlightActiveButton(index);
    }

    private void AutoSetBestResolution()
    {
        int currentWidth = Screen.currentResolution.width;
        int currentHeight = Screen.currentResolution.height;
        int bestIndex = 0;
        float minDiff = float.MaxValue;

        for (int i = 0; i < resolutions.Length; i++)
        {
            var res = resolutions[i];
            float diff = Mathf.Abs(res.width * res.height - currentWidth * currentHeight);

            // Дополнительно: проверяем, что разрешение реально доступно
            bool isSupported = false;
            foreach (var avail in Screen.resolutions)
            {
                if (avail.width == res.width && avail.height == res.height)
                {
                    isSupported = true;
                    break;
                }
            }

            if (isSupported && diff < minDiff)
            {
                minDiff = diff;
                bestIndex = i;
            }
        }

        // Если ничего не найдено — используем первое доступное
        if (minDiff == float.MaxValue && Screen.resolutions.Length > 0)
        {
            var first = Screen.resolutions[0];
            bestIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].width == first.width && resolutions[i].height == first.height)
                {
                    bestIndex = i;
                    break;
                }
            }
        }

        SetResolution(bestIndex);
    }

    private void HighlightActiveButton(int index)
    {
        if (resolutionButtons == null || resolutionButtons.Length == 0) return;

        for (int i = 0; i < resolutionButtons.Length; i++)
        {
            var button = resolutionButtons[i];
            var image = button.GetComponent<Image>();
            var text = button.GetComponentInChildren<Text>();

            if (image != null) image.color = i == index ? Color.yellow : Color.white;
            if (text != null) text.color = i == index ? Color.black : Color.gray;
        }
    }
}
