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

        // Récupère l'Animator après la génération du personnage UMA
        if (avatar != null)
        {
            avatar.CharacterCreated.AddListener(OnCharacterCreated);
        }

        // Définir un premier intervalle aléatoire
        SetNewWanderInterval();
    }

    private void OnCharacterCreated(UMAData umaData)
    {
        // Récupère l'Animator après la génération du personnage
        animator = GetComponent<Animator>();
        Debug.Log("UMA Character generated");
    }

    private void Update()
    {
        if (animator == null || agent == null)
            return;

        // Gère les animations
        float normalizedSpeed = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("Speed", normalizedSpeed);

        // Déplace aléatoirement le PNJ
        timer += Time.deltaTime;

        if (timer >= wanderInterval)
        {
            Vector3 newPosition = GetRandomPoint(transform.position, wanderRadius);
            agent.SetDestination(newPosition);
            timer = 0f;

            // Définir un nouvel intervalle aléatoire
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
