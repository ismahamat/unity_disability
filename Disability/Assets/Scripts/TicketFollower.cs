using UnityEngine;
using System.Collections.Generic;

public class TicketFollower : MonoBehaviour
{
    public Transform player;
    public Transform waypointsParent;
    public float speed = 2f;
    public float playerApproachDistance = 5f;
    private List<Transform> waypoints = new List<Transform>();
    private int currentWaypoint = 0;
    private bool playerInZone = false;

    private void Start()
    {
        // R�cup�re tous les enfants du parent comme waypoints
        if (waypointsParent != null)
        {
            foreach (Transform child in waypointsParent)
            {
                waypoints.Add(child);
            }
        }

        if (waypoints.Count == 0)
        {
            Debug.LogWarning("Aucun waypoint trouv� pour " + gameObject.name);
        }
    }

    private void Update()
    {
        if (waypoints.Count == 0 || player == null || !playerInZone)
            return;

        // Avance vers le prochain waypoint
        MoveToNextWaypoint();
    }

    private void MoveToNextWaypoint()
    {
        // R�cup�re le waypoint cible
        Transform targetWaypoint = waypoints[currentWaypoint];
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;

        // D�place le ticket sur le sol (ignorer Y)
        Vector3 flatDirection = new Vector3(direction.x, 0, direction.z);
        transform.position += flatDirection * speed * Time.deltaTime;

  
        // V�rifie si on est proche du waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.5f)
        {
            if (currentWaypoint < waypoints.Count - 1)
            {
                currentWaypoint++;
            }
            else
            {
                playerInZone = false; // Arr�te le ticket au dernier waypoint
                Debug.Log("Ticket a atteint le dernier waypoint.");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = true;
            Debug.Log("Le joueur est entr� dans la zone du ticket.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInZone = false;
            Debug.Log("Le joueur a quitt� la zone du ticket.");
        }
    }
}
