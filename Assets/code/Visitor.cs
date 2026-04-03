using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Visitor : MonoBehaviour
{
    [Header("Настройки")]
    public Transform[] danceFloorPoints; 
    public Transform[] barPoints;      
    public Transform[] loungePoints;    

    private NavMeshAgent agent;
    private float waitTime;              
    private bool isWaiting;

    private void Awake() => agent = GetComponent<NavMeshAgent>();

    private void Start()
    {
        ChooseNewDestination();
    }

    private void Update()
    {
        if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
            waitTime = Random.Range(1f, 8f);
            StartCoroutine(WaitAndChooseNewDestination());
        }
    }

    private IEnumerator WaitAndChooseNewDestination()
    {
        yield return new WaitForSeconds(waitTime);
        isWaiting = false;
        ChooseNewDestination();
    }

    private void ChooseNewDestination()
    {
        int zone = Random.Range(0, 3);
        Transform[] points = zone switch
        {
            0 => danceFloorPoints,
            1 => barPoints,
            _ => loungePoints
        };

        Transform target = points[Random.Range(0, points.Length)];
        agent.SetDestination(target.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DanceFloor"))
        {
            var anim = GetComponent<Animator>();
            if (anim) anim.SetBool("IsDancing", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("DanceFloor"))
        {
            var anim = GetComponent<Animator>();
            if (anim) anim.SetBool("IsDancing", false);
        }
    }
}
