using UnityEngine;
using UnityEngine.AI;
using UMA;
using UMA.CharacterSystem;

public class RandomMovement : MonoBehaviour
{
    public float wanderRadius = 10f;
    public float minWanderInterval = 2f;
    public float maxWanderInterval = 5f;
    private NavMeshAgent agent;
    private Animator animator;
    private DynamicCharacterAvatar avatar;
    private float wanderInterval;
    private float timer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        avatar = GetComponent<DynamicCharacterAvatar>();

        // R�cup�re l'Animator apr�s la g�n�ration du personnage UMA
        if (avatar != null)
        {
            avatar.CharacterCreated.AddListener(OnCharacterCreated);
        }

        // D�finir un premier intervalle al�atoire
        SetNewWanderInterval();
    }

    private void OnCharacterCreated(UMAData umaData)
    {
        // R�cup�re l'Animator apr�s la g�n�ration du personnage
        animator = GetComponent<Animator>();
        Debug.Log("UMA Character generated");
    }

    private void Update()
    {
        if (animator == null || agent == null)
            return;

        // G�re les animations
        float normalizedSpeed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Speed", normalizedSpeed);

        // D�place al�atoirement le PNJ
        timer += Time.deltaTime;

        if (timer >= wanderInterval)
        {
            Vector3 newPosition = GetRandomPoint(transform.position, wanderRadius);
            agent.SetDestination(newPosition);
            timer = 0f;

            // D�finir un nouvel intervalle al�atoire
            SetNewWanderInterval();
        }
    }

    private void SetNewWanderInterval()
    {
        wanderInterval = Random.Range(minWanderInterval, maxWanderInterval);
        Debug.Log("Nouvel intervalle de marche : " + wanderInterval);
    }

    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        Vector3 finalPosition = center + new Vector3(randomPoint.x, 0, randomPoint.y);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(finalPosition, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return center;
    }
}
