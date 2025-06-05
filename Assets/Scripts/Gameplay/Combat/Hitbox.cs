using System.Collections;
using UnityEngine;

namespace RooseLabs.Gameplay.Combat
{
    [RequireComponent(typeof(Collider2D))]
    public class Hitbox : MonoBehaviour
    {
        [SerializeField] private LayerMask hitLayerMask;
        [SerializeField] private bool disableOnImpact = true;

        private float m_damage = 0.0f;
        private Collider2D m_collider;
        private bool m_shouldDestroy = false;
        private Coroutine m_endPerformanceCoroutine;

        private void Awake()
        {
            m_collider = GetComponent<Collider2D>();
            m_collider.enabled = false;
            m_collider.isTrigger = true;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & hitLayerMask) == 0) return;
            var damageable = other.GetComponent<IDamageable>();
            if (damageable == null) return;
            if (disableOnImpact) m_collider.enabled = false;
            damageable.ApplyDamage(m_damage);
        }

        public void Perform(float damage, float duration)
        {
            m_damage = damage;
            m_collider.enabled = true;
            if (m_endPerformanceCoroutine != null) StopCoroutine(m_endPerformanceCoroutine);
            m_endPerformanceCoroutine = StartCoroutine(EndPerformanceAfter(duration));
        }

        private IEnumerator EndPerformanceAfter(float duration)
        {
            yield return new WaitForSeconds(duration);
            EndPerformance();
        }

        public void EndPerformance()
        {
            if (m_shouldDestroy) Destroy(gameObject);
            else m_collider.enabled = false;
        }

        // public static GameObject Create<T>(Vector2 position, Vector2 size, float damage, LayerMask hitLayerMask) where T : Collider2D
        // {
        //     GameObject hitboxObject = new GameObject("Hitbox");
        //
        //     T collider = hitboxObject.AddComponent<T>();
        //     collider.isTrigger = true;
        //     collider.enabled = false;
        //
        //     Hitbox hitbox = hitboxObject.AddComponent<Hitbox>();
        //     hitbox.transform.position = position;
        //     hitbox.transform.localScale = new Vector3(size.x, size.y, 1f);
        //     hitbox.hitLayerMask = hitLayerMask;
        //     hitbox.m_damage = damage;
        //     hitbox.m_shouldDestroy = true;
        //
        //     return Instantiate(hitboxObject);
        // }
    }
}
