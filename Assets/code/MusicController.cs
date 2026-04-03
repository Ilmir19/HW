using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicSource;
    public float volumeInBar = 0.8f;
    public float volumeInLounge = 0.5f;
    public float volumeInDance = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bar")) musicSource.volume = volumeInBar;
        else if (other.CompareTag("Lounge")) musicSource.volume = volumeInLounge;
        else if (other.CompareTag("DanceFloor")) musicSource.volume = volumeInDance;
    }
}
