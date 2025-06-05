using UnityEngine;

namespace RooseLabs.StateMachine
{
    public abstract class StateMachine<TState> where TState : BaseState
    {
        private TState m_currentState;

        public TState CurrentState => m_currentState;

        public void Initialize(TState initialState)
        {
            m_currentState = initialState;
            OnEnter();
        }

        protected virtual void OnEnter()
        {
            m_currentState?.OnEnter();
        }

        public virtual void Update()
        {
            m_currentState?.Update();
        }

        public virtual void FixedUpdate()
        {
            m_currentState?.FixedUpdate();
        }

        public virtual void LateUpdate()
        {
            m_currentState?.LateUpdate();
        }

        public virtual void OnCollisionEnter2D(Collision2D collision)
        {
            m_currentState?.OnCollisionEnter2D(collision);
        }

        public virtual void OnCollisionStay2D(Collision2D collision)
        {
            m_currentState?.OnCollisionStay2D(collision);
        }

        public virtual void OnCollisionExit2D(Collision2D collision)
        {
            m_currentState?.OnCollisionExit2D(collision);
        }

        public virtual void OnTriggerEnter2D(Collider2D other)
        {
            m_currentState?.OnTriggerEnter2D(other);
        }

        public virtual void OnTriggerStay2D(Collider2D other)
        {
            m_currentState?.OnTriggerStay2D(other);
        }

        public virtual void OnTriggerExit2D(Collider2D other)
        {
            m_currentState?.OnTriggerExit2D(other);
        }

        protected virtual void OnExit()
        {
            m_currentState?.OnExit();
        }

        public void ChangeState(TState state)
        {
            if (CurrentState == state) return;
            OnExit();
            m_currentState = state;
            OnEnter();
        }
    }
}
