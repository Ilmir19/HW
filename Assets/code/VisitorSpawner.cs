using UnityEngine;

public class VisitorSpawner : MonoBehaviour
{
    public GameObject visitorPrefab;    
    public int count = 10;             

    public Transform[] spawnPoints;

    private void Start()
    {
        for (int i = 0; i < count; i++)
        {
            Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(visitorPrefab, sp.position, Quaternion.identity);
        }
    }
}
