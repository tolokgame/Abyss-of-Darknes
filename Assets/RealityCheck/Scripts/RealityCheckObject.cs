using System.Collections;
using UnityEngine;

public class RealityCheckObject : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;

    [Header("Mode")]
    [SerializeField] private bool disappearForever = false;

    [Header("Distance Mode")]
    [SerializeField] private float disappearDistance = 2f;
    [SerializeField] private float appearDistance = 3f;

    [Header("Fade")]
    [SerializeField] private float fadeDuration = 0.5f;

    private Renderer[] renderers;
    private Collider[] colliders;
    private Material[][] materials;

    private bool isVisible = true;
    private bool hasDisappearedForever = false;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);

        materials = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].materials;

            foreach (Material material in materials[i])
            {
                PrepareMaterial(material);
            }
        }
    }

    private void Update()
    {
        if (player == null)
            return;

        // Если объект уже исчез навсегда — больше ничего не делаем.
        if (hasDisappearedForever)
            return;

        float distance = Vector3.Distance(
            player.position,
            transform.position
        );

        
        // Режим 1 — Disappear Forever включен
        

        if (disappearForever)
        {
            if (distance <= disappearDistance && isVisible)
            {
                StartFade(false);
            }

            return;
        }

        
        // Режим 2 — Disappear Forever выключен
        

        // Подошли — исчезает.
        if (distance <= disappearDistance && isVisible)
        {
            StartFade(false);
        }

        // Отошли — появляеться.
        if (distance >= appearDistance && !isVisible)
        {
            StartFade(true);
        }
    }

    private void StartFade(bool show)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(Fade(show));
    }

    private IEnumerator Fade(bool show)
    {
        isVisible = show;

        if (show)
        {
            SetRenderersEnabled(true);
            SetCollidersEnabled(false);
        }

        float startAlpha = GetCurrentAlpha();
        float targetAlpha = show ? 1f : 0f;

        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float progress = fadeDuration <= 0f
                ? 1f
                : timer / fadeDuration;

            float alpha = Mathf.Lerp(
                startAlpha,
                targetAlpha,
                progress
            );

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(targetAlpha);

        if (show)
        {
            // Объект появился.
            SetCollidersEnabled(true);
        }
        else
        {
            // Объект исчез.
            SetCollidersEnabled(false);

            // Renderer выключается только после fade.
            SetRenderersEnabled(false);

            // Если включён режим "навсегда",
            // больше никогда его не показываем.
            if (disappearForever)
            {
                hasDisappearedForever = true;
            }
        }

        fadeCoroutine = null;
    }

    private void SetAlpha(float alpha)
    {
        for (int i = 0; i < materials.Length; i++)
        {
            foreach (Material material in materials[i])
            {
                if (!material.HasProperty("_Color"))
                    continue;

                Color color = material.color;
                color.a = alpha;

                material.color = color;
            }
        }
    }

    private float GetCurrentAlpha()
    {
        for (int i = 0; i < materials.Length; i++)
        {
            foreach (Material material in materials[i])
            {
                if (material.HasProperty("_Color"))
                    return material.color.a;
            }
        }

        return isVisible ? 1f : 0f;
    }

    private void SetRenderersEnabled(bool value)
    {
        foreach (Renderer renderer in renderers)
        {
            renderer.enabled = value;
        }
    }

    private void SetCollidersEnabled(bool value)
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = value;
        }
    }

    private void PrepareMaterial(Material material)
    {
        if (material == null)
            return;

        // Standard Shader
        if (material.shader.name == "Standard")
        {
            material.SetFloat("_Mode", 3);

            material.SetInt(
                "_SrcBlend",
                (int)UnityEngine.Rendering.BlendMode.SrcAlpha
            );

            material.SetInt(
                "_DstBlend",
                (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha
            );

            material.SetInt("_ZWrite", 0);

            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");

            material.renderQueue = 3000;
        }

        // URP
        if (material.HasProperty("_Surface"))
        {
            material.SetFloat("_Surface", 1f);

            material.SetInt(
                "_SrcBlend",
                (int)UnityEngine.Rendering.BlendMode.SrcAlpha
            );

            material.SetInt(
                "_DstBlend",
                (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha
            );

            material.SetInt("_ZWrite", 0);

            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");

            material.renderQueue = 3000;
        }
    }

    private void OnValidate()
    {
        disappearDistance = Mathf.Max(0f, disappearDistance);
        appearDistance = Mathf.Max(
            disappearDistance,
            appearDistance
        );

        fadeDuration = Mathf.Max(0f, fadeDuration);
    }
}