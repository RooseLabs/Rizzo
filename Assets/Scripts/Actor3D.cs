using System.Collections;
using UnityEngine;

namespace RooseLabs
{
    public class Actor3D : MonoBehaviour
    {
        [SerializeField] protected Renderer actorRenderer;

        private bool m_isActorRendererAvailable = false;
        private GameObject m_actorObject;
        private Material[] m_materials;
        private Color[] m_materialOriginalColors;

        public Material[] Materials => m_materials;
        public Color[] MaterialOriginalColors => m_materialOriginalColors;

        private void Awake()
        {
            m_actorObject = transform.GetChild(0).gameObject;
            transform.rotation = Quaternion.Euler(-30, 0, 0);

            if (actorRenderer != null)
            {
                m_isActorRendererAvailable = true;
                m_materials = actorRenderer.materials;
                m_materialOriginalColors = new Color[m_materials.Length];

                for (int i = 0; i < m_materials.Length; i++)
                {
                    m_materialOriginalColors[i] = m_materials[i].color;
                }
            }
        }

        private void Update()
        {
            UpdateZPosition();
        }

        private void UpdateZPosition()
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.y * 10f
            );
        }

        /// <summary>
        ///   <para>Rotates the 3D object to look at the target position.</para>
        /// </summary>
        /// <param name="targetPos">The position to look at in world space.</param>
        /// <param name="smooth">Whether to rotate smoothly or instantly.</param>
        /// <param name="rotationSpeed">The speed of the rotation if smooth is true.</param>
        public void LookAt(Vector3 targetPos, bool smooth = false, float rotationSpeed = 10f)
        {
            float angleRad = Mathf.Atan2(targetPos.y - transform.position.y, (targetPos.x - transform.position.x) * 0.5f);
            Vector3 eulerRotation = m_actorObject.transform.localRotation.eulerAngles;
            eulerRotation.y = 90 - angleRad * Mathf.Rad2Deg;

            if (m_rotationCoroutine != null) StopCoroutine(m_rotationCoroutine);
            if (smooth)
            {
                m_rotationCoroutine = StartCoroutine(SmoothRotateCoroutine(Quaternion.Euler(eulerRotation), rotationSpeed));
            }
            else
            {
                m_actorObject.transform.localRotation = Quaternion.Euler(eulerRotation);
            }
        }

        private Coroutine m_rotationCoroutine;

        private IEnumerator SmoothRotateCoroutine(Quaternion targetRotation, float rotationSpeed)
        {
            while (Quaternion.Angle(m_actorObject.transform.localRotation, targetRotation) > 0.01f)
            {
                m_actorObject.transform.localRotation = Quaternion.Lerp(
                    m_actorObject.transform.localRotation,
                    targetRotation,
                    Time.deltaTime * rotationSpeed
                );
                yield return null;
            }
            m_actorObject.transform.localRotation = targetRotation;
        }

        public Vector2 GetFacingDirection()
        {
            return m_actorObject.transform.forward;
        }

        public void FadeOut(float delay = 2.5f, bool destroy = true)
        {
            if (m_isActorRendererAvailable)
            {
                StartCoroutine(FadeOutAndDestroyCoroutine(delay, destroy));
            }
            else if (destroy)
            {
                Destroy(gameObject);
            }
        }

        private IEnumerator FadeOutAndDestroyCoroutine(float delay, bool destroy)
        {
            yield return new WaitForSeconds(delay);
            if (m_isActorRendererAvailable)
            {
                for (int i = 0; i < m_materials.Length; i++)
                {
                    MakeMaterialTransparent(m_materials[i]);
                }
                float fadeDuration = 1f;
                float elapsedTime = 0f;

                while (elapsedTime < fadeDuration)
                {
                    float t = elapsedTime / fadeDuration;
                    float alpha = Mathf.Lerp(1f, 0f, t);
                    for (int i = 0; i < m_materials.Length; i++)
                    {
                        Color color = m_materialOriginalColors[i];
                        color.a = alpha;
                        m_materials[i].color = color;
                    }
                    elapsedTime += Time.deltaTime;
                    yield return null;
                }
            }
            if (destroy) Destroy(gameObject);
        }

        private static void MakeMaterialTransparent(Material material)
        {
            material.SetFloat("_Surface", 1.0f); // Transparent surface mode
            material.SetFloat("_ZWrite", 0.0f);
            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_SrcBlendAlpha", (float)UnityEngine.Rendering.BlendMode.One);
            material.SetFloat("_DstBlendAlpha", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetFloat("_Cull", (float)UnityEngine.Rendering.CullMode.Back);
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            material.EnableKeyword("_SURFACE_TYPE_TRANSPARENT");
        }
    }
}
