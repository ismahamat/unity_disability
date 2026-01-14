using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class OcclusionCullingSetup : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private float occluderMinSize = 2f;

    [ContextMenu("1. Marquer les objets")]
    void MarkObjects()
    {
#if UNITY_EDITOR
        MeshRenderer[] allRenderers = FindObjectsOfType<MeshRenderer>();
        
        foreach (MeshRenderer renderer in allRenderers)
        {
            GameObject obj = renderer.gameObject;
            Bounds bounds = renderer.bounds;
            float maxSize = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);
            
            if (maxSize >= occluderMinSize)
            {
                GameObjectUtility.SetStaticEditorFlags(obj, 
                    StaticEditorFlags.OccluderStatic | 
                    StaticEditorFlags.OccludeeStatic);
            }
            else
            {
                GameObjectUtility.SetStaticEditorFlags(obj, 
                    StaticEditorFlags.OccludeeStatic);
            }
        }
#endif
    }
}