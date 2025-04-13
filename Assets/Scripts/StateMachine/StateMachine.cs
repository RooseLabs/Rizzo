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
            m_currentState.OnEnter();
        }

        public virtual void Update()
        {
            m_currentState.Update();
        }

        public virtual void FixedUpdate()
        {
            m_currentState.FixedUpdate();
        }

        protected virtual void OnExit()
        {
            m_currentState.OnExit();
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
