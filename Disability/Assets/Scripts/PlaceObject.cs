using System.Collections.Generic;
using UnityEngine;

public class MakeSupermarket : MonoBehaviour
{
    [Header("Prefab & Count")]
    public GameObject productPrefab;
    public int productCount = 50;

    [Header("Disorder Settings")]
    [Range(0f, 10f)]
    public float disorderLevel = 5f;
    public float perlinFrequency = 2f;
    public float disorderAmplitude = 0.4f;

    [Header("Placement Settings")]
    public float shelfMargin = 0.05f;

    private float planeWidth;
    private float planeDepth;
    private List<GameObject> spawnedProducts = new List<GameObject>(); // ← AJOUTE ÇA

    void Start()
    {
        InitPlaneSize();
        PlaceProducts();

        // Notifier le LOD
        RayonLOD lod = GetComponent<RayonLOD>();
        if (lod != null)
        {
            lod.SetProducts(spawnedProducts); // ← AJOUTE ÇA
        }
    }

    void InitPlaneSize()
    {
        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        Bounds bounds = mesh.bounds;
        planeWidth = bounds.size.x * transform.localScale.x;
        planeDepth = bounds.size.z * transform.localScale.z;
    }

    void PlaceProducts()
    {
        int gridSize = Mathf.CeilToInt(Mathf.Sqrt(productCount));
        int placed = 0;

        float minX = -planeWidth * 0.5f * (1f - shelfMargin);
        float maxX = planeWidth * 0.5f * (1f - shelfMargin);
        float minZ = -planeDepth * 0.5f * (1f - shelfMargin);
        float maxZ = planeDepth * 0.5f * (1f - shelfMargin);

        for (int gz = 0; gz < gridSize && placed < productCount; gz++)
        {
            for (int gx = 0; gx < gridSize && placed < productCount; gx++)
            {
                float tX = (float)gx / (gridSize - 1);
                float tZ = (float)gz / (gridSize - 1);

                float baseX = Mathf.Lerp(minX, maxX, tX);
                float baseZ = Mathf.Lerp(minZ, maxZ, tZ);

                Vector2 perlinOffset = GetPerlinOffset(gx, gz);

                Vector3 localPos = new Vector3(
                    baseX + perlinOffset.x,
                    0,
                    baseZ + perlinOffset.y
                );

                Vector3 worldPos = transform.TransformPoint(localPos);

                float angle = perlinOffset.x * 90f;
                Quaternion rot = Quaternion.Euler(0, angle, 0);

                // ← TON CODE ORIGINAL (sans parent)
                GameObject produit = Instantiate(productPrefab, worldPos, rot);
                spawnedProducts.Add(produit); // ← AJOUTE ÇA

                placed++;
            }
        }
    }

    Vector2 GetPerlinOffset(int gx, int gz)
    {
        if (disorderLevel <= 0f)
            return Vector2.zero;

        float seedX = 157.123f;
        float seedZ = 951.727f;

        float perlinX = Mathf.PerlinNoise(
            (gx + seedX) * perlinFrequency,
            (gz + seedZ) * perlinFrequency
        );
        float perlinZ = Mathf.PerlinNoise(
            (gx + seedZ) * perlinFrequency,
            (gz + seedX) * perlinFrequency
        );

        float amplitude = disorderLevel * disorderAmplitude;

        return new Vector2(
            (perlinX - 0.5f) * amplitude,
            (perlinZ - 0.5f) * amplitude
        );
    }
}