using System.Collections;
using UnityEngine;

namespace RooseLabs
{
    public class Actor3D : MonoBehaviour
    {
        private GameObject m_actorObject;
        private float m_aObjectMinY;

        private void Awake()
        {
            m_actorObject = transform.GetChild(0).gameObject;

            transform.rotation = Quaternion.Euler(-30, 0, 0);
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
    }
}
