using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResolutionButtons : MonoBehaviour
{
    [Header("Кнопки (можно не тянуть, найдём по имени)")]
    public Button btnLeft;
    public Button btnRight;
    public Button btnFullscreen;

    [Header("Куда писать текущее разрешение")]
    public TMP_Text label;

    readonly (int w, int h, string name)[] res =
    {
        (1280, 720, "HD  720p"),
        (1366, 768, "WXGA 768p"),
        (1920,1080, "FullHD 1080p"),
        (2560,1440, "QHD 1440p"),
        (3840,2160, "4К  2160p")
    };

    int index;

    void Start()
    {
        if (btnLeft == null) btnLeft = GameObject.Find("Left")?.GetComponent<Button>();
        if (btnRight == null) btnRight = GameObject.Find("Right")?.GetComponent<Button>();
        if (btnFullscreen == null) btnFullscreen = GameObject.Find("FullscreenBtn")?.GetComponent<Button>();
        if (label == null) label = GameObject.Find("Label")?.GetComponent<TMP_Text>();

        // вешаем обработчики
        btnLeft?.onClick.AddListener(() => Change(+1));
        btnRight?.onClick.AddListener(() => Change(-1));
        btnFullscreen?.onClick.AddListener(ToggleFS);

        index = PlayerPrefs.GetInt("ResIdx", 2);
        Apply();
    }

    void Change(int dir)
    {
        index = (index + dir + res.Length) % res.Length;
        Apply();
    }

    void Apply()
    {
        var r = res[index];
        Screen.SetResolution(r.w, r.h, Screen.fullScreen);
        label?.SetText($"{r.name}  {r.w}×{r.h}");
        PlayerPrefs.SetInt("ResIdx", index);
    }

    void ToggleFS()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
