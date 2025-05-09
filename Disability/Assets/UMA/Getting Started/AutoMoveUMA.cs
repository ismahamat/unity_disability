using UnityEngine;
using UnityEngine.AI;
using UMA;
using UMA.CharacterSystem;

public class AutoMoveUMA : MonoBehaviour
{
    public float speed = 3.5f;
    public Transform waypointsParent;
    private Transform[] waypoints;
    private int currentWaypoint = 0;
    private Animator animator;
    private NavMeshAgent agent;
    private DynamicCharacterAvatar avatar;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        avatar = GetComponent<DynamicCharacterAvatar>();

        // Attendre que l'UMA soit généré
        if (avatar != null)
        {
            avatar.CharacterCreated.AddListener(OnCharacterCreated);
        }

        // Récupère tous les enfants du parent comme waypoints
        if (waypointsParent != null)
        {
            waypoints = new Transform[waypointsParent.childCount];
            for (int i = 0; i < waypointsParent.childCount; i++)
            {
                waypoints[i] = waypointsParent.GetChild(i);
            }
        }

        // Initialise le premier waypoint
        if (waypoints.Length > 0)
        {
            agent.speed = speed;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }

    void OnCharacterCreated(UMAData umaData)
    {
        // Récupère l'Animator après la génération du personnage
        animator = GetComponent<Animator>();
        Debug.Log("UMA Character generated");
    }

    void Update()
    {
        if (animator == null || agent == null || waypoints.Length == 0)
            return;

        // Si on est proche du waypoint, passer au suivant
        if (agent.remainingDistance < 0.5f && !agent.pathPending)
        {
            currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
            agent.SetDestination(waypoints[currentWaypoint].position);
        }

        // Gère les animations
        float normalizedSpeed = agent.velocity.magnitude / speed;
        animator.SetFloat("Speed", normalizedSpeed);
    }
}
