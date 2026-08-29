using System.Collections;
using UnityEngine;

/// <summary>
/// Creates a simple monument and handles white glow + particles without external assets.
/// </summary>
public class CheckpointVisual : MonoBehaviour
{
    [Header("Appearance")]
    [SerializeField] private Color inactiveColor = new Color(0.12f, 0.12f, 0.15f, 1f);
    [SerializeField] private Color activeColor = Color.white;
    [SerializeField] private float glowDuration = 0.6f;

    [Header("Particles")]
    [SerializeField] private int burstCount = 45;
    [SerializeField] private float particleLifetime = 1.2f;
    [SerializeField] private float particleRadius = 0.9f;

    private Renderer[] renderers;
    private MaterialPropertyBlock propertyBlock;
    private ParticleSystem particles;
    private Light glowLight;
    private bool active;

    private void Awake()
    {
        BuildMonumentIfNeeded();
        CacheRenderers();
        CreateParticles();
        CreateLight();
        SetColor(inactiveColor);
    }

    private void BuildMonumentIfNeeded()
    {
        if (transform.childCount > 0)
            return;

        CreatePrimitive(PrimitiveType.Cylinder, "Base", new Vector3(0f, 0.25f, 0f), new Vector3(1.15f, 0.25f, 1.15f));
        CreatePrimitive(PrimitiveType.Cylinder, "Pillar", new Vector3(0f, 1.15f, 0f), new Vector3(0.48f, 0.9f, 0.48f));
        CreatePrimitive(PrimitiveType.Cube, "Crown", new Vector3(0f, 2.05f, 0f), new Vector3(0.85f, 0.25f, 0.85f));
        CreatePrimitive(PrimitiveType.Sphere, "Core", new Vector3(0f, 2.45f, 0f), new Vector3(0.38f, 0.38f, 0.38f));
    }

    private GameObject CreatePrimitive(PrimitiveType type, string objectName, Vector3 localPosition, Vector3 localScale)
    {
        GameObject obj = GameObject.CreatePrimitive(type);
        obj.name = objectName;
        obj.transform.SetParent(transform, false);
        obj.transform.localPosition = localPosition;
        obj.transform.localScale = localScale;

        Collider collider = obj.GetComponent<Collider>();
        if (collider != null)
            Destroy(collider);

        return obj;
    }

    private void CacheRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>();
        propertyBlock = new MaterialPropertyBlock();
    }

    private void CreateParticles()
    {
        GameObject particleObject = new GameObject("CheckpointParticles");
        particleObject.transform.SetParent(transform, false);

        particles = particleObject.AddComponent<ParticleSystem>();
        var main = particles.main;
        main.loop = false;
        main.playOnAwake = false;
        main.startLifetime = particleLifetime;
        main.startSpeed = 1.5f;
        main.startSize = 0.08f;
        main.startColor = Color.white;
        main.maxParticles = burstCount;

        var emission = particles.emission;
        emission.enabled = true;
        emission.rateOverTime = 0f;

        var shape = particles.shape;
        shape.enabled = true;
        shape.shapeType = ParticleSystemShapeType.Circle;
        shape.radius = particleRadius;

        var velocity = particles.velocityOverLifetime;
        velocity.enabled = true;
        velocity.space = ParticleSystemSimulationSpace.Local;
        velocity.radial = new ParticleSystem.MinMaxCurve(0.5f);
    }

    private void CreateLight()
    {
        GameObject lightObject = new GameObject("CheckpointGlow");
        lightObject.transform.SetParent(transform, false);
        lightObject.transform.localPosition = new Vector3(0f, 2.2f, 0f);

        glowLight = lightObject.AddComponent<Light>();
        glowLight.type = LightType.Point;
        glowLight.color = Color.white;
        glowLight.range = 5f;
        glowLight.intensity = 0f;
        glowLight.enabled = true;
    }

    public void Activate()
    {
        active = true;
        StopAllCoroutines();
        StartCoroutine(ActivationRoutine());

        if (particles != null)
        {
            particles.Clear();
            particles.Emit(burstCount);
        }
    }

    public void Deactivate()
    {
        active = false;
        StopAllCoroutines();
        SetColor(inactiveColor);

        if (glowLight != null)
            glowLight.intensity = 0f;
    }

    private IEnumerator ActivationRoutine()
    {
        float elapsed = 0f;
        Color start = inactiveColor;

        while (elapsed < glowDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / glowDuration);
            t = Mathf.SmoothStep(0f, 1f, t);
            SetColor(Color.Lerp(start, activeColor, t));

            if (glowLight != null)
                glowLight.intensity = Mathf.Lerp(0f, 3f, t);

            yield return null;
        }

        SetColor(activeColor);

        if (glowLight != null)
            glowLight.intensity = 3f;
    }

    private void SetColor(Color color)
    {
        if (renderers == null)
            return;

        foreach (Renderer renderer in renderers)
        {
            if (renderer == null)
                continue;

            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_BaseColor", color);
            propertyBlock.SetColor("_Color", color);
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
