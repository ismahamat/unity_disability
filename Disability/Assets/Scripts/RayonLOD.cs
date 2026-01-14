using UnityEngine;
using System.Collections.Generic;

public class RayonLOD : MonoBehaviour
{
    [Header("LOD Configuration")]
    [SerializeField] private Transform player;
    [SerializeField] private float activationDistance = 10f;
    [SerializeField] private float deactivationDistance = 15f;

    private List<GameObject> produits = new List<GameObject>();
    private bool isActive = false;
    private float updateInterval = 0.5f;
    private float nextUpdateTime = 0f;

    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // ← NOUVELLE MÉTHODE : Recevoir la liste de produits directement
    public void SetProducts(List<GameObject> products)
    {
        produits = products;
        // Désactiver au départ
        SetProduitsActive(false);
    }

    void Update()
    {
        if (Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + updateInterval;
            CheckDistance();
        }
    }

    void CheckDistance()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (!isActive && distance <= activationDistance)
        {
            SetProduitsActive(true);
        }
        else if (isActive && distance >= deactivationDistance)
        {
            SetProduitsActive(false);
        }
    }

    void SetProduitsActive(bool active)
    {
        isActive = active;

        foreach (GameObject produit in produits)
        {
            if (produit != null)
            {
                produit.SetActive(active);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, activationDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, deactivationDistance);
    }
}