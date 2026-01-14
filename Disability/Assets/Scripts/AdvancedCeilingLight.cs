using UnityEngine;

public class AdvancedCeilingLight : MonoBehaviour
{
    [Header("Paramètres de la lumière")]
    [SerializeField] private Color lightColor = new Color(1f, 0.96f, 0.92f);
    [SerializeField] private float intensity = 3f;
    [SerializeField] private float range = 8f;
    [SerializeField] private float spotAngle = 60f;
    [SerializeField] private float innerSpotAngle = 45f;

    [Header("Effets avancés")]
    [SerializeField] private bool useFlicker = false;
    [SerializeField] private float flickerSpeed = 0.5f;
    [SerializeField] private float flickerAmount = 0.15f;

    private Light lightComponent;
    private float baseIntensity;
    private float flickerTimer;

    void Start()
    {
        SetupLight();
        baseIntensity = intensity;
    }

    void SetupLight()
    {
        lightComponent = GetComponent<Light>();

        if (lightComponent == null)
        {
            lightComponent = gameObject.AddComponent<Light>();
        }

        lightComponent.type = LightType.Spot;
        lightComponent.color = lightColor;
        lightComponent.intensity = intensity;
        lightComponent.range = range;
        lightComponent.spotAngle = spotAngle;
        lightComponent.innerSpotAngle = innerSpotAngle;

        lightComponent.renderMode = LightRenderMode.ForcePixel;



        lightComponent.cullingMask = -1;
    }

    void Update()
    {
        if (useFlicker && lightComponent != null)
        {
            ApplyFlicker();
        }
    }

    void ApplyFlicker()
    {
        flickerTimer += Time.deltaTime * flickerSpeed;
        float noise = Mathf.PerlinNoise(flickerTimer, 0f);
        float variation = (noise - 0.5f) * flickerAmount;

        lightComponent.intensity = baseIntensity + variation;
    }
}