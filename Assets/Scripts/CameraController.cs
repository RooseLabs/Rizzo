using RooseLabs.Events.Channels;
using RooseLabs.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RooseLabs
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField][Range(0.1f, 1f)] public float followSmoothTime = 0.1f;
        [SerializeField][Range(1f, 5f)] private float lookRadius = 2f;
        [SerializeField][Range(0.1f, 2f)] private float lookDamping = 0.5f;

        [Header("Listening to")]
        [SerializeField] private TransformEventChannelSO playerInstantiatedChannel;

        private Transform m_target;
        private bool m_isTargetSet;
        private bool m_followTarget = true;
        private Vector3 m_velocity = Vector3.zero;
        private Vector3 m_lookOffset = Vector3.zero;

        private InputManager m_inputManager;
        private InputAction m_lookAction;

        public Transform Target
        {
            get => m_target;
            set => m_target = value;
        }

        public bool FollowTarget
        {
            get => m_followTarget;
            set => m_followTarget = value;
        }

        private void Awake()
        {
            m_inputManager = InputManager.Instance;
            m_lookAction = m_inputManager.GameplayActions.Look;
        }

#if UNITY_EDITOR
        private void Start()
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) OnPlayerInstantiated(player.transform);
        }
#endif

        private void OnEnable()
        {
            playerInstantiatedChannel.OnEventRaised += OnPlayerInstantiated;
        }

        private void OnDisable()
        {
            playerInstantiatedChannel.OnEventRaised -= OnPlayerInstantiated;
        }

        private void FixedUpdate()
        {
            if (m_isTargetSet && m_followTarget)
            {
                Vector3 targetPosition = m_target.position;
                targetPosition.z = transform.position.z;
                transform.position = Vector3.SmoothDamp(transform.position, targetPosition + m_lookOffset, ref m_velocity, followSmoothTime);
            }
        }

        private void LateUpdate()
        {
            if (m_isTargetSet && m_inputManager.IsCurrentDeviceKeyboardAndMouse())
            {
                #if UNITY_EDITOR
                Vector2 mouseScreenPos = Application.isFocused
                    ? m_lookAction.ReadValue<Vector2>()
                    : new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);
                #else
                Vector2 mouseScreenPos = m_lookAction.ReadValue<Vector2>();
                #endif
                // If the mouse position is outside the screen bounds, return early
                if (mouseScreenPos.x < 0f || mouseScreenPos.y < 0f || mouseScreenPos.x > Screen.width || mouseScreenPos.y > Screen.height)
                {
                    #if UNITY_EDITOR
                    m_lookOffset = Vector3.zero;
                    #endif
                    return;
                }
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

        private void OnPlayerInstantiated(Transform playerTransform)
        {
            m_target = playerTransform;
            m_isTargetSet = true;
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }
    }
}
