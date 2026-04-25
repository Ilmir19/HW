using UnityEngine;
using UnityEngine.UI;

public class SimpleTransition : MonoBehaviour
{
    public Image fadeImage;
    public GameObject showObject;
    public float delay = 3f;

    private void Start()
    {
        if (showObject != null)
            showObject.SetActive(false);

        Invoke("TriggerTransition", delay);
    }

    private void TriggerTransition()
    {
        if (fadeImage != null)
            fadeImage.gameObject.SetActive(false);

        if (showObject != null)
            showObject.SetActive(true);
    }
}
