using UnityEngine;

namespace RooseLabs.StateMachine
{
    public interface IState
    {
        void OnEnter();
        void Update();
        void FixedUpdate();
        void LateUpdate();
        void OnCollisionEnter2D(Collision2D collision);
        void OnCollisionStay2D(Collision2D collision);
        void OnCollisionExit2D(Collision2D collision);
        void OnTriggerEnter2D(Collider2D other);
        void OnTriggerStay2D(Collider2D other);
        void OnTriggerExit2D(Collider2D other);
        void OnExit();
    }
}
