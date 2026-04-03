using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Guard : MonoBehaviour
{
    public float speedMultiplier = 1.5f; 
    public float stopDistance = 3f;   
    private NavMeshAgent agent;
    private GameObject[] visitors;     
    private bool isWaiting;

    private void Awake() => agent = GetComponent<NavMeshAgent>();

    private void Start()
    {
        agent.speed *= speedMultiplier;
        StartCoroutine(UpdateTargetRoutine());
    }

    private IEnumerator UpdateTargetRoutine()
    {
        while (true)
        {
            visitors = GameObject.FindGameObjectsWithTag("Visitor");
            if (visitors.Length > 0)
            {
                GameObject target = visitors[Random.Range(0, visitors.Length)];
                agent.stoppingDistance = stopDistance;
                agent.SetDestination(target.transform.position);
            }
            else
            {
                agent.ResetPath();
            }

            while (!agent.pathPending && agent.remainingDistance > agent.stoppingDistance)
                yield return null;

            float wait = Random.Range(1f, 3f);
            yield return new WaitForSeconds(wait);
        }
    }
}
