using UnityEngine;

namespace RooseLabs
{
    public class SpriteDepthSolver : MonoBehaviour
    {
        [SerializeField] private float zOffset = 0.0f;

        private void Awake()
        {
            UpdateZPosition();
        }

        private void OnValidate()
        {
            UpdateZPosition();
        }

        private void UpdateZPosition()
        {
            transform.position = new Vector3(
                transform.position.x,
                transform.position.y,
                transform.position.y * 10.0f + zOffset
            );
        }
    }
}
