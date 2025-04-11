using RooseLabs.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RooseLabs
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Transform target;
        [SerializeField][Range(0.1f, 1f)] public float followSmoothTime = 0.1f;
        [SerializeField][Range(1f, 5f)] private float lookRadius = 2f;
        [SerializeField][Range(0.1f, 2f)] private float lookDamping = 0.5f;

        private Vector3 m_velocity = Vector3.zero;
        private Vector3 m_lookOffset = Vector3.zero;
        private bool m_isTargetSet;
        private bool m_followTarget = true;

        private InputManager m_inputManager;
        private InputAction m_lookAction;

        public bool FollowTarget
        {
            get => m_followTarget;
            set => m_followTarget = value;
        }

        private void Awake()
        {
            m_inputManager = InputManager.Instance;
            m_lookAction = m_inputManager.GameplayActions.Look;
            m_isTargetSet = target != null;
        }

        private void FixedUpdate()
        {
            if (m_isTargetSet && m_followTarget)
            {
                Vector3 targetPosition = target.position;
                targetPosition.z = transform.position.z;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition + m_lookOffset, ref m_velocity, followSmoothTime);
            }
        }

        private void LateUpdate()
        {
            if (m_isTargetSet && m_inputManager.IsCurrentDeviceKeyboardAndMouse())
            {
                Vector2 mouseScreenPos = m_lookAction.ReadValue<Vector2>();
                Vector2 screenCenter = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                Vector2 offset = mouseScreenPos - screenCenter;

                // Normalize to -1 to 1 range
                offset /= screenCenter;

                // Calculate desired look offset
                Vector3 desiredLookOffset = new Vector3(offset.x, offset.y, 0f) * lookRadius;

                // Apply exponential slowdown
                float t = desiredLookOffset.magnitude / lookRadius;
                float scale = 1f - Mathf.Exp(-t * 3f);
                desiredLookOffset *= scale;
                m_lookOffset = Vector3.Lerp(m_lookOffset, desiredLookOffset, lookDamping * Time.deltaTime);
            }
            else
            {
                // Reset look offset when not using mouse/keyboard
                m_lookOffset = Vector3.Lerp(m_lookOffset, Vector3.zero, lookDamping * Time.deltaTime);
            }
        }
    }
}
