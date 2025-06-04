using UnityEngine;

namespace RooseLabs.StateMachine
{
    public abstract class BaseState : IState
    {
        public virtual void OnEnter() {}
        public virtual void Update() {}
        public virtual void FixedUpdate() {}
        public virtual void LateUpdate() {}
        public virtual void OnCollisionEnter2D(Collision2D collision) {}
        public virtual void OnCollisionStay2D(Collision2D collision) {}
        public virtual void OnCollisionExit2D(Collision2D collision) {}
        public virtual void OnTriggerEnter2D(Collider2D other) {}
        public virtual void OnTriggerStay2D(Collider2D other) {}
        public virtual void OnTriggerExit2D(Collider2D other) {}
        public virtual void OnExit() {}
    }
}
