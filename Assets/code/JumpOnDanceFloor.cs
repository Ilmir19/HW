using UnityEngine;

public class JumpOnDanceFloor : MonoBehaviour
{
    private Vector3 startPos;
    private float timer;

    private void Start() => startPos = transform.position;

    private void Update()
    {
        timer += Time.deltaTime;
        float y = Mathf.Sin(timer * Mathf.PI * 2) * 0.25f;
        transform.position = startPos + new Vector3(0, y, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DanceFloor"))
        {
            GetComponent<Animator>().SetTrigger("Dance");
        }
    }

}
