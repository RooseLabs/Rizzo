using UnityEngine;

namespace RooseLabs
{
    public class Actor3D : MonoBehaviour
    {
        private GameObject m_actorObject;
        private Renderer m_aObjectRenderer;
        private float m_aObjectMinY;

        private void Awake()
        {
            m_actorObject = transform.GetChild(0).gameObject;
            m_aObjectRenderer = m_actorObject.GetComponentInChildren<Renderer>();
            m_aObjectMinY = m_aObjectRenderer.bounds.min.y;

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
                (transform.position.y - m_aObjectMinY) * 10f
            );
        }

        /// <summary>
        ///   <para>Rotates the 3D object to look at the target position.</para>
        /// </summary>
        /// <param name="targetPos">The position to look at in world space.</param>
        public void LookAt(Vector3 targetPos)
        {
            float angleRad = Mathf.Atan2(targetPos.y - transform.position.y, (targetPos.x - transform.position.x) * 0.5f);
            Vector3 eulerRotation = m_actorObject.transform.localRotation.eulerAngles;
            eulerRotation.y = 90 - angleRad * Mathf.Rad2Deg;
            m_actorObject.transform.localRotation = Quaternion.Euler(eulerRotation);
        }

        public Vector2 GetFacingDirection()
        {
            return m_actorObject.transform.forward;
        }
    }
}