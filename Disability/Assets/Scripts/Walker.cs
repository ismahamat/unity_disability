using UnityEngine;
using UnityEngine.AI;

public class Walker : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    private NavMeshAgent agent;
    private Transform currentTarget;
    private Animator animator;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        currentTarget = pointB;
        agent.SetDestination(currentTarget.position);
    }

    void Update()
    {
        // Mettre à jour l'état d'animation selon la vitesse
        animator.SetBool("isWalking", agent.velocity.magnitude > 0.1f);

        // Faire demi-tour à l'arrivée
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentTarget = currentTarget == pointA ? pointB : pointA;
            agent.SetDestination(currentTarget.position);
        }
    }
}
